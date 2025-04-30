using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.Infra.ExternalServices.Videos
{
    public class FFMpegKiller
    {
        public static void MatarFFMPEG()
        {
            try
            {
                Process[] processos = Process.GetProcessesByName("ffmpeg");

                foreach (Process proc in processos)
                {
                    try
                    {
                        proc.Kill(); 
                        proc.WaitForExit();  
                        Console.WriteLine($"Processo ffmpeg (PID: {proc.Id}) finalizado com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao tentar matar o processo ffmpeg (PID: {proc.Id}): {ex.Message}");
                    }
                }

                if (processos.Length == 0)
                {
                    Console.WriteLine("Nenhum processo ffmpeg em execução.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro geral: {ex.Message}");
            }
        }
    }
}
