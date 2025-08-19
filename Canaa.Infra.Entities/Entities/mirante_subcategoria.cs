using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class mirante_subcategoria
{
    public int id_subcategoria { get; set; }

    public int? id_categoria { get; set; }

    public string nome_subcategoria { get; set; } = null!;
}
