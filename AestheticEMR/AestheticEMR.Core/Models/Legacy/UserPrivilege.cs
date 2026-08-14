using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class UserPrivilege
{
    [StringLength(50)]
    [Unicode(false)]
    public string UserName { get; set; } = null!;

    public bool? mnuFile { get; set; }

    public bool? mnuRec { get; set; }

    public bool? mnuNurse { get; set; }

    public bool? mnuConsulting { get; set; }

    public bool? mnuPharm { get; set; }

    public bool? mnuLab { get; set; }

    public bool? mnuAdmission { get; set; }

    public bool? mnuClinic { get; set; }

    public bool? mnuclient { get; set; }

    public bool? mnuHR { get; set; }

    public bool? mnuFin { get; set; }

    public bool? mnuSec { get; set; }

    public bool? mnuCon { get; set; }

    public bool? mnuPControl { get; set; }

    public bool? mnuExclusive { get; set; }

    public bool? roleID { get; set; }
}
