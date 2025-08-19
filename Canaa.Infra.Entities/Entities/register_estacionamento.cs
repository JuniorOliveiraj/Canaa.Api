using System;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities;

public partial class register_estacionamento
{
    public int? register_estacionamento_typeVeiculo { get; set; }

    public int id { get; set; }

    public string? register_estacionamento_placa { get; set; }

    public DateTime? register_estacionamento_chegada { get; set; }

    public DateTime? register_estacionamento_saida { get; set; }

    public TimeOnly? register_estacionamento_duracao { get; set; }

    public int? register_estacionamento_duration { get; set; }

    public string? register_estacionamento_preco { get; set; }

    public string? register_estacionamento_pago { get; set; }

    public int? register_estacionamento_complet { get; set; }
}
