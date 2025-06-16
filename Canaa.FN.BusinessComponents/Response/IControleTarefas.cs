namespace Canaa.FN.BusinessComponents.Response
{
    public interface IControleTarefas
    {
        ResponseDataContrac GetTarefasPendentes();
        List<Dictionary<string, object>> tabela();
    }
}