using BLL.Contracts;
using Domain.Models;
using Domain.Models.PlateRecognition;
using Infrastructure.Configs;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace BLL.Services;
public class Client : IClient
{
    private const string LOGIN_URL = "auth/login";
    private const string CHECK_IN_URL = "parking-sessions/check-in";
    private const string CHECK_OUT_URL = "parking-sessions/check-out";

    private static Token? _authorizeToken;
    private readonly string _apiUrl;
    private readonly PlateRecognitionConfig _plateRecognitionConfig;
    private readonly ISerialClient _serialClient;

    public Client(
        ServerSettings serverSettings,
        PlateRecognitionConfig plateRecognitionConfig,
        ISerialClient serialClient)
    {
        _apiUrl = serverSettings.ApiUrl;
        _plateRecognitionConfig = plateRecognitionConfig;
        _serialClient = serialClient;
    }

    public Token Login(LoginModel loginModel)
    {
        var jsonRequestBody = JsonConvert.SerializeObject(loginModel);
        var content = new StringContent(
            jsonRequestBody,
            Encoding.UTF8,
            "application/json");

        var token = Post<Token>(
            _apiUrl + LOGIN_URL,
            content);

        _authorizeToken = token;

        return token!;
    }

    public bool CheckIn()
    {
        var carNumber = GetCarNumber();

        var authHeader = new AuthenticationHeaderValue(
            "Bearer",
            _authorizeToken?.AccessToken);
        var content = new StringContent(
            JsonConvert.SerializeObject(carNumber),
            Encoding.UTF8,
            "application/json");
        
        var result = Post<bool>(
            _apiUrl + CHECK_IN_URL,
            content,
            authHeader);

        if (result)
            _serialClient.OpenGate();
    
        return result;

        throw new Exception("Error occured");
    }

    public bool CheckOut()
    {
        var carNumber = GetCarNumber();

        var authHeader = new AuthenticationHeaderValue(
            "Bearer",
            _authorizeToken?.AccessToken);
        var content = new StringContent(
            JsonConvert.SerializeObject(carNumber),
            Encoding.UTF8,
            "application/json");

        var result = Post<bool>(
            _apiUrl + CHECK_OUT_URL,
            content,
            authHeader);

        if (result)
            _serialClient.OpenGate();

        return result;
    }

    public void Logout()
    {
        _authorizeToken!.AccessToken = string.Empty;
        _authorizeToken!.RefreshToken = string.Empty;
    }

    private string? GetCarNumber()
    {
        var imgBytes = _serialClient.CaptureImage();
        var carNumber = GetCarNumberFromImage(imgBytes);

        return carNumber;
    }

    private string GetCarNumberFromImage(byte[] imgBytes)
    {
        var authHeader = new AuthenticationHeaderValue(
            "Token",
            _plateRecognitionConfig.Token);

        var formData = new MultipartFormDataContent();
        var base64Img = Convert.ToBase64String(imgBytes);

        formData.Add(new StringContent(base64Img), "upload");

        var result = Post<PlateRecognitionResult>(
            _plateRecognitionConfig.ApiUrl,
            formData,
            authHeader);

        if (result.Results.Count > 0)
        {
            var carNumber = result.Results.First().Plate;

            return carNumber.ToUpper();
        }

        return string.Empty;
    }

    private T Post<T>(
        string url,
        HttpContent content,
        AuthenticationHeaderValue? authHeader = null)
            where T : new()
    {
        using var client = new HttpClient();

        client.DefaultRequestHeaders.Add(
            "Accept-Language",
            Thread.CurrentThread.CurrentCulture.Name);
        if (authHeader != null)
            client.DefaultRequestHeaders.Authorization = authHeader;

        var response = client.PostAsync(url, content).Result;
        var responseString = response.Content.ReadAsStringAsync().Result;

        if (response.IsSuccessStatusCode)
        {
            var result = JsonConvert.DeserializeObject<T>(responseString)!;
            return result;
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest
            || response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var error = JsonConvert.DeserializeObject<ApiError>(responseString)!;
            throw new Exception(error.Message);
        }

        return new T();
    }
}
