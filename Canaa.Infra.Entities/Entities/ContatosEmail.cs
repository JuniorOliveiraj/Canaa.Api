using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Canaa.Infra.Entities.Entities;

[Table("contatos_email")]
public partial class ContatosEmail
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("tipo_contato")]
    [StringLength(100)]
    public string TipoContato { get; set; } = null!;

    [Column("nome_empresa")]
    [StringLength(150)]
    public string NomeEmpresa { get; set; } = null!;

    [Column("categoria")]
    [StringLength(100)]
    public string? Categoria { get; set; }

    [Column("cidade")]
    [StringLength(100)]
    public string? Cidade { get; set; }

    [Column("dominio")]
    [StringLength(150)]
    public string? Dominio { get; set; }

    [Column("email_principal")]
    [StringLength(150)]
    public string? EmailPrincipal { get; set; }

    [Column("email_comercial")]
    [StringLength(150)]
    public string? EmailComercial { get; set; }

    [Column("site_link")]
    [StringLength(255)]
    public string? SiteLink { get; set; }

    [Column("observacoes", TypeName = "text")]
    public string? Observacoes { get; set; }

    [Column("criado_em", TypeName = "timestamp")]
    public DateTime? CriadoEm { get; set; }

    [Column("imagem")]
    [StringLength(255)]
    public string? Imagem { get; set; }
}
