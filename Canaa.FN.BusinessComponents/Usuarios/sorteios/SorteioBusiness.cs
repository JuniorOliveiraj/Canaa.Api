using Canaa.DataContracts.Service;
using Canaa.DataContracts.Whatsapp;
using Canaa.FN.BusinessComponents.Response;
using Canaa.Infra.Entities.Tabelasbef;
using Canaa.Infra.Entities.Utils;
using Canaa.Infra.ExternalServices.Whatsapp;
using Canaa.TempModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace Canaa.FN.BusinessComponents.Usuarios.sorteios
{
    public class SorteioBusiness : ISorteioBusiness
    {
        public List<Participants> BuscarParticipantes()
        {
            var participantes = ParticipantesSorteios.GetAll();

            return participantes;
        }

        public ResponseDataContrac DeletarParticipante(int idParticipante)
        {
            try
            {
                ParticipantesSorteios.DeleteMany(new Criteria("Id", idParticipante));
            }
            catch (Exception ex)
            {
                return new ResponseDataContrac
                {
                    success = false,
                    message = "Erro ao deletar participante: " + ex.Message
                };
            }
            return new ResponseDataContrac
            {
                success = true,
                message = "Participante deletado com sucesso!"
            };
        }
        public ResponseDataContrac AdicionarParticipants(ParticipantesRequest participante)
        {
            bool sucession = false;


            try
            {
                var participanteNew = new Participants
                {
                    Name = participante.Nome,
                    Phone = participante.Telefone
                };
                ParticipantesSorteios.Create(participanteNew);
            }catch (Exception ex) {
                return new ResponseDataContrac
                {
                    success = false,
                    message = "Erro ao adicionar participante: " + ex.Message
                };
            }

            return new ResponseDataContrac
            {
                success = true,
                message = "Participante adicionado com sucesso!"
            };
        }


        public async Task<ResponseDataContrac> GerarNovoSorteio()
        {
            try
            {
                // 1 - Limpar tabela
                SorteiosEntity.DeleteAll();

                // 2 - Buscar participantes
                var participantes = ParticipantesSorteios.GetAll();

                if (participantes == null || participantes.Count == 0)
                {
                    return new ResponseDataContrac
                    {
                        success = false,
                        message = "Nenhum participante disponível para o sorteio."
                    };
                }

                if (participantes.Count == 1)
                {
                    return new ResponseDataContrac
                    {
                        success = false,
                        message = "Não é possível realizar sorteio com apenas 1 participante."
                    };
                }

                // 3 - Gerar derangement (garantia total de que ninguém tira a si mesmo)
                var participantesSorteados = Derangement(participantes);

                var combinacoes = new List<(Participants sorteador, Participants sorteado)>();

                for (int i = 0; i < participantes.Count; i++)
                {
                    combinacoes.Add((participantes[i], participantesSorteados[i]));
                }

                // 4 - Salvar resultados no banco
                foreach (var combo in combinacoes)
                {
                    var item = new Sorteios
                    {
                        IdSorteador = combo.sorteador.Id,
                        NomeSorteado = combo.sorteado.Name
                    };

                    SorteiosEntity.Create(item);
                }

                // 5 - Gerar links de retorno
                var links = combinacoes.Select(c => new
                {
                    nome = c.sorteado.Name,
                    c.sorteado.Phone,
                    link = $"https://app.juniorbelem.com/verificar/{c.sorteado.Id}"
                }).ToList();

                return new ResponseDataContrac
                {
                    success = true,
                    message = "Sorteio realizado com sucesso!",
                    data = links
                };
            }
            catch (Exception ex)
            {
                return new ResponseDataContrac
                {
                    success = false,
                    message = "Erro ao realizar sorteio: " + ex.Message
                };
            }
        }
        private List<T> Derangement<T>(List<T> original)
        {
            Random rng = new Random();
            List<T> result;

            do
            {
                result = original.OrderBy(x => rng.Next()).ToList();

            } while (original.Zip(result, (o, r) => Equals(o, r)).Any(x => x));

            return result;
        }
        public void AterarStatusSorteios()
        {
            var sorteios = SorteiosEntity.GetAll();
            foreach (var item in sorteios)
            {
                item.Viewed = null;
                SorteiosEntity.Save(item);
            }
        }

        private async Task  EnviarLinkDoSorteioAsync(string phone, string link)
        {
            if (string.IsNullOrEmpty(phone)) return;

            var sorteio = SorteiosEntity.GetAll();
            foreach (var item in sorteio)
            {
                var mensagem = new SendTextMessageDataObjec
                {
                    
                    instancia = "BOTJR",
                    number = phone,
                    textMessage = new TextMessage
                    {
                        text = $"Para visualizar sua mensagem, basta acessar o link abaixo:{link}"
                    }
                };

                var resultado = await WhatsAppSender.Mensage(mensagem);
            }
        }

    }
}
