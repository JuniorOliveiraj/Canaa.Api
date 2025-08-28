using AngleSharp.Dom;
using Canaa.AppHost.utils.Query;
using Canaa.DataContracts.Videos;
using Canaa.FN.BusinessComponents.Utils;
using Canaa.Infra.Entities.Context;
using Canaa.Infra.Entities.Entities;
using Canaa.Infra.Entities.Tabelasbef;
using Canaa.Infra.Entities.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Canaa.FN.BusinessComponents.Response
{
    public class ControleTarefas: IControleTarefas
    {



        public async Task<Z_USUARIO?> BuscarPorIdAsync()
        {
            int usuarioId =  CanaaContext.GetUserId(); 
            var usuario = await ZUsuarios.GetFirstOrDefault(new Criteria("ID", usuarioId));
            return usuario;
        }
        public ResponseDataContrac GetTarefasPendentes()
        {
            ResponseDataContrac response = new ResponseDataContrac();
            var query = new Query(@"SELECT * FROM Z_TAREFAS WHERE Status <> :TAREFA");
            query.AddParameter(new Parameter("TAREFA", "Completo"));
            var result = query.Execute();
            if (result != null && result.Count > 0) 
            {
                foreach (var item in result)
                {
                    if (item.ContainsKey("CATEGORIA") && int.TryParse(item["CATEGORIA"].ToString(), out int categoriaInt))
                    {
                        if (Enum.IsDefined(typeof(TarefasCategorias), categoriaInt))
                        {
                            var categoriaEnum = (TarefasCategorias)categoriaInt;
                            item["CATEGORIA_NOME"] = GetJsonPropertyName(categoriaEnum); // Ex: "Baixar Video"
                        }
                    }
                }

                response.success = true;
                response.data = result;
                response.message = "Tarefas pendentes encontradas.";
            }
            else
            {
                response.success = false;
                response.message = "Nenhuma tarefa pendente encontrada.";
            }
            return response;
        }
        private string GetJsonPropertyName(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<JsonPropertyNameAttribute>();
            return attr?.Name ?? value.ToString(); // Usa o nome legível, como "Baixar Video"
        }

        public List<Dictionary<string, object>> tabela() 
        {
            var query = new Query(@"SHOW COLUMNS FROM Z_USUARIOS");
            return query.Execute(); // Retorna diretamente o resultado da execução
        }
    }
    }

    
 
 
