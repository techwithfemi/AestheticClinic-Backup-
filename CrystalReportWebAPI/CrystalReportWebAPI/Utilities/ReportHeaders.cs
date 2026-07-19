namespace CrystalReportWebAPI.Utilities
{
    public static class ReportHeaders
    {
        public static string BuildProfitAndLossHeader(string rptBy, string year, string period, string reportItemName, string companyName, bool isClosed, System.DateTime periodCloseDate)
        {
            var item = string.IsNullOrWhiteSpace(reportItemName) ? string.Empty : reportItemName.Trim();
            var by = string.IsNullOrWhiteSpace(rptBy) ? "Period" : rptBy.Trim();
            var yr = string.IsNullOrWhiteSpace(year) ? string.Empty : year.Trim();
            var prd = string.IsNullOrWhiteSpace(period) ? string.Empty : period.Trim();

            switch (by)
            {
                case "Period":
                    return isClosed ? $"{item} For Period ended {periodCloseDate.ToShortDateString()}" : $"{item} For Period ended {prd}";
                case "QTR_1":
                    return $"{item} Consolidated 1st Qtr Report Details {yr}";
                case "QTR_2":
                    return $"{item} Consolidated 2nd Qtr Report Details {yr}";
                case "QTR_3":
                    return $"{item} Consolidated 3rd Qtr Report Details {yr}";
                case "QTR_4":
                    return $"{item} Consolidated 4th Qtr Report Details {yr}";
                case "HALF_YR_1":
                    return $"{item} Consolidated 1st Half Year Report Details {yr}";
                case "HALF_YR_2":
                    return $"{item} Consolidated 2nd Half Year Report Details {yr}";
                case "Year":
                    return $"{item} For Year ended {yr}";
                default:
                    return $"{item} For Period ended {prd}";
            }
        }
    }
}
