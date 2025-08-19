using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class participante
{
    public int id { get; set; }

    public string nome { get; set; } = null!;

    public int? view { get; set; }
}
