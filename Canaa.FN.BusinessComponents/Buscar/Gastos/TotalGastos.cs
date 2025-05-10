

using Canaa.AppHost.utils.Query;
using Canaa.DataContracts.Gastos;

namespace Canaa.FN.BusinessComponents.Buscar.Gastos
{
    public class TotalGastos : ITotalGastos
    {
        public double Total()
        {
            var query = new Query(@"SELECT
                ROUND(SUM(valor), 2) AS 'Total'
                FROM 
                    gastos_mensais_notion
                WHERE
                YEAR(data) = @ANO AND 
                MONTH(data) = @MES
                AND STATUS <> :STATUS");
            query.AddParameter(new Parameter("STATUS", StatusGastos.Inativo));

            var result = query.Execute().FirstOrDefault();

            if (result != null && result.ContainsKey("Total"))
            {
                return Convert.ToDouble(result["Total"]);
            }
            return 0.0;

        }
    }
}
