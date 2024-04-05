using ApiProtheusConsumer.Infra;
using ApiProtheusConsumer.Model;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
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


        //public string GerarToken(string usuario)
        //{
        //    var _tokenScheme = usuario;
        //    var combinePatch = DateTime.Now.ToString("yyyyMMdd");
        //    var combinePatchCalc = $"{DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day}";

        //    var token = string.Concat(_tokenScheme, combinePatch, combinePatchCalc);

        //    string hash;
        //    SecurityKey newKey = null;
        //    using (var md5 = System.Security.Cryptography.MD5.Create())
        //    {
        //        hash = String.Concat(md5.ComputeHash(Encoding.Default.GetBytes(token)).Select(x => x.ToString("x3")));
        //    }
        //    HttpClient httpClient = new HttpClient();
        //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario);


        //}


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

        public List<NaturezaApiResponse> ObterNatureza()
        {        
            List<NaturezaApiResponse> result = new List<NaturezaApiResponse>();

            var naturezaReturn = Leitura.LerArquivoJson(_configuration["_Path"]);                        

            return naturezaReturn;
        }
        public string ObterNatureza(string codigo)
        {
            var jsonObject = Leitura.LerArquivoJson(_configuration["_Path"]);

            foreach (var itemNatureza in jsonObject)
            {
                if (itemNatureza.Codigo == codigo)
                {
                    return itemNatureza.ToString();
                }
            }
            return "Natureza de operação não encontrada!";
        }

        public bool ValidarToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();            

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])),
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
