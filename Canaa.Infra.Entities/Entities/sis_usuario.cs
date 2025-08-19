using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class sis_usuario
{
    public int id { get; set; }

    public string? ds_nome { get; set; }

    public string? ds_email { get; set; }

    public string? ds_telefone { get; set; }

    public int? ds_status { get; set; }
}
