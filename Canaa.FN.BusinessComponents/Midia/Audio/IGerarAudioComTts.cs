using Canaa.DataContracts.AzureTTS;
using Canaa.FN.BusinessComponents.Response;

namespace Canaa.FN.BusinessComponents.Midia.Audio
{
    public interface IGerarAudioComTts
    {
        Task<ResponseDataContrac> GerarAudioAsync(TtsCreateDataObject data);
    }
}