using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class card
{
    public int id { get; set; }

    public int? id_user { get; set; }

    public string? cardNumber { get; set; }

    public string? cardValid { get; set; }

    public string? cardType { get; set; }
}
