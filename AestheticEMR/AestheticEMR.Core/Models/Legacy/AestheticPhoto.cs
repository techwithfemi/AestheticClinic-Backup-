using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Index("ConsultationId", Name = "IX_AestheticPhotos_ConsultationId")]
public partial class AestheticPhoto
{
    [Key]
    public int Id { get; set; }

    public int ConsultationId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string FileName { get; set; } = null!;

    [StringLength(4000)]
    [Unicode(false)]
    public string FilePath { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Type { get; set; }

    [StringLength(40)]
    public string? CreatedBy { get; set; }

    [StringLength(40)]
    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ConsultId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PNo { get; set; }

    [ForeignKey("ConsultationId")]
    [InverseProperty("AestheticPhotos")]
    public virtual AestheticConsultation Consultation { get; set; } = null!;
}
