using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class Order
{
    public int ID { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? OrderDate { get; set; }

    public int? EmployeeID { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? RequiredDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ShippedDate { get; set; }

    public int? ShipVia { get; set; }

    [Column(TypeName = "money")]
    public decimal? Freight { get; set; }

    [StringLength(50)]
    public string? ShipName { get; set; }

    [StringLength(60)]
    public string? ShipAddress { get; set; }

    [StringLength(50)]
    public string? ShipCity { get; set; }

    [StringLength(50)]
    public string? ShipRegion { get; set; }

    [StringLength(50)]
    public string? ShipPostalCode { get; set; }

    [StringLength(50)]
    public string? ShipCountry { get; set; }

    public int? CustomerID { get; set; }
}
