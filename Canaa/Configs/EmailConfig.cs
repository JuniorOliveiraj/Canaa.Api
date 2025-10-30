using Canaa.Infra.ExternalServices.Email;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.Configs
{
    public static class EmailConfig
    {
        public static void Configure(IServiceCollection services, IConfiguration config)
        {
            var chaveSecreta2 = config["Resend:ApiKey"];

            if (chaveSecreta2 == null)
                throw new InvalidOperationException("A chave de API do Resend não foi encontrada nas configurações.");

            EmailService.Configure(
                apiKey: chaveSecreta2,
                defaultFromEmail: "contato@juniorbelem.com",
                defaultFromName: "junior Belem"
            );
        }
    }
}
