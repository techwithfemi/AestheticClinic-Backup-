using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhExpenseCat
{
    [StringLength(7)]
    [Unicode(false)]
    public string CatCode { get; set; } = null!;

    [StringLength(225)]
    [Unicode(false)]
    public string CatName { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string? AcctID { get; set; }

    [StringLength(225)]
    [Unicode(false)]
    public string CatType { get; set; } = null!;

    [StringLength(225)]
    [Unicode(false)]
    public string? Description { get; set; }
}
