using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class mirante_imagen
{
    public int id_imagem { get; set; }

    public int? id_produto { get; set; }

    public string nome_arquivo { get; set; } = null!;

    public string url_arquivo { get; set; } = null!;
}
