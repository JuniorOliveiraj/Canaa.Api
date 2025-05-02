using Canaa.AppHost.utils.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.FN.BusinessComponents.Response
{
    public class ControleTarefas: IControleTarefas
    {
        public ResponseDataContrac GetTarefasPendentes()
        {
            ResponseDataContrac response = new ResponseDataContrac();
            var query = new Query(@"SELECT * FROM Z_TAREFAS WHERE Status <> :TAREFA");
            query.AddParameter(new Parameter("TAREFA", "Completo"));
            var result = query.Execute();
            if (result != null && result.Count > 0) 
            {
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
    }
}
