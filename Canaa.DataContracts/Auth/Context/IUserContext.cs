using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.DataContracts.Auth.Context
{
    public interface IUserContext
    {
        int Id { get; }
        string Email { get; }
        string Nome { get; }
    }
}
