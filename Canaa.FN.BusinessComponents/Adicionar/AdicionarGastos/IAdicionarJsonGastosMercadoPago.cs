using Canaa.DataContracts.Gastos;
using Canaa.FN.BusinessComponents.Response;

namespace Canaa.FN.BusinessComponents.Adicionar.AdicionarGastos
{
    public interface IAdicionarJsonGastosMercadoPago
    {
        ResponseDataContrac AdicionarComJson(List<GastosMercadoPagoDataContract> gastos);
    }
}