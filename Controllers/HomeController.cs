﻿using ApiProtheusConsumer.Infra;
using ApiProtheusConsumer.Model;
using ApiProtheusConsumer.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
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

        [Authorize]
        [HttpGet("/ObterDados")]
        public IActionResult ObterDados()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer", "");          

            var x = _naturezaRepo.ObterNatureza();
            return Ok(x);
        }

        [HttpPost("/Authentication")]
        public IActionResult Authentication(Usuario usuario)
        {
            if (usuario.Nome == "Rafael" && usuario.Senha == "123456" && usuario.Role == "Usuario")
            {
                return Ok(_naturezaRepo.GerarToken(usuario.Nome));
            }
            return Ok("Usuario não encontrado!");
        }

        [Authorize]
        [HttpPost("/ObterDados/{codigo}")]
        public IActionResult ObterDados(string codigo)
        {
            return Ok(_naturezaRepo.ObterNatureza(codigo));
        }

    }
}
