using Canaa.AppHost.utils.Query;
using Canaa.DataContracts.Auth;

namespace Canaa.FN.BusinessComponents.Auth
{
    public class UsuarioLogadoMetodo : IUsuarioLogadoMetodo
    {
        public UsuarioLogado Usuario()
        {
            var query = new  Query(@"SELECT  displayName, email FROM users WHERE ID = @USUARIO");

            var senha =  query.Execute().FirstOrDefault();

            return new UsuarioLogado
            {
                Email = senha["displayName"].ToString(),
                Senha = senha["email"].ToString(),
            };
        }

    }
}
