using Newtonsoft.Json.Linq;

namespace ApiProtheusConsumer.Infra
{
    public static class Leitura
    {
        public static JObject LerArquivoJson(string caminho)
        {
            using (StreamReader reader = new StreamReader(caminho))
            {
                string json = reader.ReadToEnd();
                return JObject.Parse(json);
            }
        }
    }
}
