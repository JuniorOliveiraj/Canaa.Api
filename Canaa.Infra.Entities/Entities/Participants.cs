using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Canaa.TempModels;

[Table("participants")]
[Index("Name", Name = "name", IsUnique = true)]
public partial class Participants
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("phone")]
    [StringLength(20)]
    public string? Phone { get; set; }

    [InverseProperty("Participant")]
    public virtual Draws? Draws { get; set; }

    [InverseProperty("IdSorteadorNavigation")]
    public virtual ICollection<Sorteios> Sorteios { get; set; } = new List<Sorteios>();
}
