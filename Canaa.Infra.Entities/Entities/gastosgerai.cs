using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class gastosgerai
{
    public int id { get; set; }

    public int? user_id { get; set; }

    public DateOnly? compra_data { get; set; }

    public decimal? valor { get; set; }

    public string? categoria { get; set; }

    public string? descricao { get; set; }
}
