using Canaa.AppHost.utils.Query;
using Canaa.DataContracts.Gastos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Canaa.FN.BusinessComponents.Financas.Buscar.Gastos
{
    public class TotalGastos : ITotalGastos
    {
        public double Total()
        {
            var query = new Query(@"
                SELECT
                    ROUND(SUM(valor), 2) AS 'Total'
                FROM 
                    gastos_mensais_notion
                WHERE
                    YEAR(data) = @ANO 
                    AND MONTH(data) = @MES
                    AND STATUS <> :STATUS");

            query.AddParameter(new Parameter("STATUS", "Inativo"));

            var result = query.Execute().FirstOrDefault();

            return result is not null && result.TryGetValue("Total", out var totalValue)
                ? Convert.ToDouble(totalValue)
                : 0.0;
        }

        private List<Dictionary<string, object>> BuscarUltimosGastos()
        {
            var query = new Query(@"
                SELECT 
                    NAME, 
                    VALOR, 
                    DATA, 
                    avatarImage,
                    DESCRICAO
                FROM gastos_mensais_notion 
                WHERE 
                    YEAR(data) = @ANO 
                    AND MONTH(data) = @MES 
                    AND STATUS <> :STATUS
                ORDER BY DATA DESC 
                LIMIT 10");

            query.AddParameter(new Parameter("STATUS", "Inativo"));
            return query.Execute();
        }

        public TotalComUltimosGastos RetornarTotalComUltimosGastos()
        {
            var total = Total();
            var gastosDict = BuscarUltimosGastos();

            var listaGastos = new List<GastosMercadoPagoDataContract>();

            if (gastosDict != null && gastosDict.Any())
            {
                foreach (var item in gastosDict)
                {
                    var gasto = new GastosMercadoPagoDataContract
                    {
                        nome = item.TryGetValue("NAME", out var nome) ? nome?.ToString() : "",
                        amount = item.TryGetValue("VALOR", out var valor) ? valor?.ToString() : "0",
                        date = item.TryGetValue("DATA", out var data) ? data?.ToString() : "",
                        imageUrl = item.TryGetValue("avatarImage", out var img) ? img?.ToString() : "",
                        description = item.TryGetValue("DESCRICAO", out var desc) ? desc?.ToString() : "",
                     };

                    listaGastos.Add(gasto);
                }
            }

            return new TotalComUltimosGastos
            {
                total = new gastoTotalMercadoPago { totalGastosMP = total },
                ultimoGastos = listaGastos
            };
        }
    }
}
