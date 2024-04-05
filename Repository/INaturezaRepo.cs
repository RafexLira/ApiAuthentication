using ApiProtheusConsumer.Model;
using Microsoft.IdentityModel.Tokens;

namespace ApiProtheusConsumer.Repository
{
    public interface INaturezaRepo
    {
        public string GerarToken(string usuario);
        public List<NaturezaApiResponse> ObterNatureza();        
        public string ObterNatureza(string codigo);
        public bool ValidarToken(string token);       
     
    }
}
