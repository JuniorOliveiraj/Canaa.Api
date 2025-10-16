using Canaa.AppHost.utils.Query;
using Canaa.DataContracts.Videos;
using Canaa.FN.BusinessComponents.Response;
using Canaa.FN.BusinessComponents.Utils;
using Canaa.Infra.ExternalServices.Utils;
using Canaa.Infra.ExternalServices.Youtube;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.FN.BusinessComponents.Midia.Video.Youtube
{
    public class YoutubeComponent : IYoutubeComponent
    {
        public async Task<ResponseDataContrac> BaixarVideo(string link)
        {
            ResponseDataContrac response = new ResponseDataContrac();
            string _guid = Guid.NewGuid().ToString();
            ProgressService.InsertNewTask(_guid, "");

            try
            {
                var videoPath = await YoutubeDownloader.DownloadVideoAsync(
                    link,
                    CanaaContext.DiretorioTemporario("Youtube"),
                    _guid
                );

                if (videoPath == "Erro")
                {
                    response.success = false;
                    response.error = BuscarErro(_guid);
                }
                else
                {
                    string url = CanaaContext.GetApiLocation();
                    url += $"/v1/Arquivos/stream?caminho={videoPath}";
                    response.data = new string[] { videoPath, url };
                    response.success = true;
                    response.message = "Vídeo baixado com sucesso!";
                    ProgressService.UpdateTaskFilePath(_guid, videoPath);
                    ProgressService.UpdateTaskFileUrl(_guid, url);
                    ProgressService.SetTaskCategoria(_guid, TarefasCategorias.BaixarVideoYoutube);
                    ProgressService.SetConcluido(_guid);
                }
            }
            catch (YoutubeExplode.Exceptions.VideoUnavailableException)
            {
                response.success = false;
                response.error = "O vídeo não está disponível no YouTube.";
            }
            catch (Exception ex)
            {
                response.success = false;
                response.error = $"Erro ao baixar o vídeo: {ex.Message}";
            }

            return response;
        }


        public async Task<ResponseDataContrac> BaixarLegendasYoutube(string link)
        {
            ResponseDataContrac response = new ResponseDataContrac();
            string _guid = Guid.NewGuid().ToString();
            var videoPath = await YoutubeDownloader.DownloadCaptionsAsync(link, CanaaContext.DiretorioTemporario("Youtube"), _guid);
            response.success = true;
            if (videoPath == "Erro")
            {
                response.success = false;
                response.error = BuscarErro(_guid);
            }
            string url = CanaaContext.GetApiLocation();
            url += $"/v1/Arquivos/download?caminho={videoPath}";
            response.data = new string[] { videoPath, url };
            response.message = "Legendas baixadas com sucesso!";
            ProgressService.UpdateTaskFilePath(_guid, videoPath);
            ProgressService.UpdateTaskFileUrl(_guid, url);
            ProgressService.SetTaskCategoria(_guid, TarefasCategorias.BaixarLegenda);
            ProgressService.SetConcluido(_guid);
            return response;
        }

        public async Task<ResponseDataContrac> BaixarAudioYoutube(string link)
        {
            ResponseDataContrac response = new ResponseDataContrac();
            string _guid = Guid.NewGuid().ToString();
            ProgressService.InsertNewTask(_guid, "");
            var videoPath = await YoutubeDownloader.DownloadAudioAsync(link, CanaaContext.DiretorioTemporario("Youtube"), _guid);
            response.success = true;
            if (videoPath == "Erro")
            {
                response.success = false;
                response.error = BuscarErro(_guid);
            }
            string url = CanaaContext.GetApiLocation();
            url += $"/v1/Arquivos/audio?caminho={videoPath}";
            response.data = new string[] { videoPath, url };
            response.message = "Audio baixado com sucesso!";
            ProgressService.UpdateTaskFilePath(_guid, videoPath);
            ProgressService.UpdateTaskFileUrl(_guid, url);
            ProgressService.SetTaskCategoria(_guid, TarefasCategorias.BaixarAudio);
            ProgressService.SetConcluido(_guid);
            return response;
        }



        private string BuscarErro(string guid)
        {
            var query = new Query(@"
                SELECT ERROR
                FROM Z_TAREFAS
                WHERE GUID = :GUID");
            query.AddParameter(new Parameter("GUID", guid));
            var result = query.Execute().FirstOrDefault();
            if (result != null)
                return result.ToString();

            return string.Empty;

        }
    }
}
