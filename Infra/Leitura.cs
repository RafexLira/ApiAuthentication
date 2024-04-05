using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiProtheusConsumer.Infra
{
    public static class Leitura
    {
        public static List<NaturezaApiResponse> LerArquivoJson(string pathJson)
        {
            using (StreamReader reader = new StreamReader(pathJson))
            {
                string naturezaJson = reader.ReadToEnd();

               return JsonConvert.DeserializeObject<List<NaturezaApiResponse>>(naturezaJson);
                
            }
        }
    }
}
