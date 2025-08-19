using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class produtosagro
{
    public int id { get; set; }

    public string name_produto { get; set; } = null!;

    public string quantidade_produto { get; set; } = null!;

    public string valor_produto { get; set; } = null!;

    public string imagem_produto { get; set; } = null!;

    public int? status_produto { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }
}
