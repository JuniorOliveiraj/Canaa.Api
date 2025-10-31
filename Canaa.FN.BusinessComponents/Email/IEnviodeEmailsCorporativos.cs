using Canaa.FN.BusinessComponents.Response;
namespace Canaa.FN.BusinessComponents.Email
{
    public interface IEnviodeEmailsCorporativos
    {
        Task<ResponseDataContrac> EnvioDeEmailEmMassaCorporativos(string guid, long idUsuario);
    }
}