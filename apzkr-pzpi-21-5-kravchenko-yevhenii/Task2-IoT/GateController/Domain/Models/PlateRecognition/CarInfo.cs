using Newtonsoft.Json;

namespace Domain.Models.PlateRecognition;
public class CarInfo
{
    [JsonProperty("plate")]
    public string Plate { get; set; } = null!;
}
