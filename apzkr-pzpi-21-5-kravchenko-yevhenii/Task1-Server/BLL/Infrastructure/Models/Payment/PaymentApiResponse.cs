using Newtonsoft.Json;

namespace BLL.Infrastructure.Models.Payment;
public class PaymentApiResponse
{
    [JsonProperty("data")]
    public string Data { get; set; } = null!;

    [JsonProperty("signature")]
    public string Signature { get; set; } = null!;

    [JsonProperty("apiKey")]
    public Guid ApiKey { get; set; }
}
