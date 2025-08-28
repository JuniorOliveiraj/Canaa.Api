using Canaa.Infra.Entities.Entities;

namespace Canaa.FN.BusinessComponents.Response
{
    public interface IControleTarefas
    {
        ResponseDataContrac GetTarefasPendentes();
        List<Dictionary<string, object>> tabela();

        Task<Z_USUARIO> BuscarPorIdAsync();
    }
}