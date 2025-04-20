using Canaa.DataContracts.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.Infra.ExternalServices.Query.QueryFunciotosQ
{
    public class UsuarioQuery
    {
        private readonly IUserContext _userContext;

        public UsuarioQuery(IUserContext userContext)
        {
            _userContext = userContext;
        }
        public int UsuarioQueryLogado() {

            if (_userContext.Id == 0) 
                return 0;

            return _userContext.Id;
        }
    }
}
