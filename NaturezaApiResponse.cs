using Newtonsoft.Json;

namespace ApiProtheusConsumer
{
    public class NaturezaApiResponse
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Codigo")]
        public string Codigo { get; set; }
        [JsonProperty("Natureza")]
        public string Natureza { get; set; }
    }
}
