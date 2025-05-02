using AngleSharp.Io;
using Canaa.DataContracts.Auth.Context;
using Canaa.DataContracts.Videos;
using Canaa.FN.BusinessComponents.Response;
using Canaa.FN.BusinessComponents.Utils;
using Canaa.Infra.ExternalServices.Utils;
using Canaa.Infra.ExternalServices.Youtube;
using Mysqlx.Session;
using MySqlX.XDevAPI.Common;
using Ninject.Activation;
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
        private readonly IUserContext _userContext;

        public VideosVertical(IUserContext userContext)
        {
            _userContext = userContext;
        }

        private string _guid;

        public async Task<ResponseDataContrac> EmpilharVideosLegendar(string linkVideoTop, string linkVideoButton)
        {
            _guid = Guid.NewGuid().ToString();
            ProgressService.InsertNewTask(_guid, "VIDEO");
            ProgressService.UpdateTaskStatus(_guid, "Iniciando");
            string tempDirectory = CanaaContext.DiretorioTemporario("Youtube");
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
            EmpilharVideosDataObject empilharVideosData = new EmpilharVideosDataObject();
            empilharVideosData.VideoTop = result.VideoTopPath;
            empilharVideosData.VideoBottom = result.VideoButtonPath;
            empilharVideosData.OutputDirectory = tempDirectory;
            empilharVideosData.ProcessId = _guid;
            string VideoPath = await VideoStackerTemplate.StackVerticallyAsync(empilharVideosData);


            string finalComLegenda = Path.Combine(tempDirectory, NomeArquivoFinal(result.VideoTopPath));

            ProgressService.UpdateTaskStatus(_guid, "Empilhando Videos");

            LegendarVideosDataObject legendarVideosDataObject = new LegendarVideosDataObject();

            legendarVideosDataObject.Video = VideoPath;
            legendarVideosDataObject.Legendas = result.legendas;
            legendarVideosDataObject.FinalComLegenda = finalComLegenda;
            legendarVideosDataObject.OnProgress = progressLine =>
            {
                // ProgressService.SetProgress(_guid,progressLine);
                Console.WriteLine("PROGRESS: " + progressLine);
            };
            var videoFinal = VideoStackerTemplate.AdicionarLegendasAoVideo(legendarVideosDataObject);       

            ProgressService.UpdateTaskFilePath(_guid, videoFinal);

            ProgressService.SetTaskCategoria(_guid, TarefasCategorias.CriarVideoCompleto);
            ProgressService.SetConcluido(_guid);

            return new ResponseDataContrac
            {
                success = true,
                message = "Vídeos empilhados com sucesso.",
                data = new string[] { videoFinal },
                error = null,
                status = "OK"
            };
        }


        public async Task<ResponseDataContrac> EmpilharVideo(string pathVideoPrincipal, string pathVideoSecundario)
        {
            EmpilharVideosDataObject empilharVideosData = new EmpilharVideosDataObject
            {
                VideoTop = pathVideoPrincipal,
                VideoBottom = pathVideoSecundario,
                OutputDirectory = CaminhoArquivoFinal()
            };
            string VideoPath = await VideoStackerTemplate.StackVerticallyAsync(empilharVideosData);
            return new ResponseDataContrac
            {
                success = true,
                message = "Vídeos empilhados com sucesso.",
                data = VideoPath,
                error = null,
                status = "OK"
            };
        }


        public async Task<ResponseDataContrac> AdicionarLegendasAoVideo(string pathVideoPrincipal, string pathLegendas)
        {
            string finalComLegenda = Path.Combine(CaminhoArquivoFinal(), NomeArquivoFinal(pathVideoPrincipal));

            LegendarVideosDataObject legendarVideosDataObject = new LegendarVideosDataObject();

            legendarVideosDataObject.Video = pathVideoPrincipal;
            legendarVideosDataObject.Legendas = pathLegendas;
            legendarVideosDataObject.FinalComLegenda = finalComLegenda;
            legendarVideosDataObject.OnProgress = progressLine =>
            {
                Console.WriteLine("PROGRESS: " + progressLine);
            };

            var videoFinal = VideoStackerTemplate.AdicionarLegendasAoVideo(legendarVideosDataObject);

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
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
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
