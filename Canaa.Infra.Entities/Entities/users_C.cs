using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class users_C
{
    public int id { get; set; }

    public string? users_C_name { get; set; }

    public string? users_C_passwd { get; set; }

    public DateOnly? users_C_register { get; set; }
}
