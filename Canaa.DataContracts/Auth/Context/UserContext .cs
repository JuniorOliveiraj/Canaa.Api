using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace Canaa.DataContracts.Auth.Context
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public int Id
        {
            get
            {
                var idClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return int.TryParse(idClaim, out var id) ? id : 0;
            }
        }

        public string Email
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
            }
        }

        public string Nome
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? string.Empty;
            }
        }

        public HttpContext HttpContext => _httpContextAccessor.HttpContext;
    }
}
