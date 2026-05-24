using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class UserPrivilege
{
    public string UserName { get; set; } = null!;

    public bool? MnuFile { get; set; }

    public bool? MnuRec { get; set; }

    public bool? MnuNurse { get; set; }

    public bool? MnuConsulting { get; set; }

    public bool? MnuPharm { get; set; }

    public bool? MnuLab { get; set; }

    public bool? MnuAdmission { get; set; }

    public bool? MnuClinic { get; set; }

    public bool? Mnuclient { get; set; }

    public bool? MnuHr { get; set; }

    public bool? MnuFin { get; set; }

    public bool? MnuSec { get; set; }

    public bool? MnuCon { get; set; }

    public bool? MnuPcontrol { get; set; }

    public bool? MnuExclusive { get; set; }

    public bool? RoleId { get; set; }
}
