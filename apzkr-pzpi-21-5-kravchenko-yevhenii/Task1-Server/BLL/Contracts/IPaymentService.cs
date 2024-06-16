using BLL.Infrastructure.Models.Payment;

namespace BLL.Contracts;
public interface IPaymentService
{
    IEnumerable<PaymentModel> GetUserPayments(int userId);

    PaymentModel GetById(int paymentId);

    void Add(PaymentModel paymentModel);

    void Update(PaymentModel paymentModel);

    void Delete(int paymentId);

    Task<string> MakePayment(decimal sum, string? payload = null);

    Task<string> MakePayment(int paymentId);

    void ProcessPayment(PaymentApiResponse paymentResponse);

    byte[] GetPaymentStatistics(DateTime from, DateTime to);
}
