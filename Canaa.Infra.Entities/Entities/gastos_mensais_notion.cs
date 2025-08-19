using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class gastos_mensais_notion
{
    public int id { get; set; }

    public string? handleNotion { get; set; }

    public string? name { get; set; }

    public string? url { get; set; }

    public float? gasto_esse_mes { get; set; }

    public float? valor { get; set; }

    public DateTime? data { get; set; }

    public string? status { get; set; }

    public string? conta { get; set; }

    public int? categoriastatus { get; set; }

    public string? avatarImage { get; set; }

    public string? descricao { get; set; }

    public string? conta_origem { get; set; }

    public virtual ICollection<categorias_compra> id_categoria { get; set; } = new List<categorias_compra>();
}
