using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class Order
{
    public int Id { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? RequiredDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public int? ShipVia { get; set; }

    public decimal? Freight { get; set; }

    public string? ShipName { get; set; }

    public string? ShipAddress { get; set; }

    public string? ShipCity { get; set; }

    public string? ShipRegion { get; set; }

    public string? ShipPostalCode { get; set; }

    public string? ShipCountry { get; set; }

    public int? CustomerId { get; set; }
}
