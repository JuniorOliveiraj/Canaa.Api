using System;
using System.Collections.Generic;
using Canaa.AppHost.utils.Query;

using System.Collections.Concurrent;
using Canaa.DataContracts.Videos;
using System.Web;
using System.Diagnostics;

namespace Canaa.Infra.ExternalServices.Utils
{

    public static class ProgressService
    {
        private static readonly ConcurrentDictionary<string, double> _progressMap = new();
        private static string _status;


        // Insere uma nova tarefa no banco
        public static void InsertNewTask(string processGUID, string url)
        {
            _status = "pending";
            if (!VerificaTask(processGUID))
            {
                var query = new Canaa.AppHost.utils.Query.Query(@"
                INSERT INTO Z_TAREFAS
                  (GUID, URL, STATUS, PROGRESSO )
                VALUES
                  (:GUID, :URL, :STATUS, :PROGRESSO )");

                query.AddParameter(new Parameter("GUID", processGUID));
                query.AddParameter(new Parameter("URL", url));
                query.AddParameter(new Parameter("STATUS", "pending"));
                query.AddParameter(new Parameter("PROGRESSO", 0));
                query.Execute();
            }
        }

        // Atualiza status da tarefa
        public static void UpdateTaskStatus(string processGUID, string status)
        {
            var query = new Canaa.AppHost.utils.Query.Query(@"
                UPDATE Z_TAREFAS
                SET STATUS = :STATUS
                WHERE GUID = :GUID");

            query.AddParameter(new Parameter("STATUS", status));
            query.AddParameter(new Parameter("GUID", processGUID));
            query.Execute();
            _status = status;
        }

        // Atualiza progresso da tarefa no banco e em memória
        public static void UpdateTaskProgress(string processGUID, double progress)
        {
            _progressMap[processGUID] = progress;
        }

        // Armazena o caminho do arquivo quando completo
        public static void UpdateTaskFilePath(string processGUID, string filePath)
        {
            var query = new Canaa.AppHost.utils.Query.Query(@"
                UPDATE Z_TAREFAS
                SET FILEPATH = :FILEPATH 
                WHERE GUID = :GUID");

            query.AddParameter(new Parameter("FILEPATH", filePath));
            query.AddParameter(new Parameter("GUID", processGUID));
            query.Execute();
        }

        public static void UpdateTaskFileUrl(string processGUID, string url)
        {
            var query = new Canaa.AppHost.utils.Query.Query(@"
                UPDATE Z_TAREFAS
                SET URL = :URL 
                WHERE GUID = :GUID");

            query.AddParameter(new Parameter("URL", url));
            query.AddParameter(new Parameter("GUID", processGUID));
            query.Execute();
        }

        // Registra mensagem de erro
        public static void UpdateTaskError(string processGUID, string errorMessage)
        {
            var query = new Canaa.AppHost.utils.Query.Query(@"
                UPDATE Z_TAREFAS
                SET ERRO = :ERRO
                WHERE GUID = :GUID");

            query.AddParameter(new Parameter("ERRO", errorMessage));
            query.AddParameter(new Parameter("GUID", processGUID));
            query.Execute();
        }

        private static bool VerificaTask(string processGUID)
        {
            var query = new Canaa.AppHost.utils.Query.Query(@"
                SELECT COUNT(*) AS COUNT
                FROM Z_TAREFAS
                WHERE GUID = :GUID");
            query.AddParameter(new Parameter("GUID", processGUID));
            var result = query.Execute().FirstOrDefault();
            return result != null && result["COUNT"] is int count && count > 0;
        }


        // Define progresso em memória e no banco
        public static void SetProgress(string processGUID, double percent)
        {
            UpdateTaskProgress(processGUID, percent);
        }       
        public static void SetConcluido(string processGUID)
        {
            UpdateTaskStatus(processGUID, "Completo");
            SetProgress(processGUID, 100);
        }
        public static void SetTaskCategoria(string processGUID, TarefasCategorias categorias )
        {
            var query = new Canaa.AppHost.utils.Query.Query(@"
                UPDATE Z_TAREFAS
                SET CATEGORIA = :CATEGORIA
                WHERE GUID = :GUID");

            query.AddParameter(new Parameter("CATEGORIA", categorias));
            query.AddParameter(new Parameter("GUID", processGUID));
            query.Execute();
        }

        // Retorna o progresso e status atual
        public static ProgressResponse GetProgress(string processGUID)
        {
            var progress = _progressMap.TryGetValue(processGUID, out var value) ? value : 0;

            return new ProgressResponse
            {
                ProcessId = processGUID,
                Progress = progress,
                Status = progress >= 100 ? "Completed" : _status

            };
        }
    }
}
