using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class compras_cartao
{
    public int id { get; set; }

    public int? id_user { get; set; }

    public int? id_card { get; set; }

    public string? compra_nome { get; set; }

    public int? categoriastatus { get; set; }

    public DateOnly? compra_data { get; set; }

    public TimeOnly? compra_hora { get; set; }

    public decimal? compra_valor { get; set; }

    public int? compra_status { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public string? descricao { get; set; }
}
