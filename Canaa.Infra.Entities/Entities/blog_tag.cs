using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class blog_tag
{
    public int id { get; set; }

    public int? blog_id { get; set; }

    public string? tag_value { get; set; }
}
