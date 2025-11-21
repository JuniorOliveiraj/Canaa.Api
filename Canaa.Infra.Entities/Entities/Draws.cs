using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Canaa.TempModels;

[Table("draws")]
[Index("ParticipantId", Name = "participant_id", IsUnique = true)]
public partial class Draws
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("participant_id")]
    public int ParticipantId { get; set; }

    [Column("drawn_name")]
    [StringLength(255)]
    public string DrawnName { get; set; } = null!;

    [Column("viewed")]
    public bool Viewed { get; set; }

    [ForeignKey("ParticipantId")]
    [InverseProperty("Draws")]
    public virtual Participants Participant { get; set; } = null!;
}
