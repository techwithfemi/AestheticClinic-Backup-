using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HAttendanceSummItem
{
    public long Sno { get; set; }

    public string ItemName { get; set; } = null!;

    public long NumVal { get; set; }

    public DateTime? DtDate { get; set; }
}
