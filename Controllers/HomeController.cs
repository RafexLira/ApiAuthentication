using ApiProtheusConsumer.Infra;
using ApiProtheusConsumer.Model;
using ApiProtheusConsumer.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Refit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiProtheusConsumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class HomeController : ControllerBase
    {
        private readonly INaturezaRepo _naturezaRepo;

        public HomeController(INaturezaRepo naturezaRepo)
        {
            _naturezaRepo = naturezaRepo;
        }

        [HttpPost("/Authentication")]
        public IActionResult Authentication(Usuario usuario)
        {
            if (usuario.Nome == "rafael.lira" && usuario.Senha == "%rftq830%" && usuario.Role == "adm")
            {
                var token = _naturezaRepo.GerarToken(usuario.Nome);

               // var x = JsonConvert.SerializeObject(token, Formatting.Indented);                            

                return Ok(token);
            }
            return Ok("Usuario não encontrado!");
        }
               
        [HttpGet("/ObterDados")]
        public IActionResult ObterDados([Header("Authorization")] string token)
        {
           // token = Request.Headers["Authorization"].ToString().Replace("Bearer", "");          
            var naturezaReturn = _naturezaRepo.ObterNatureza();          

            return Ok(naturezaReturn);
        }
       
        [HttpPost("/ObterDados/{codigo}")]
        public IActionResult ObterDadosCodigo(string codigo)
        {
            return Ok(_naturezaRepo.ObterNatureza(codigo));
        }

    }
}
