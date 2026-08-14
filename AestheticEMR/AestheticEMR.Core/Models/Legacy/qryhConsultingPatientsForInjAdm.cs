using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConsultingPatientsForInjAdm
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime InjDate { get; set; }

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(150)]
    public string pSurname { get; set; } = null!;

    [StringLength(150)]
    public string? pFirstname { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(2500)]
    public string InjName { get; set; } = null!;

    [StringLength(500)]
    public string drgCatName { get; set; } = null!;

    public int? numOfTimes { get; set; }

    public int? numTaken { get; set; }

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(2500)]
    public string? Dosage { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;
}
