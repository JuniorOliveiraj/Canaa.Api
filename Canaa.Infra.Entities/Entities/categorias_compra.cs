using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class categorias_compra
{
    public int id_categoria { get; set; }

    public string nome_categoria { get; set; } = null!;

    public string? icon { get; set; }

    public virtual ICollection<gastos_mensais_notion> id_compras { get; set; } = new List<gastos_mensais_notion>();
}
