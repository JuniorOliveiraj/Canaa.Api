using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Canaa.AppHost.utils;
using Canaa.DataContracts.Auth;
using Canaa.FN.BusinessComponents.Auth;

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




    // ROTA PÚBLICA
    [AllowAnonymous]
    [HttpGet("public")]
    public IActionResult GetUser()
    {
        // var usuario = BusinessComponent.CreateInstance<IUserService>();
        // string nome = usuario.GetUsername();

        return Ok(new { user = "Rota publixa" });
    }


    // ROTA PÚBLICA
    [AllowAnonymous]
    [HttpGet("public/teste2")]
    public IActionResult GetUser2()
    {
        // var usuario = BusinessComponent.CreateInstance<IUserService>();
        // string nome = usuario.GetUsername();
        return Ok(new { user = "Rota Vam bora" });
    }

    // ROTA PRIVADA
    [Authorize]
    [HttpGet("private")]
    public IActionResult Private()
    {
        var username = User.Identity.Name;
        var jtiDoToken = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        var usuarioLogado = BusinessComponent.CreateInstance<IUsuarioLogadoMetodo>();

        UsuarioLogado usuario = usuarioLogado.Usuario();

        // Verifica se o jti do token é igual ao último salvo
        if (TokenStore.TokensPorUsuario.TryGetValue(username, out var ultimoJti))
        {
            if (jtiDoToken != ultimoJti)
            {
                return Unauthorized("❌ Token inválido ou substituído por um mais recente.");
            }
        }

        return Ok(new
        {
             
            Nome = usuario.Email,
            Senha = usuario.Senha,
            Mensagem = "🔒 Acesso autorizado à rota privada!"

        });
        //return Ok("");
    }


    
}



