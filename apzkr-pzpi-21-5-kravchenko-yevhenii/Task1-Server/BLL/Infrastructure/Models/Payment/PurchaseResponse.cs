using BLL.Infrastructure.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BLL.Infrastructure.Models.Payment;
public class PurchaseResponse
{
    [JsonProperty("registry_ref_no")]
    public string? RegistryRefNo { get; set; }

    [JsonProperty("status_code")]
    public string StatusCode { get; set; } = null!;

    [JsonProperty("fee")]
    public string? Fee { get; set; }

    [JsonProperty("customer_ip")]
    public string? CustomerIp { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; } = null!;

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("cc_token")]
    public string CcToken { get; set; } = null!;

    [JsonProperty("customer_fname")]
    public string? CustomerFname { get; set; }

    [JsonProperty("mcc")]
    public string? Mcc { get; set; }

    [JsonProperty("cc_mask")]
    public string CcMask { get; set; } = null!;

    [JsonProperty("mode")]
    [JsonConverter(typeof(StringEnumConverter))]
    public PaymentMode Mode { get; set; }

    [JsonProperty("options_3ds")]
    public Order3DsBypass Options3ds { get; set; }

    [JsonProperty("payload")]
    public string? Payload { get; set; }

    [JsonProperty("pos_id")]
    public Guid PosId { get; set; }

    [JsonProperty("customer_user_agent")]
    public string? CustomerUserAgent { get; set; }

    [JsonProperty("payment_id")]
    public Guid PaymentId { get; set; }

    [JsonProperty("processed_at")]
    public DateTime? ProcessedAt { get; set; }

    [JsonProperty("currency")]
    [JsonConverter(typeof(StringEnumConverter))]
    public Currency Currency { get; set; }

    [JsonProperty("_version")]
    public string? Version { get; set; }

    [JsonProperty("transaction_id")]
    public Guid TransactionId { get; set; }

    [JsonProperty("user_action_url")]
    public string? UserActionUrl { get; set; }

    [JsonProperty("billing_order_id")]
    public string? BillingOrderId { get; set; }

    [JsonProperty("status_description")]
    public string StatusDescription { get; set; } = null!;

    [JsonProperty("result_url")]
    public string? ResultUrl { get; set; }

    [JsonProperty("amount")]
    public int Amount { get; set; }

    [JsonProperty("method")]
    [JsonConverter(typeof(StringEnumConverter))]
    public PaymentMethod Method { get; set; }

    [JsonProperty("processed_amount")]
    public int? ProcessedAmount { get; set; }

    [JsonProperty("customer_phone")]
    public string CustomerPhone { get; set; } = null!;

    [JsonProperty("payway")]
    public string? Payway { get; set; }

    [JsonProperty("gateway_order_id")]
    public Guid GatewayOrderId { get; set; }

    [JsonProperty("entity_id")]
    public string? EntityId { get; set; }

    [JsonProperty("auth_code")]
    public string? AuthCode { get; set; }

    [JsonProperty("processed_currency")]
    public string? ProcessedCurrency { get; set; }

    [JsonProperty("customer_email")]
    public string CustomerEmail { get; set; } = null!;

    [JsonProperty("customer_lname")]
    public string? CustomerLname { get; set; }

    [JsonProperty("customer_id")]
    public string? CustomerId { get; set; }

    [JsonProperty("order_id")]
    public Guid OrderId { get; set; }

    [JsonProperty("status")]
    [JsonConverter(typeof(StringEnumConverter))]
    public PaymentStatus PaymentStatus { get; set; }
}
