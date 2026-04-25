using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class hReferal
{
    public long ID { get; set; }

    public string consultID { get; set; } = null!;

    public string clientCat { get; set; } = null!;

    public DateTime? apptDate { get; set; }

    public DateTime? apptTime { get; set; }

    public string? referTo { get; set; }

    public string? pNo { get; set; }

    public string? refReason { get; set; }

    public DateTime? refDate { get; set; }

    public DateTime? refTime { get; set; }

    public bool? AttendedTo { get; set; }

    public string? refAddress { get; set; }

    public string? conID { get; set; }

    public bool? suppres { get; set; }

    public string? Comments { get; set; }

    public bool? AttendedToByRec { get; set; }

    public string? EmpID { get; set; }

    public string? Remarks { get; set; }
}
