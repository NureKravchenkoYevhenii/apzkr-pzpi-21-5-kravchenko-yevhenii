namespace Infrastructure.Configs;
public class PaymentCreds
{
    public Guid PosId { get; set; }

    public string EndpointsKey { get; set; } = null!;

    public Guid ApiKey { get; set; }

    public string ApiSecret { get; set; } = null!;

    public string PaymentUrl { get; set; } = null!;

    public string? ServerUrl { get; set; }

    public bool IsTestMode { get; set; }
}
