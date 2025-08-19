using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class saldo_conta_notion
{
    public string notion_id { get; set; } = null!;

    public string? notion_name { get; set; }

    public string? notion_url { get; set; }

    public string? notion_property_conta { get; set; }

    public DateTime? notion_data { get; set; }

    public string? notion_status { get; set; }

    public string? notion_receita { get; set; }
}
