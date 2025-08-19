using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class draw
{
    public int id { get; set; }

    public int participant_id { get; set; }

    public string drawn_name { get; set; } = null!;

    public bool viewed { get; set; }

    public virtual participant participant { get; set; } = null!;
}
