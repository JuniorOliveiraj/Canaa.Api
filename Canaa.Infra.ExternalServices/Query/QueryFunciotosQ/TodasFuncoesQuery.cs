using Canaa.DataContracts.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.Infra.ExternalServices.Query.QueryFunciotosQ
{
    public class TodasFuncoesQuery : ITodasFuncoesQuery
    {
        private readonly IUserContext _userContext;

        public TodasFuncoesQuery(IUserContext userContext)
        {

            _userContext = userContext;
            if (_userContext == null)
            {
                throw new Exception("IUserContext não foi injetado!");
            }

        }

        public string RetunCommand(string commandText)
        {
            if (_userContext == null)
            {
                throw new Exception("IUserContext não foi injetado!");
            }
            int id = _userContext.Id;


            string processed = QueryFunctions.ProcessFunctions(commandText, id.ToString());

            return processed;
        }
    }
}
