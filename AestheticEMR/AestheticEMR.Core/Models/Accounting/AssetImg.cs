using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class AssetImg
{
    public string? ImgId { get; set; }

    public byte[]? Img { get; set; }

    public long SNo { get; set; }
}
