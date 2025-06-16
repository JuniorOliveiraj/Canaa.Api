using Canaa.AppHost.utils.Query;
using Canaa.DataContracts.Gastos;
using Canaa.FN.BusinessComponents.Response;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Canaa.FN.BusinessComponents.Adicionar.AdicionarGastos
{
    public class AdicionarJsonGastosMercadoPago : IAdicionarJsonGastosMercadoPago
    {
        private readonly ResponseDataContrac responseDataContrac;
        private int count = 0;
        public AdicionarJsonGastosMercadoPago()
        {
            responseDataContrac = new ResponseDataContrac();
        }

        public ResponseDataContrac AdicionarComJson(List<GastosMercadoPagoDataContract> gastos)
        {
            foreach (var gasto in gastos)
            {
                ProcessarGastos(gasto);
            }
            VerificarContator();

            return new ResponseDataContrac()
            {
                success = responseDataContrac.success,
                message = responseDataContrac.message,
                error = responseDataContrac.error,
                data = responseDataContrac.data,

            };

        }

        private void ProcessarGastos(GastosMercadoPagoDataContract gastos)
        {
            if (VerificarGastosExistente(gastos))
                responseDataContrac.data += @$"Gasto existente:{gastos.nome}  \n ";

            else
                AdicionarGastos(gastos);
        }

        private bool VerificarGastosExistente(GastosMercadoPagoDataContract gastos)
        {
            var query = new Query(@"SELECT ID AS TOTAL 
                    FROM gastos_mensais_notion
                    WHERE NAME = :NOME AND VALOR LIKE :VALOR AND DESCRICAO = :DESCRICAO AND DATA = :DATA");
            query.AddParameter(new Parameter("NOME", gastos.nome));
            query.AddParameter(new Parameter("VALOR", ConverterParaFloat(gastos.amount)));
            query.AddParameter(new Parameter("DESCRICAO", gastos.description));
            query.AddParameter(new Parameter("DATA", DateTime.Parse(gastos.date)));

            var result = query.Execute().FirstOrDefault();
            if (result != null)
                return true;

            return false;
        }

        private void AdicionarGastos(GastosMercadoPagoDataContract gastos)
        {
            try
            {
                string total = gastos.amount.Replace(',', '.');

                var query = new Query(@"INSERT INTO gastos_mensais_notion 
                    (NAME, GASTO_ESSE_MES, VALOR, DESCRICAO, DATA, AVATARIMAGE, CONTA_ORIGEM, STATUS) 
                    VALUES (:NOME, :GASTO_ESSE_MES, :VALOR, :DESCRICAO,:DATA,  :AVATARIMAGE, :CONTA_ORIGEM , :STATUS )");

                query.AddParameter(new Parameter("NOME", gastos.nome));
                query.AddParameter(new Parameter("VALOR", ConverterParaFloat(gastos.amount)));
                query.AddParameter(new Parameter("GASTO_ESSE_MES", ConverterParaFloat(gastos.amount)));
                query.AddParameter(new Parameter("DESCRICAO", gastos.description));
                query.AddParameter(new Parameter("DATA", DateTime.Parse(gastos.date)));
                query.AddParameter(new Parameter("AVATARIMAGE", gastos.imageUrl));
                query.AddParameter(new Parameter("CONTA_ORIGEM", "MERCADO PAGO"));
                query.AddParameter(new Parameter("STATUS", StatusGastos.Concluido));
                var id = query.Execute().FirstOrDefault();

                if (id["ID"] != null)
                {
                    responseDataContrac.data += @$"Gasto adicionado:{gastos.nome} e id:{id["ID"]} \n ";
                    responseDataContrac.success = true;
                    count++;
                }
            }
            catch (Exception ex)
            {
                responseDataContrac.error = ex.Message;
                responseDataContrac.success = false;
                responseDataContrac.message += "Erro ao adicionar gasto";
                responseDataContrac.data = null;

            }
        }

        private void VerificarContator()
        {
            if (count == 0)
            {
                responseDataContrac.success = true;
                responseDataContrac.message = "Nenhum gasto foi adicionado";
                responseDataContrac.data = null;
            }
            else
            {
                responseDataContrac.success = true;
                responseDataContrac.message = $"{count} gastos foram adicionados com sucesso";
            }
        }

        static float ConverterParaFloat(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                throw new ArgumentException("Entrada inválida.");

            string textoFormatado = texto.Replace(',', '.');
            if (float.TryParse(textoFormatado, NumberStyles.Any, CultureInfo.InvariantCulture, out float resultado))
            {
                return resultado;
            }
            else
            {
                throw new FormatException("Não foi possível converter a string para float.");
            }
        }
    }
}