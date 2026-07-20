namespace CrystalReportWebAPI.Utilities
{
    public static class ReportHeaders
    {
        public static string BuildProfitAndLossHeader(string rptBy, string year, string period, string reportItemName, string companyName, bool isClosed, System.DateTime periodCloseDate)
        {
            var prefix = string.IsNullOrWhiteSpace(reportItemName) ? string.Empty : reportItemName.Trim() + " ";
            var by = string.IsNullOrWhiteSpace(rptBy) ? "Period" : rptBy.Trim();
            var yr = string.IsNullOrWhiteSpace(year) ? string.Empty : year.Trim();
            var prd = string.IsNullOrWhiteSpace(period) ? string.Empty : period.Trim();

            switch (by)
            {
                case "Period":
                    var dateStr = periodCloseDate != System.DateTime.MinValue
                        ? periodCloseDate.ToShortDateString()
                        : prd;
                    return $"{prefix}For Period ended {dateStr}";
                case "QTR_1":
                    return $"{prefix}Consolidated 1st Qtr Report {yr}";
                case "QTR_2":
                    return $"{prefix}Consolidated 2nd Qtr Report {yr}";
                case "QTR_3":
                    return $"{prefix}Consolidated 3rd Qtr Report {yr}";
                case "QTR_4":
                    return $"{prefix}Consolidated 4th Qtr Report {yr}";
                case "HALF_YR_1":
                    return $"{prefix}Consolidated 1st Half Year Report {yr}";
                case "HALF_YR_2":
                    return $"{prefix}Consolidated 2nd Half Year Report {yr}";
                case "Year":
                    return $"{prefix}For Year ended {yr}";
                default:
                    var fallbackDate = periodCloseDate != System.DateTime.MinValue
                        ? periodCloseDate.ToShortDateString()
                        : prd;
                    return $"{prefix}For Period ended {fallbackDate}";
            }
        }
    }
}
