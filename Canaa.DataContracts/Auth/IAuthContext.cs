using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.Infra.ExternalServices.Context
{
    interface IAuthContext
    {
        int UserId { get; }
        string Nome { get; }
        string Email { get; }
    }
}
