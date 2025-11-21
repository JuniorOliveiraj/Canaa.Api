using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Canaa.TempModels;

[Table("sorteios")]
[Index("IdSorteador", Name = "id_sorteador")]
public partial class Sorteios
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_sorteador")]
    public int IdSorteador { get; set; }

    [Column("nome_sorteado")]
    [StringLength(255)]
    public string NomeSorteado { get; set; } = null!;

    [Column("viewed")]
    public int? Viewed { get; set; }

    [ForeignKey("IdSorteador")]
    [InverseProperty("Sorteios")]
    public virtual Participants IdSorteadorNavigation { get; set; } = null!;
}
