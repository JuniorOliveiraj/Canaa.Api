using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Canaa.AppHost.utils;
using Canaa.DataContracts.Auth;
using Canaa.FN.BusinessComponents.Auth;
using Canaa.Auth;

public static class TokenStore
{
    public static Dictionary<string, string> TokensPorUsuario = new();
}

[ApiController]
[Route("v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult LoginJwt(ParamUser param)
    {
        var credentials = new UserLogin
        {
            Email = param.Email,
            Senha = param.Senha
        };

        var loginComponent = BusinessComponent.CreateInstance<IAuthContaUsuario>();
        var loginResult = loginComponent.Login(credentials);

        if (!loginResult?.Autorizado ?? true)
        {
            return Unauthorized();
        }

        var tokenGenerator = new GerarToken(_config);
        var (token, jti) = tokenGenerator.GetToken(
            loginResult.Id.ToString(),
            loginResult.Email,
            loginResult.Senha
        );

        if (!TokenStore.TokensPorUsuario.ContainsKey(loginResult.Id.ToString()))
        {
            TokenStore.TokensPorUsuario[loginResult.Id.ToString()] = new List<string>();
        }

        TokenStore.TokensPorUsuario[loginResult.Id.ToString()].Add(jti);


        return Ok(new { token });
    }




    [AllowAnonymous]
    [HttpPost("CriarConta")]
    public IActionResult CriarConta([FromBody] ParamUser param)
    {
        var credentials = new UserLogin
        {
            NomeUsuario = param.NomeUsuario,
            Email = param.Email,
            Senha = param.Senha,
            Nome = param.Nome
        };
        var createnComponent = BusinessComponent.CreateInstance<IAuthContaUsuario>();
        var createResult = createnComponent.CriarConta(credentials);
        if (createResult?.Mensage != null &&!createResult.Autorizado)
        {
            return BadRequest(new { Mensage = createResult.Mensage });
        }
        var tokenGenerator = new GerarToken(_config);
        var (token, jti) = tokenGenerator.GetToken(
            createResult.Id.ToString(),
            createResult.Email,
            createResult.Senha
        );
        if (!TokenStore.TokensPorUsuario.ContainsKey(createResult.Id.ToString()))
        {
            TokenStore.TokensPorUsuario[createResult.Id.ToString()] = new List<string>();
        }

        TokenStore.TokensPorUsuario[createResult.Id.ToString()].Add(jti);


        return Ok(new { 
            Ok = "Ok",
            sucess = true,
            id = createResult.Id,
            token = token,
        });
    }
 

    [AllowAnonymous]
    [HttpGet("Liberado")]
    public IActionResult GerarTokenPadrao()
    {
        var credentials = new UserLogin
        {
            Email = "teste@tes.com",
            Senha = "123"
        };

        var loginComponent = BusinessComponent.CreateInstance<IAuthContaUsuario>();
        var loginResult = loginComponent.Login(credentials);

        if (!loginResult?.Autorizado ?? true)
        {
            return Unauthorized();
        }

        var tokenGenerator = new GerarToken(_config);
        var (token, jti) = tokenGenerator.GetToken(
            loginResult.Id.ToString(),
            loginResult.Email,
            loginResult.Email
        );
 

        if (!TokenStore.TokensPorUsuario.ContainsKey(loginResult.Id.ToString()))
        {
            TokenStore.TokensPorUsuario[loginResult.Id.ToString()] = new List<string>();
        }

        TokenStore.TokensPorUsuario[loginResult.Id.ToString()].Add(jti);

        return Ok(new { token });
    }

    public static class TokenStore
    {
        public static Dictionary<string, List<string>> TokensPorUsuario = new();
    }

    public class ParamUser
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public string? NomeUsuario { get; set; }
        public string? Nome  { get; set; }
    }

}



