using Canaa.AppHost.utils.Query;
using Canaa.DataContracts.Auth;

namespace Canaa.FN.BusinessComponents.Auth
{
    public class UsuarioLogadoMetodo : IUsuarioLogadoMetodo
    {
        public UsuarioLogado Usuario()
        {
            var query = new  Query(@"SELECT  NOME, EMAIL FROM Z_USUARIOS WHERE ID = @USUARIO");

            var senha =  query.Execute().FirstOrDefault();

            return new UsuarioLogado
            {
                Email = senha["NOME"].ToString(),
                Senha = senha["EMAIL"].ToString(),
            };
        }

    }
}
