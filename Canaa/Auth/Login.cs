using Canaa.AppHost.utils;
using Canaa.DataContracts.Auth;
using Canaa.FN.BusinessComponents.Auth;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Canaa.Auth
{
    [ApiController]
    [Route("v1/[controller]")]
    public class Login : ControllerBase
    {
        private readonly IConfiguration _config;

        public Login(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult LoginJwt([FromBody] ParamUser param)
        {
            var credentials = new UserLogin
            {
                Email = param.Email,
                Senha = param.Senha
            };

            var loginComponent = BusinessComponent.CreateInstance<ILogin>();
            var loginResult = loginComponent.FazerLogin(credentials);

            if (!loginResult?.Autorizado ?? true)
            {
                return Unauthorized();
            }

            var (token, jti) = GenerateToken(
                loginResult.Id.ToString(),
                loginResult.Email,
                loginResult.Email
            );
            TokenStore.TokensPorUsuario[loginResult.Id.ToString()] = jti;

            return Ok(new { token });
        }

        private (string token, string jti) GenerateToken(string userId, string email, string nome)
        {
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenId = Guid.NewGuid().ToString();

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, nome),
            new Claim(JwtRegisteredClaimNames.Jti, tokenId)
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return (tokenHandler.WriteToken(token), tokenId);
        }

        public class ParamUser
        {
            public string Email { get; set; }
            public string Senha { get; set; }
        }
    }

}
