using ApiProtheusConsumer.Infra;
using ApiProtheusConsumer.Model;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApiProtheusConsumer.Repository
{
    public class NaturezaRepo : INaturezaRepo
    {
        private readonly IConfiguration _configuration;     

        public NaturezaRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GerarToken(string usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, usuario),              
                }),
                Expires = DateTime.UtcNow.AddMinutes(10), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),                
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]                
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            //var tokenString = tokenHandler.WriteToken(token);

            return tokenHandler.WriteToken(token);

            //    // Salt
            //    //var rng = new RNGCryptoServiceProvider();
            //    //var saltBytes = new byte[16]; 
            //    //rng.GetBytes(saltBytes);
            //    //var TokenSalt = Convert.ToBase64String(saltBytes);
            //    // using var hmac = HMACSHA512(string.Concat(Guid.NewGuid().ToString(), DateTime.Now, _configuration["SecretKey"]));  
        }
      
        public List<string> ObterNatureza()
        {
            string naturezaOperacao;

            var path = _configuration["_Path"];

            List<string> result = new List<string>();

            JObject jsonObject = Leitura.LerArquivoJson(path);
            var natureza = jsonObject;

            foreach (var item in natureza)
            {
                naturezaOperacao = item.ToString().Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "");

                result.Add(naturezaOperacao);
            }

            return result;
        }
        public string ObterNatureza(string codigo) 
        {
            var path = _configuration["_Path"];
            JObject jsonObject = Leitura.LerArquivoJson(path);

            foreach (var y in jsonObject )
            {
                if (y.Key == codigo)
                {
                    return y.ToString();
                }
            }
            return "Natureza de operação não encontrada!";
        }

        public bool ValidarToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey =  new SymmetricSecurityKey(key),
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero

                }, out SecurityToken validatedToken);

                return true; // Token válido
            }
            catch (SecurityTokenException ex)
            {               
                Console.WriteLine($"Token inválido: {ex.Message}");
                return false;
            }
        }       
      
    }
}
