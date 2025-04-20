using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Canaa.AppHost.utils.Query;
using Canaa.DataContracts.Auth;
namespace Canaa.FN.BusinessComponents.Auth
{
    public class Login : ILogin
    {
        public UserLogin FazerLogin(UserLogin userLogin)
        {
            var query = new Query(@"SELECT ID FROM users WHERE EMAIL = :EMAIL AND PASSWORD = :SENHA");
            query.AddParameter(new Parameter("EMAIL", userLogin.Email));
            query.AddParameter(new Parameter("SENHA", userLogin.Senha));

            var result = query.Execute().FirstOrDefault();

            if (result != null && result["ID"] is int id)
            {
                return new UserLogin
                {
                    Id = id,
                    Email = userLogin.Email,
                    Autorizado = true
                };
            }

            return new UserLogin
            {
                Email = userLogin.Email,
                Autorizado = false
            };
        }

    }
}
