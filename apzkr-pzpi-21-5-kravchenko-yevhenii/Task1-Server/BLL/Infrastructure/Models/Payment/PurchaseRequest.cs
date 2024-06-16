using BLL.Infrastructure.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BLL.Infrastructure.Models.Payment;
internal class PurchaseRequest
{
    [JsonProperty("pos_id")]
    internal Guid PosId { get; set; }

    [JsonProperty("mode")]
    [JsonConverter(typeof(StringEnumConverter))]
    internal PaymentMode PaymentMode { get; set; }

    [JsonProperty("method")]
    [JsonConverter(typeof(StringEnumConverter))]
    internal PaymentMethod PaymentMethod { get; set; }

    [JsonProperty("amount")]
    internal int Amount { get; set; }

    [JsonProperty("currency")]
    [JsonConverter(typeof(StringEnumConverter))]
    internal Currency Currency { get; set; }

    [JsonProperty("description")]
    internal string Description { get; set; } = null!;

    [JsonProperty("order_id")]
    internal Guid OrderId { get; set; }

    [JsonProperty("order_3ds_bypass")]
    [JsonConverter(typeof(StringEnumConverter))]
    internal Order3DsBypass Order3DsBypass { get; set; }

    [JsonProperty("customer_email")]
    internal string CustomerEmail { get; set; } = string.Empty;

    [JsonProperty("customer_phone")]
    internal string CustomerPhone { get; set; } = string.Empty;

    [JsonProperty("result_url")]
    internal string? ResultUrl { get; set; }

    [JsonProperty("server_url")]
    internal string? ServerUrl { get; set; }

    [JsonProperty("payload")]
    internal string? Payload { get; set; }
}
