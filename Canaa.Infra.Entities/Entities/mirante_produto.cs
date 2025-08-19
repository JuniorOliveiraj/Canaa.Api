using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class mirante_produto
{
    public int id_produto { get; set; }

    public int? id_subcategoria { get; set; }

    public string nome_produto { get; set; } = null!;
}
