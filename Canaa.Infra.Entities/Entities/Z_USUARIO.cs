using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class Z_USUARIO
{
    public int ID { get; set; }

    public string NOME { get; set; } = null!;

    public string EMAIL { get; set; } = null!;

    public string SENHA { get; set; } = null!;

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public string? COMPANY { get; set; }

    public int? STATUS { get; set; }

    public string? PAPEL { get; set; }

    public string? FOTO { get; set; }

    public string? APELIDO { get; set; }
}
