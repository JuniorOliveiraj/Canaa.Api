using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class news
{
    public int id { get; set; }

    public int? user_id { get; set; }

    public string title { get; set; } = null!;

    public string description { get; set; } = null!;

    public string content { get; set; } = null!;

    public string url { get; set; } = null!;

    public string image { get; set; } = null!;

    public string publishedAt { get; set; } = null!;

    public string source_name { get; set; } = null!;

    public string source_url { get; set; } = null!;

    public int status { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public int? lida { get; set; }

    public string? q { get; set; }

    public int? type { get; set; }
}
