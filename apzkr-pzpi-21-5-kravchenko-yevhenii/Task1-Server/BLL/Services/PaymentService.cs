using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Enums;
using BLL.Infrastructure.Models.Payment;
using DAL.Contracts;
using Domain.Models;
using Infrastructure.Configs;
using Infrastructure.Enums;
using Infrastructure.Exceptions;
using Infrastructure.Resources;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Globalization;
using System.Net;
using System.Text;

namespace BLL.Services;
public class PaymentService : IPaymentService
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<IMapper> _mapper;
    private readonly Lazy<PaymentCreds> _paymentCreds;
    private readonly Lazy<IUserMembershipService> _userMembershipService;
    private readonly Lazy<IRepository<Payment>> _payments;
    private readonly Lazy<IRepository<ParkingSession>> _parkingSessions;

    public PaymentService(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IMapper> mapper,
        Lazy<PaymentCreds> paymentCreds,
        Lazy<IUserMembershipService> userMembershipService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _paymentCreds = paymentCreds;
        _userMembershipService = userMembershipService;
        _payments = _unitOfWork.Value.GetLazyRepository<Payment>();
        _parkingSessions = _unitOfWork.Value.GetLazyRepository<ParkingSession>();
    }

    public void Add(PaymentModel paymentModel)
    {
        var payment = _mapper.Value.Map<Payment>(paymentModel);

        _payments.Value.Add(payment);
    }

    public void Delete(int paymentId)
    {
        var payment = _payments.Value.GetById(paymentId);
        if (payment == null)
            return;

        _payments.Value.Remove(payment);
    }

    public PaymentModel GetById(int paymentId)
    {
        var payment = _payments.Value.GetAll()
            .Include(p => p.User)
            .ThenInclude(u => u.UserProfile)
            .FirstOrDefault(p => p.Id == paymentId)
                ?? throw new EntityNotFoundException("PAYMENT_NOT_FOUND");

        var paymentModel = _mapper.Value.Map<PaymentModel>(payment);

        return paymentModel;
    }

    public IEnumerable<PaymentModel> GetUserPayments(int userId)
    {
        var payments = _payments.Value.GetAll()
            .Include(p => p.User)
            .ThenInclude(u => u.UserProfile)
            .Where(p => p.UserId == userId)
            .ToList();

        var paymentModels = _mapper.Value.Map<List<PaymentModel>>(payments);

        return paymentModels;
    }

    public async Task<string> MakePayment(int paymentId)
    {
        var payment = _payments.Value
            .GetById(paymentId)
                ?? throw new ParkyException("PAYMENT_NOT_FOUND");

        var payload = new PayloadModel()
        {
            PayItemId = payment.Id,
            PurchaseType = PurchaseType.PayForParkingSession,
            UserId = payment.UserId
        };
        var payloadString = JsonConvert.SerializeObject(payload);

        var paymentUrl = await MakePayment(payment.Sum, payloadString);

        return paymentUrl;
    }

    public async Task<string> MakePayment(decimal sum, string? payload = null)
    {
        var purchaseRequest = GetPurchaseRequest(sum, payload);

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add(
                "X-API-AUTH",
                $"CPAY {_paymentCreds.Value.ApiKey}:{_paymentCreds.Value.ApiSecret}");
            client.DefaultRequestHeaders.Add(
                "X-API-KEY",
                $"{_paymentCreds.Value.EndpointsKey}");

            var jsonData = JsonConvert.SerializeObject(
                purchaseRequest, 
                new JsonSerializerSettings 
                { 
                    NullValueHandling = NullValueHandling.Ignore 
                });
            var content = new StringContent(
                jsonData,
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(_paymentCreds.Value.PaymentUrl, content);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new ParkyException("PAYMENT_REQUEST_FAILED");

            var paymentUrl = response.RequestMessage?.RequestUri;

            return paymentUrl?.ToString() ?? string.Empty;
        }
    }

    public void ProcessPayment(PaymentApiResponse paymentResponse)
    {
        var paymentDataBytes = Convert.FromBase64String(paymentResponse.Data);
        var paymentData = Encoding.UTF8.GetString(paymentDataBytes);
        var purchaseResponse = JsonConvert.DeserializeObject<PurchaseResponse>(paymentData);

        var payload = JsonConvert.DeserializeObject<PayloadModel>(purchaseResponse!.Payload!);

        if (purchaseResponse.PaymentStatus == PaymentStatus.success
            && payload != null 
            && payload.PurchaseType == PurchaseType.PurchaseMembership
            && payload.UserId.HasValue)
        {
            _userMembershipService.Value.Add(payload.PayItemId, payload.UserId.Value);

            _payments.Value.Add(new Payment
            {
                Transaction = purchaseResponse.OrderId.ToString(),
                Sum = (decimal)purchaseResponse.Amount / 100,
                PayDate = DateTime.UtcNow,
                Description = "",
                PurchaseType = payload.PurchaseType,
                UserId = payload.UserId.Value,
            });
        }

        if (purchaseResponse.PaymentStatus == PaymentStatus.success
            && payload != null
            && payload.PurchaseType == PurchaseType.PayForParkingSession)
        {
            var payment = _payments.Value.GetById(payload.PayItemId)!;

            payment.Transaction = purchaseResponse.OrderId.ToString();
            payment.PayDate = DateTime.UtcNow;

            _payments.Value.Update(payment);
        }

        Console.WriteLine(JsonConvert.SerializeObject(purchaseResponse, Formatting.Indented));
    }

    public void Update(PaymentModel paymentModel)
    {
        var payment = _payments.Value.GetById(paymentModel.Id)
            ?? throw new EntityNotFoundException("PAYMENT_NOT_FOUND");

        _mapper.Value.Map(paymentModel, payment);
        _payments.Value.Update(payment);
    }

    public byte[] GetPaymentStatistics(DateTime from, DateTime to)
    {
        from = from.ToUniversalTime();
        to = to.ToUniversalTime();

        var payments = _payments.Value
            .GetAll()
            .Where(p => p.PayDate.HasValue && p.PayDate >= from && p.PayDate <= to)
            .ToList();

        var statistics = payments.GroupBy(p => p.PayDate!.Value.Date)
            .Select(g => new
            {
                Date = g.Key,
                Amout = g.Sum(p => p.Sum)
            });

        var totalAmount = statistics.Sum(s => s.Amout);

        var purchaseTypeStatistics = payments.GroupBy(p => p.PurchaseType)
            .Select(g => new
            {
                Type = g.Key.ToString(),
                Amount = g.Sum(p => p.Sum)
            });

        using var excelPackage = new ExcelPackage();
        using var worksheet = excelPackage.Workbook.Worksheets.Add(
            Resources.Get("PAYMENT_STATISTICS"));

        worksheet.Cells["A1"].Value = Resources.Get("DATE");
        worksheet.Cells["B1"].Value = Resources.Get("AMOUNT");

        var row = 2;
        foreach (var stat in statistics)
        {
            worksheet.Cells[$"A{row}"].Value = stat.Date.ToLocalTime().ToShortDateString();
            worksheet.Cells[$"B{row}"].Value = stat.Amout;
            row++;
        }

        worksheet.Cells[$"A{row}"].Value = Resources.Get("TOTAL");
        worksheet.Cells[$"B{row}"].Value = totalAmount;

        row += 2;

        worksheet.Cells[$"A{row}"].Value = Resources.Get("PURCHASE_TYPE");
        worksheet.Cells[$"B{row}"].Value = Resources.Get("AMOUNT");

        row++;

        foreach (var typeStat in purchaseTypeStatistics)
        {
            worksheet.Cells[$"A{row}"].Value = Resources.Get(typeStat.Type.ToString());
            worksheet.Cells[$"B{row}"].Value = typeStat.Amount;
            row++;
        }

        worksheet.Cells[$"A1:B{row}"].AutoFitColumns();

        var excelBytes = excelPackage.GetAsByteArray();

        return excelBytes;
    }

    private PurchaseRequest GetPurchaseRequest(decimal sum, string? payload)
    {
        var currentRegionCurrency = new RegionInfo(
            Thread.CurrentThread.CurrentUICulture.LCID).ISOCurrencySymbol;

        // USD is used as default currency
        // if current region is not supported
        var currency = Currency.USD;
        if (Enum.TryParse<Currency>(currentRegionCurrency, out var res))
        {
            currency = res;
        }

        var purchaseRequest = new PurchaseRequest
        {
            PosId = _paymentCreds.Value.PosId,
            PaymentMode = PaymentMode.hosted,
            PaymentMethod = PaymentMethod.purchase,
            Amount = _paymentCreds.Value.IsTestMode
                ? 1
                : (int)(sum * 100),
            Currency = _paymentCreds.Value.IsTestMode
                ? Currency.XTS
                : currency,
            Description = string.Empty,
            OrderId = Guid.NewGuid(),
            Order3DsBypass = Order3DsBypass.supported,
            CustomerEmail = string.Empty,
            CustomerPhone = string.Empty,
            ResultUrl = null,
            ServerUrl = _paymentCreds.Value.ServerUrl,
            Payload = payload
        };

        return purchaseRequest;
    }
}
