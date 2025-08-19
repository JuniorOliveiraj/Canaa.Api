using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class Z_TAREFA
{
    public int ID { get; set; }

    public Guid? GUID { get; set; }

    public string? URL { get; set; }

    public string? Status { get; set; }

    public int? PROGRESSO { get; set; }

    public string? FILEPATH { get; set; }

    public string? ERRO { get; set; }

    public string? CATEGORIA { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
