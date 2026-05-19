using System;
using System.Collections.Generic;
using System.Text;

namespace AestheticEMR.Core.Models.Legacy
{
    public class ProductTariff
    {
        public long SNO { get; set; } // auto-increment
        public string PdtName { get; set; } = null!; //item 
        public string? Category { get; set; }
        public string? Company { get; set; } // coyID   
        public double? Price { get; set; }
        public string? Remarks { get; set; }
        public string? CoyName { get; set; } //company name
        public string? Capitated { get; set; } = "NO";
        public string? TariffStatus { get; set; } = "FIXED";
        public string? RevType { get; set; }
        public string? UsersCat { get; set; }
    }
}