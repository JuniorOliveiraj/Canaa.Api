using Canaa.DataContracts.Gastos;

namespace Canaa.FN.BusinessComponents.Financas.Buscar.Gastos
{
    public interface ITotalGastos
    {
        double Total();
        TotalComUltimosGastos RetornarTotalComUltimosGastos();
    }
}
