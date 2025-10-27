using Canaa.Infra.Entities.Entities;
using Canaa.Infra.Entities.Entities;

namespace Canaa.FN.BusinessComponents.Usuarios
{
    public interface IUsuariosBusiness
    {
        Task<Z_USUARIO> BuscarPorIdAsync(int id);
        Task<string> InserirContatosEmailDoJson(string json);
        Task<List<ContatosEmail>> BuscarTodosContatosEmail();
    }
}