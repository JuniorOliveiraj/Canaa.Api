using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class mirante_cupon
{
    public int id { get; set; }

    public string nome { get; set; } = null!;

    public int usus { get; set; }

    public DateOnly data_por_dia { get; set; }

    public int semana_do_ano { get; set; }

    public int mes_do_ano { get; set; }

    public string? status { get; set; }
}
