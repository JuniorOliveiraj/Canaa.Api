

using Canaa.AppHost.utils.Query;

namespace Canaa.FN.BusinessComponents.Buscar.Gastos
{
    public class TotalGastos : ITotalGastos
    {
        public double Total()
        {
            var query = new Query(@"SELECT
                SUM(valor) AS 'Total'
                FROM 
                    gastos_mensais_notion
                WHERE
                YEAR(data) = @ANO AND 
                MONTH(data) = @MES");
            var result = query.Execute().FirstOrDefault();

            if (result != null && result.ContainsKey("Total"))
            {
                return Convert.ToDouble(result["Total"]);
            }
            return 0.0;

        }
    }
}
