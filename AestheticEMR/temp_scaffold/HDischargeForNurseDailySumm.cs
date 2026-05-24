using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HDischargeForNurseDailySumm
{
    public long Id { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? DischTime { get; set; }

    public string Pno { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public string WardId { get; set; } = null!;

    public string SummDischbyNurse { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime? ApptDate { get; set; }

    public string? ApprvBy { get; set; }

    public string WhatDay { get; set; } = null!;
}
