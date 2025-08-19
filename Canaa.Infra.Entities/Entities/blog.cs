using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class blog
{
    public int id { get; set; }

    public string? title { get; set; }

    public string? description { get; set; }

    public string? content { get; set; }

    public string? cover_link { get; set; }

    public int? publish { get; set; }

    public int? comments { get; set; }

    public string? meta_title { get; set; }

    public string? meta_description { get; set; }

    public int? user_id { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public string? type { get; set; }
}
