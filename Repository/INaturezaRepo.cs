using ApiProtheusConsumer.Model;

namespace ApiProtheusConsumer.Repository
{
    public interface INaturezaRepo
    {
        public string GerarToken(string usuario);
        public List<string> ObterNatureza();        
        public string ObterNatureza(string codigo);
        public bool ValidarToken(string token);
     
    }
}
