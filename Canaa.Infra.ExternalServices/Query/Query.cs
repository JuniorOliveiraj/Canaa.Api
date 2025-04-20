using Canaa.Infra.ExternalServices.Dependency;
using Canaa.Infra.ExternalServices.Query;
using Canaa.Infra.ExternalServices.Query.QueryFunciotosQ;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace Canaa.AppHost.utils.Query
{
    public class Query
    {
        private readonly string _commandText;
        private readonly List<Parameter> _parameters = new();
       
       


        public Query(string commandText, params Parameter[] parameters)
        {
            _commandText = commandText;
            _parameters.AddRange(parameters);
        }
        public void AddParameter(Parameter parameter)
        {
            _parameters.Add(parameter);
        }
        public List<Dictionary<string, object>> Execute()
        {
            var results = new List<Dictionary<string, object>>();
            var connectionString = Config.GetConnectionString();

            try
            {
                string processedCommandText = ProcessCommandText(_commandText, out var processedParams);

                using var connection = new MySqlConnection(connectionString);
                connection.Open();

                using var command = new MySqlCommand(processedCommandText, connection);
                AddParametersToCommand(command, processedParams);

                using var reader = command.ExecuteReader();
                results = ReadResults(reader);
            }
            catch (MySqlException ex)
            {
                Console.Error.WriteLine($"Erro MySQL: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {

                Console.Error.WriteLine($"Erro inesperado: {ex.Message}");
                throw;
            }

            return results;
        }

        private string ProcessCommandText(string commandText, out List<KeyValuePair<string, object>> processedParams)
        {
            // Primeiro, processa as funções tipo @NOW(), @RANDOM(), etc
            var gstosComponent = BusinessComponentInfra.CreateInstance<ITodasFuncoesQuery>();

            string processed = gstosComponent.RetunCommand(commandText);
            

            //  string processed = QueryFunctions.ProcessFunctions(commandText);

            processedParams = new();

            foreach (var param in _parameters)
            {
                if (param.Value is IEnumerable<object> list && !(param.Value is string))
                {
                    var placeholders = new List<string>();
                    int index = 0;
                    foreach (var item in list)
                    {
                        var mysqlParamName = $"@{param.Name}{index}";
                        placeholders.Add(mysqlParamName);
                        processedParams.Add(new KeyValuePair<string, object>(mysqlParamName, item));
                    }

                    // Substitui :NOME por múltiplos @NOME0,@NOME1,...
                    processed = Regex.Replace(
                        processed,
                        $@":{param.Name}\b",
                        string.Join(",", placeholders)
                    );
                }
                else
                {
                    var mysqlParamName = $"@{param.Name}";

                    // Substitui :NOME por @NOME
                    processed = Regex.Replace(
                        processed,
                        $@":{param.Name}\b",
                        mysqlParamName
                    );

                    processedParams.Add(new KeyValuePair<string, object>(mysqlParamName, param.Value));
                }
            }

            return processed;
        }


        private void AddParametersToCommand(MySqlCommand command, List<KeyValuePair<string, object>> parameters)
        {
            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value);
            }
        }

        private List<Dictionary<string, object>> ReadResults(MySqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();

            while (reader.Read())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.GetValue(i);
                }
                results.Add(row);
            }

            return results;
        }
    }
}
