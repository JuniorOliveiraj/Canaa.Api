using Canaa.Infra.ExternalServices.Query.QueryFunciotosQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Canaa.Infra.ExternalServices.Query
{
    public static class QueryFunctions
    {
        public static string ProcessFunctions(string commandText, string idUsuarioLogado)
        {
            return Regex.Replace(commandText, @"@(\w+)", match =>
            {
                var functionName = match.Groups[1].Value.ToUpper();

                return functionName switch
                {
                    "ANO" => DateTime.Now.Year.ToString(),
                    "MES" => DateTime.Now.Month.ToString(),
                    "DIA" => DateTime.Now.Day.ToString(),
                    "HORA" => DateTime.Now.Hour.ToString(),
                    "AGORA" => DateTime.Now.ToString(),
                    "USUARIO" => idUsuarioLogado,
                    // Pode adicionar mais funções aqui  
                    _ => throw new InvalidOperationException($"Função especial '@{functionName}' não reconhecida.")
                };
            });
        }
    }
}
