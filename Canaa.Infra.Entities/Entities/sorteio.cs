using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class sorteio
{
    public int id { get; set; }

    public int id_sorteador { get; set; }

    public string nome_sorteado { get; set; } = null!;

    public int? viewed { get; set; }

    public virtual participant id_sorteadorNavigation { get; set; } = null!;
}
