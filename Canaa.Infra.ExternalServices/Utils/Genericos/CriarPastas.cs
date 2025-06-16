using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.Infra.ExternalServices.Utils.Genericos
{
    public static class CriarPastas
    {
        public static string CriarPasta(string nomeDaPasta)
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            // Cria o caminho completo
            string caminhoCompleto = Path.Combine(userFolder, nomeDaPasta);

            // Cria a pasta (se não existir)
            Directory.CreateDirectory(caminhoCompleto);
            return caminhoCompleto;
        }
        public static string CriarPastaTemp(string nomeDaPasta)
        {
            string tempFolder = Path.GetTempPath();
             
            string temPasta = Path.Combine(tempFolder, nomeDaPasta);
            Directory.CreateDirectory(temPasta);

            return temPasta;
        }
    }
}
