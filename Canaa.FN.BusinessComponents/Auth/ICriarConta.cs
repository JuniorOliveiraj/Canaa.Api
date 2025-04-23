using Canaa.DataContracts.Auth;

namespace Canaa.FN.BusinessComponents.Auth
{
    public interface ICriarConta
    {
        UserLogin Create(UserLogin UserLogin);
    }
}