using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.FN.BusinessComponents.Utils
{
    public static class ApiLocation
    {
        public static string GetApiLocation(HttpContext httpContext)
        {
            var request = httpContext.Request;
            string urlAtual = $"{request.Scheme}://{request.Host}{request.PathBase}";
            return urlAtual;
        }
        public static string DiretorioTemporario(string nome = "video")
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), nome);

            if (!Directory.Exists(tempDirectory))
                Directory.CreateDirectory(tempDirectory);

            return tempDirectory;
        }
    }
}
