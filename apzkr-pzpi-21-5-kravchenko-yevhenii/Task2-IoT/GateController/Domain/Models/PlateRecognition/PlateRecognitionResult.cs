using Newtonsoft.Json;

namespace Domain.Models.PlateRecognition;
public class PlateRecognitionResult
{
    [JsonProperty("results")]
    public List<CarInfo> Results { get; set; } = new List<CarInfo>();
}
