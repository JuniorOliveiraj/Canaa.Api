using Canaa.DataContracts.Auth;

namespace Canaa.FN.BusinessComponents.Auth
{
    public interface IAuthContaUsuario
    {
        public UserLogin CriarConta(UserLogin userLogin);
        UserLogin Login(UserLogin userLogin);
    }
}