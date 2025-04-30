using Canaa.DataContracts.Videos;
using Canaa.FN.BusinessComponents.Response;
using Canaa.FN.BusinessComponents.Utils;
using Canaa.Infra.ExternalServices.Utils;
using Canaa.Infra.ExternalServices.Youtube;
using Mysqlx.Session;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode.Videos;

namespace Canaa.FN.BusinessComponents.Video.CriarVideos
{
    public class VideosVertical : IVideosVertical
    {
        private string _guid;

        public async Task<ResponseDataContrac> EmpilharVideosCompletoAsync(string linkVideoTop, string linkVideoButton)
        {
            _guid = Guid.NewGuid().ToString();
            ProgressService.InsertNewTask(_guid, "VIDEO");
            ProgressService.UpdateTaskStatus(_guid, "Iniciando");
            string tempDirectory = ApiLocation.DiretorioTemporario("Youtube");
            GerarTentarCriarVideo result = await BaixarVideos(linkVideoTop, linkVideoButton, tempDirectory);

            if (result.sucess == false)
            {

                return new ResponseDataContrac
                {
                    success = false,
                    message = "Erro ao baixar os vídeos.",
                    data = null,
                    error = result.error,
                    status = "ERROR"
                };
            }
            string VideoPath = await VideoStackerTemplate.StackVerticallyAsync(
                result.VideoTopPath,
                result.VideoButtonPath,
                tempDirectory,
                1080,
                _guid
             );

            string finalComLegenda = Path.Combine(tempDirectory, NomeArquivoFinal(result.VideoTopPath));

            ProgressService.UpdateTaskStatus(_guid, "Empilhando Videos");

            var videoFinal = VideoStackerTemplate.AdicionarLegendasAoVideo(
                VideoPath,
                result.legendas,
               finalComLegenda,
                    progressLine => {
                        // aqui você pode atualizar um progress bar, log no UI etc.

                        Console.WriteLine("PROGRESS: " + progressLine);
                    }
                );
            ProgressService.SetConcluido(_guid);
            // Chama o método de empilhamento vertical
            return new ResponseDataContrac
            {
                success = true,
                message = "Vídeos empilhados com sucesso.",
                data = videoFinal,
                error = null,
                status = "OK"
            };
        }


        public async Task<ResponseDataContrac> EmpilharVideosAsync(string pathVideoPrincipal, string pathVideoSecundario)
        {
            string VideoPath = await VideoStackerTemplate.StackVerticallyAsync(
                pathVideoPrincipal,
                pathVideoSecundario,
                CaminhoArquivoFinal()
             );
            return new ResponseDataContrac
            {
                success = true,
                message = "Vídeos empilhados com sucesso.",
                data = VideoPath,
                error = null,
                status = "OK"
            };
        }


        public async Task<ResponseDataContrac> EmpilharVideoLegendadoAsync(string pathVideoPrincipal, string pathLegendas)
        {
            string finalComLegenda = Path.Combine(CaminhoArquivoFinal(), NomeArquivoFinal(pathVideoPrincipal));


            var videoFinal = VideoStackerTemplate.AdicionarLegendasAoVideo(
               pathVideoPrincipal,
               pathLegendas,
               finalComLegenda,
                    progressLine => {
                        // aqui você pode atualizar um progress bar, log no UI etc.
                        Console.WriteLine("PROGRESS: " + progressLine);
                    }
                );


            return new ResponseDataContrac
            {
                success = true,
                message = "Vídeos empilhados com sucesso.",
                data = videoFinal,
                error = null,
                status = "OK"
            };
        }


        private async Task<GerarTentarCriarVideo> BaixarVideos(string linkVideoTop, string linkVideoButton, string diretorio)
        {
            var videoTopPath = await YoutubeDownloader.DownloadVideoAsync(linkVideoTop, diretorio, _guid);
            var videoButtonPath = await YoutubeDownloader.DownloadVideoAsync(linkVideoButton, diretorio, _guid); 
            var LegendaPath = await YoutubeDownloader.DownloadCaptionsAsync(linkVideoTop, diretorio);

            if (videoTopPath == null || videoButtonPath == null || LegendaPath == null)
            {
                return new GerarTentarCriarVideo
                {
                    sucess = false,
                    error = videoTopPath + " " + videoButtonPath + " " + LegendaPath,
                };
            }
            return new GerarTentarCriarVideo
            {
                sucess = true,
                VideoTopPath = videoTopPath,
                VideoButtonPath = videoButtonPath,
                legendas = LegendaPath,
                error = null
            };
        }

        private string NomeArquivoFinal(string path)
        {
            string fileName = Path.GetFileName(path);
            // Separar nome e extensão
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            // Adicionar o "_COM-LEGENDAS"
            string newFileName = $"{nameWithoutExtension}_COM-LEGENDAS{extension}";
            return newFileName;
        }

        private string CaminhoArquivoFinal()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), "Youtube");

            if (!Directory.Exists(tempDirectory))
                Directory.CreateDirectory(tempDirectory);

            return tempDirectory;
        }

        private class GerarTentarCriarVideo
        {
            public bool sucess { get; set; }
            public string VideoTopPath { get; set; }
            public string VideoButtonPath { get; set; }

            public string legendas { get; set; }
            public string error { get; set; }
        }
    }
}
