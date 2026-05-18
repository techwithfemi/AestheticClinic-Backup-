using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AestheticEMR.Core.Models.Legacy;

public partial class BillingDetail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string billNO { get; set; } 

    public long SNO { get; set; } = 0;

    public DateTime dtDate { get; set; }

    public string drgName { get; set; } // same as bill item

    public double Price { get; set; }

    public double Qty { get; set; }

    public decimal? subTotal { get; set; }

    public string? billType { get; set; }=null!;

    public string? conID { get; set; }=null!;

    public string? Capitated { get; set; }="NO"!;

    public string? Dosage { get; set; }=null!;

    public string? Category { get; set; }=null!;

    public string BillTo { get; set; } // same as coyname in attendance

    public string CoyName { get; set; } // same as coyname in attendance, for audit trail

    public string? BillHead { get; set; } = null!;

    public string? revType { get; set; } = null!;

    public string? DRGCode { get; set; } = null!;

    public bool isPost { get; set; }=false;

    public bool? isRct { get; set; } = false;

    public string? BillBy { get; set; }// same as empID in attendance, for audit trail

    public string? treatedBy { get; set; } //same as doctor in attendance, for audit trail

    public string? Dept { get; set; }=null!;

    public bool? isOLD { get; set; } = false;

    //public DateTime? EntryDate { get; set; }

    //public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; } // device name that posted the billing detail, for audit trail

    public string? AppName { get; set; }// name of the application that posted the billing detail, for audit trail

    public string? RevClinic { get; set; } // for revenue clinic

    //public decimal? AmtPaid { get; set; } = 0;

    public bool? Reversed { get; set; } = false;

    public string? Remarks { get; set; }= null!;

    public bool? suppres { get; set; } = false;

    public int? AppVersion { get; set; } = 1;

    //public string? TranID { get; set; }

    //public long? ReversedPair { get; set; }
}
