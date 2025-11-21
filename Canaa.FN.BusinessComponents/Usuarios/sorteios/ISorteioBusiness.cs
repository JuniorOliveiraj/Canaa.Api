using Canaa.DataContracts.Service;
using Canaa.FN.BusinessComponents.Response;
using Canaa.TempModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.FN.BusinessComponents.Usuarios.sorteios
{
    public interface ISorteioBusiness
    {
        List<Participants> BuscarParticipantes();
        ResponseDataContrac AdicionarParticipants(ParticipantesRequest participante);
        Task<ResponseDataContrac> GerarNovoSorteio();
        ResponseDataContrac DeletarParticipante(int idParticipante);
        void AterarStatusSorteios();
        void ApagarSorteios();
    }
}
