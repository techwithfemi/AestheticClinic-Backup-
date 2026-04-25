using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class hAppointment
{
    public long ID { get; set; }


    //public string consultID { get; set; } = null!;

    //public string clientCat { get; set; } = null!;

    public string pno { get; set; } = null!;
    public DateTime? entryDate { get; set; } // current date when the appointment is created

    public DateTime? entryTime { get; set; } // current time when the appointment is created

    public DateTime? ApptDate { get; set; }

    public DateTime? ApptTime { get; set; }

    public string? clinicType { get; set; }

    public string? remarks { get; set; }

    public string? EmpID { get; set; } // Employee who created the appointment

 //public bool? attendedTo { get; set; }

    //public string? conID { get; set; }

    //public bool? suppres { get; set; }

    //public string? RetainCode { get; set; }


    //public bool? AttendedToByRec { get; set; }
}
