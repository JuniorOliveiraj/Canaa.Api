using Canaa.DataContracts.Gastos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.FN.BusinessComponents.Response
{
    public class ResponseDataContrac
    {
        public bool success { get; set; }
        public string? message { get; set; }
        public Object? data { get; set; }
        public string? error { get; set; }
        public string? status { get; set; }

    }
}
