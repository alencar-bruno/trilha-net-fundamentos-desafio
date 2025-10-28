using Newtonsoft.Json;

namespace DesafioFundamentos.Models
{
    public class ParkingLotSpecs
    {
        [JsonProperty("FixedValue")]
        public decimal FixedTax { get; set; }
        [JsonProperty("PerHourValue")]
        public decimal PerHourTax { get; set; }
    }
}