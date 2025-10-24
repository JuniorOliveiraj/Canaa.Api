using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.DataContracts.Gastos
{
    public class GastosMercadoPagoDataContract
    {
        public string nome { get; set; }
        public string amount { get; set; }
        public string description { get; set; }
        public string origin { get; set; }
        public string date { get; set; }
        public string imageUrl { get; set; }
        public StatusGastos? status { get; set; }

    }
    public class TotalComUltimosGastos
    {
        public gastoTotalMercadoPago total{ get; set; }
        public List<GastosMercadoPagoDataContract>? ultimoGastos { get; set; }
    }

    public class gastoTotalMercadoPago
    {
        public double totalGastosMP { get; set; }
    }
}
