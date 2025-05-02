using Canaa.DataContracts.Auth.Context;
using Canaa.Infra.ExternalServices.Dependency;
using Canaa.Infra.ExternalServices.Query.QueryFunciotosQ;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.FN.BusinessComponents.Utils
{
    public static class CanaaContext
    {
        public static string GetApiLocation()
        {
            IUserContext context = GetContext();

            var request = context.HttpContext.Request;
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
        public static int GetUserId() { return GetContext().Id; }
        private static IUserContext GetContext()
        {
            var contextComponet = BusinessComponentInfra.CreateInstance<ITodasFuncoesQuery>();
            IUserContext context = contextComponet.GetContext();
            if (context == null)
            {
                throw new Exception("IUserContext não foi injetado!");
            }
            return context;
        }
    }
}
