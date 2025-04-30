using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Canaa.Infra.ExternalServices.Videos.Legenda
{
    public static class LegendaConverter
    {
        public static string ConverterTxtParaSrt(string caminhoTxt)
        {
            if (!File.Exists(caminhoTxt))
                throw new FileNotFoundException("Arquivo de entrada não encontrado.", caminhoTxt);

            string diretorio = Path.GetDirectoryName(caminhoTxt);
            string nomeSemExtensao = Path.GetFileNameWithoutExtension(caminhoTxt);
            string caminhoSrt = Path.Combine(diretorio, nomeSemExtensao + ".srt");

            var linhas = File.ReadAllLines(caminhoTxt);
            var legendas = new List<(string Inicio, string Fim, string Texto)>();

            for (int i = 0; i < linhas.Length; i++)
            {
                var matchInicio = Regex.Match(linhas[i], @"^([\d:.]+):\s*(.*)");
                if (!matchInicio.Success)
                    continue;

                string tempoInicio = FormatarTempo(matchInicio.Groups[1].Value);
                string texto = matchInicio.Groups[2].Value;

                if (i + 1 >= linhas.Length)
                    break;

                var matchFim = Regex.Match(linhas[i + 1], @"^([\d:.]+):");
                if (!matchFim.Success)
                    continue;

                string tempoFim = FormatarTempo(matchFim.Groups[1].Value);
                legendas.Add((tempoInicio, tempoFim, texto));

                i++;  // pula a linha de tempo de “fim”
            }

            using (var writer = new StreamWriter(caminhoSrt, false, Encoding.UTF8))
            {
                for (int idx = 0; idx < legendas.Count; idx++)
                {
                    var (inicio, fim, texto) = legendas[idx];
                    writer.WriteLine($"{idx + 1}");
                    writer.WriteLine($"{inicio} --> {fim}");
                    writer.WriteLine(texto);
                    writer.WriteLine();  
                }
            }

            return caminhoSrt;
        }

        private static string FormatarTempo(string entrada)
        {
            var partes = entrada.Split('.');
            string principal = partes[0].Replace('.', ':');
            string milissegundos = (partes.Length > 1)
                ? partes[1].PadRight(3, '0').Substring(0, 3)
                : "000";

            return $"{principal},{milissegundos}";
        }

    }
}
