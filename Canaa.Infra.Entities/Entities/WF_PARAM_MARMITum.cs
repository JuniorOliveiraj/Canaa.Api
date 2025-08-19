using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class WF_PARAM_MARMITum
{
    public int ID { get; set; }

    public string? TEXTO { get; set; }

    public string? INSTANCIA { get; set; }

    public long? NUMERO { get; set; }

    public string? DIADASEMANA { get; set; }

    public int? ENVIAR { get; set; }

    public int? NOTIFICARENVIO { get; set; }

    public DateTime? DATA_ULTIMA_ALTERACAO { get; set; }
}
