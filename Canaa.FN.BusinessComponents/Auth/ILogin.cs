using Canaa.DataContracts.Auth;

namespace Canaa.FN.BusinessComponents.Auth
{
    public interface ILogin
    {
        UserLogin FazerLogin(UserLogin UserLogin);
    }
}