using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace Canaa.DataContracts.Auth.Context
{
    public interface IUserContext
    {
        int Id { get; }
        string Email { get; }
        string Nome { get; }
        HttpContext HttpContext { get; }
    }
}
