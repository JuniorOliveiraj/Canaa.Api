using Canaa.DataContracts.Auth.Context;
 
namespace Canaa.Infra.ExternalServices.Query.QueryFunciotosQ
{
    public interface ITodasFuncoesQuery
    {
        string RetunCommand(string commandText);
        IUserContext GetContext();
    }
}