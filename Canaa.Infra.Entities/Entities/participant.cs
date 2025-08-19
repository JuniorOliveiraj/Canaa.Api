using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class participant
{
    public int id { get; set; }

    public string name { get; set; } = null!;

    public virtual draw? draw { get; set; }

    public virtual ICollection<sorteio> sorteios { get; set; } = new List<sorteio>();
}
