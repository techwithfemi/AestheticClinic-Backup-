using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhConsultingVisitsList
{
    public DateTime Date { get; set; }

    public string Remarks { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public string? Referal { get; set; }

    public string? PCatId { get; set; }

    public string RetainCode { get; set; } = null!;

    public string? Ref { get; set; }

    public string? ClientCatId { get; set; }

    public string PNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string? PFirstname { get; set; }

    public string ConsultId { get; set; } = null!;

    public bool? IsBilled { get; set; }

    public string RetainId { get; set; } = null!;

    public string? ClientCat { get; set; }

    public string Clinic { get; set; } = null!;
}
