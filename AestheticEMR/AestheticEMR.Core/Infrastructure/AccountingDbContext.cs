using System;
using System.Collections.Generic;
using AestheticEMR.Core.Models.Accounting;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core.Infrastructure;

public partial class AccountingDbContext : DbContext
{
    public AccountingDbContext(DbContextOptions<AccountingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountMonth> AccountMonths { get; set; }

    public virtual DbSet<AccountMonthOpen> AccountMonthOpens { get; set; }

    public virtual DbSet<AccountingYear> AccountingYears { get; set; }

    public virtual DbSet<AcctPeriodType> AcctPeriodTypes { get; set; }

    public virtual DbSet<AcctPeriodTypesDetail> AcctPeriodTypesDetails { get; set; }

    public virtual DbSet<AppDefault> AppDefaults { get; set; }

    public virtual DbSet<AssetDepartment> AssetDepartments { get; set; }

    public virtual DbSet<AssetDepreciation> AssetDepreciations { get; set; }

    public virtual DbSet<AssetDepreciationMaster> AssetDepreciationMasters { get; set; }

    public virtual DbSet<AssetDisposal> AssetDisposals { get; set; }

    public virtual DbSet<AssetGroup> AssetGroups { get; set; }

    public virtual DbSet<AssetGroupX> AssetGroupXes { get; set; }

    public virtual DbSet<AssetImg> AssetImgs { get; set; }

    public virtual DbSet<AssetLocation> AssetLocations { get; set; }

    public virtual DbSet<AssetMaster> AssetMasters { get; set; }

    public virtual DbSet<AssetSubGroup> AssetSubGroups { get; set; }

    public virtual DbSet<AssetSubGroupX> AssetSubGroupXes { get; set; }

    public virtual DbSet<AssetTransfer> AssetTransfers { get; set; }

    public virtual DbSet<Auditrail> Auditrails { get; set; }

    public virtual DbSet<BalanceSheetHeader> BalanceSheetHeaders { get; set; }

    public virtual DbSet<BalanceSheetHeaders2> BalanceSheetHeaders2s { get; set; }

    public virtual DbSet<BalanceSheetHeaders3> BalanceSheetHeaders3s { get; set; }

    public virtual DbSet<BranchDept> BranchDepts { get; set; }

    public virtual DbSet<ChartOfAccount> ChartOfAccounts { get; set; }

    public virtual DbSet<ChartOfAccountMaster> ChartOfAccountMasters { get; set; }

    public virtual DbSet<ChartOfAccountMaster010421> ChartOfAccountMaster010421s { get; set; }

    public virtual DbSet<ChartOfAccountMaster_20260101_052234> ChartOfAccountMaster_20260101_052234s { get; set; }

    public virtual DbSet<ChartOfAccountMaster_20260104_141252> ChartOfAccountMaster_20260104_141252s { get; set; }

    public virtual DbSet<ChartOfAccounts010421> ChartOfAccounts010421s { get; set; }

    public virtual DbSet<ChartOfAccounts160221> ChartOfAccounts160221s { get; set; }

    public virtual DbSet<ChartOfAccountsArchive> ChartOfAccountsArchives { get; set; }

    public virtual DbSet<ChartOfAccountsBalSheet> ChartOfAccountsBalSheets { get; set; }

    public virtual DbSet<ChartOfAccountsClosedPeriod> ChartOfAccountsClosedPeriods { get; set; }

    public virtual DbSet<ChartOfAccountsOPBal> ChartOfAccountsOPBals { get; set; }

    public virtual DbSet<ChartOfAccountsPreArchive> ChartOfAccountsPreArchives { get; set; }

    public virtual DbSet<ChartOfAccountsPreArchive2> ChartOfAccountsPreArchive2s { get; set; }

    public virtual DbSet<ChartOfAccountsTemp> ChartOfAccountsTemps { get; set; }

    public virtual DbSet<ChartOfAccountsTesting> ChartOfAccountsTestings { get; set; }

    public virtual DbSet<ChartOfAccountsTestingX> ChartOfAccountsTestingXes { get; set; }

    public virtual DbSet<ChartOfAccounts_20260101_052234> ChartOfAccounts_20260101_052234s { get; set; }

    public virtual DbSet<ChartOfAccounts_20260104_141252> ChartOfAccounts_20260104_141252s { get; set; }

    public virtual DbSet<ChartOfAccounts_BeginBalance_From_Excel> ChartOfAccounts_BeginBalance_From_Excels { get; set; }

    public virtual DbSet<ChartOfAccounts_BeginBalance_Monitor> ChartOfAccounts_BeginBalance_Monitors { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CostCenter> CostCenters { get; set; }

    public virtual DbSet<DateListingForPeriod> DateListingForPeriods { get; set; }

    public virtual DbSet<DateMonitorForTranID> DateMonitorForTranIDs { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Division> Divisions { get; set; }

    public virtual DbSet<GroupCat> GroupCats { get; set; }

    public virtual DbSet<GroupCatMaster> GroupCatMasters { get; set; }

    public virtual DbSet<GroupItem> GroupItems { get; set; }

    public virtual DbSet<IDgen> IDgens { get; set; }

    public virtual DbSet<LedgerCategory> LedgerCategories { get; set; }

    public virtual DbSet<Logs_Migration> Logs_Migrations { get; set; }

    public virtual DbSet<Period> Periods { get; set; }

    public virtual DbSet<PeriodEndBalance> PeriodEndBalances { get; set; }

    public virtual DbSet<PeriodEndBalanceQry> PeriodEndBalanceQries { get; set; }

    public virtual DbSet<PeriodParam> PeriodParams { get; set; }

    public virtual DbSet<PeriodTempBalance> PeriodTempBalances { get; set; }

    public virtual DbSet<ProfitAndLossHeader> ProfitAndLossHeaders { get; set; }

    public virtual DbSet<ProfitAndLossHeaders2> ProfitAndLossHeaders2s { get; set; }

    public virtual DbSet<RandomID> RandomIDs { get; set; }

    public virtual DbSet<ReportHeader> ReportHeaders { get; set; }

    public virtual DbSet<ReportSummary> ReportSummaries { get; set; }

    public virtual DbSet<StockValuationAcct> StockValuationAccts { get; set; }

    public virtual DbSet<StockValuationAcct2> StockValuationAcct2s { get; set; }

    public virtual DbSet<TestTranTable1> TestTranTable1s { get; set; }

    public virtual DbSet<TranCat> TranCats { get; set; }

    public virtual DbSet<TranFromAppsTrail> TranFromAppsTrails { get; set; }

    public virtual DbSet<TranxFromApp> TranxFromApps { get; set; }

    public virtual DbSet<Tranxaction> Tranxactions { get; set; }

    public virtual DbSet<TranxactionArchive> TranxactionArchives { get; set; }

    public virtual DbSet<TranxactionArchiveMonitor> TranxactionArchiveMonitors { get; set; }

    public virtual DbSet<TranxactionBalance> TranxactionBalances { get; set; }

    public virtual DbSet<TranxactionBalanceBK> TranxactionBalanceBKs { get; set; }

    public virtual DbSet<TranxactionDeleted> TranxactionDeleteds { get; set; }

    public virtual DbSet<TranxactionJournal> TranxactionJournals { get; set; }

    public virtual DbSet<TranxactionJournalExternal> TranxactionJournalExternals { get; set; }

    public virtual DbSet<TranxactionJournalTemp> TranxactionJournalTemps { get; set; }

    public virtual DbSet<TranxactionPreArchive> TranxactionPreArchives { get; set; }

    public virtual DbSet<TranxactionPreArchive2> TranxactionPreArchive2s { get; set; }

    public virtual DbSet<TranxactionSuspense> TranxactionSuspenses { get; set; }

    public virtual DbSet<TranxactionTemp> TranxactionTemps { get; set; }

    public virtual DbSet<TranxactionTemp_OLD> TranxactionTemp_OLDs { get; set; }

    public virtual DbSet<XXvwStockCloseBal> XXvwStockCloseBals { get; set; }

    public virtual DbSet<XXvwStockPurchased> XXvwStockPurchaseds { get; set; }

    public virtual DbSet<groupItemsReset> groupItemsResets { get; set; }

    public virtual DbSet<qrySysDateTime> qrySysDateTimes { get; set; }

    public virtual DbSet<vwAccountInfoPandL> vwAccountInfoPandLs { get; set; }

    public virtual DbSet<vwAccountMasterInfo> vwAccountMasterInfos { get; set; }

    public virtual DbSet<vwAccountMasterInfo2> vwAccountMasterInfo2s { get; set; }

    public virtual DbSet<vwAccountMonthInFinYear> vwAccountMonthInFinYears { get; set; }

    public virtual DbSet<vwAccountMonthOpen> vwAccountMonthOpens { get; set; }

    public virtual DbSet<vwAccountsInfo> vwAccountsInfos { get; set; }

    public virtual DbSet<vwAccountsInfoBalSheet> vwAccountsInfoBalSheets { get; set; }

    public virtual DbSet<vwAccountsInfoBalSheet2> vwAccountsInfoBalSheet2s { get; set; }

    public virtual DbSet<vwAccountsInfoBalSheet2Temp> vwAccountsInfoBalSheet2Temps { get; set; }

    public virtual DbSet<vwAccountsInfoCombo> vwAccountsInfoCombos { get; set; }

    public virtual DbSet<vwAccountsInfoForRpt> vwAccountsInfoForRpts { get; set; }

    public virtual DbSet<vwAccountsInfoGL> vwAccountsInfoGLs { get; set; }

    public virtual DbSet<vwAccountsInfoGLforConfirm> vwAccountsInfoGLforConfirms { get; set; }

    public virtual DbSet<vwAcctPeriodTypesDetail> vwAcctPeriodTypesDetails { get; set; }

    public virtual DbSet<vwAppDefaults_DeprGp> vwAppDefaults_DeprGps { get; set; }

    public virtual DbSet<vwAssetDepreciation> vwAssetDepreciations { get; set; }

    public virtual DbSet<vwBalanceSheetHeader> vwBalanceSheetHeaders { get; set; }

    public virtual DbSet<vwBalanceSheetHeaders2> vwBalanceSheetHeaders2s { get; set; }

    public virtual DbSet<vwBalanceSheetHeadersByYear> vwBalanceSheetHeadersByYears { get; set; }

    public virtual DbSet<vwBalanceSheetHeadersByYearPL> vwBalanceSheetHeadersByYearPLs { get; set; }

    public virtual DbSet<vwBanksAndCash> vwBanksAndCashes { get; set; }

    public virtual DbSet<vwBanksAndCashAndBBE> vwBanksAndCashAndBBEs { get; set; }

    public virtual DbSet<vwBranchDept> vwBranchDepts { get; set; }

    public virtual DbSet<vwChartOfAccount> vwChartOfAccounts { get; set; }

    public virtual DbSet<vwChartOfAccountMasterForDelete> vwChartOfAccountMasterForDeletes { get; set; }

    public virtual DbSet<vwChartOfAccountsClosingPeriodsDue> vwChartOfAccountsClosingPeriodsDues { get; set; }

    public virtual DbSet<vwClosedAndOpenPeriod> vwClosedAndOpenPeriods { get; set; }

    public virtual DbSet<vwClosedAndOpenPeriods2> vwClosedAndOpenPeriods2s { get; set; }

    public virtual DbSet<vwClosingAndClosedPeriodsUnion> vwClosingAndClosedPeriodsUnions { get; set; }

    public virtual DbSet<vwCompany> vwCompanies { get; set; }

    public virtual DbSet<vwCompanyAndOpenPeriod> vwCompanyAndOpenPeriods { get; set; }

    public virtual DbSet<vwConfirmFinRpt> vwConfirmFinRpts { get; set; }

    public virtual DbSet<vwConfirmFinRpt_COA> vwConfirmFinRpt_COAs { get; set; }

    public virtual DbSet<vwConfirmFinRpt_COA_2_XXXX> vwConfirmFinRpt_COA_2_XXXXes { get; set; }

    public virtual DbSet<vwCostCenter> vwCostCenters { get; set; }

    public virtual DbSet<vwDivision> vwDivisions { get; set; }

    public virtual DbSet<vwGL> vwGLs { get; set; }

    public virtual DbSet<vwGL2> vwGL2s { get; set; }

    public virtual DbSet<vwGLCOA> vwGLCOAs { get; set; }

    public virtual DbSet<vwGLforRpt> vwGLforRpts { get; set; }

    public virtual DbSet<vwGLforRptGrouped> vwGLforRptGroupeds { get; set; }

    public virtual DbSet<vwGLforRptPL> vwGLforRptPLs { get; set; }

    public virtual DbSet<vwGLforRptPLGrouped> vwGLforRptPLGroupeds { get; set; }

    public virtual DbSet<vwGLforRpt_SelfJoin> vwGLforRpt_SelfJoins { get; set; }

    public virtual DbSet<vwGroupCatForFixedAsset> vwGroupCatForFixedAssets { get; set; }

    public virtual DbSet<vwGroupItem> vwGroupItems { get; set; }

    public virtual DbSet<vwGroupItemsFixedAsset> vwGroupItemsFixedAssets { get; set; }

    public virtual DbSet<vwGroupItemsForBalSheet> vwGroupItemsForBalSheets { get; set; }

    public virtual DbSet<vwGroupItemsForBalSheet2> vwGroupItemsForBalSheet2s { get; set; }

    public virtual DbSet<vwGroupItemsNoSuppress> vwGroupItemsNoSuppresses { get; set; }

    public virtual DbSet<vwGroupItemsWithoutDepr> vwGroupItemsWithoutDeprs { get; set; }

    public virtual DbSet<vwGroupItemsWithoutFixedAssetsOrDepr> vwGroupItemsWithoutFixedAssetsOrDeprs { get; set; }

    public virtual DbSet<vwLocationsIP> vwLocationsIPs { get; set; }

    public virtual DbSet<vwPeriodUnionForRpt> vwPeriodUnionForRpts { get; set; }

    public virtual DbSet<vwProfitAndLossDetail> vwProfitAndLossDetails { get; set; }

    public virtual DbSet<vwProfitAndLossHeader> vwProfitAndLossHeaders { get; set; }

    public virtual DbSet<vwProfitAndLossHeadersByYear> vwProfitAndLossHeadersByYears { get; set; }

    public virtual DbSet<vwProfitAndLossHeadersList> vwProfitAndLossHeadersLists { get; set; }

    public virtual DbSet<vwProfitOrLoss> vwProfitOrLosses { get; set; }

    public virtual DbSet<vwProfitOrLoss2> vwProfitOrLoss2s { get; set; }

    public virtual DbSet<vwProfitOrLossClosePrd> vwProfitOrLossClosePrds { get; set; }

    public virtual DbSet<vwReportHeader> vwReportHeaders { get; set; }

    public virtual DbSet<vwReportSummary> vwReportSummaries { get; set; }

    public virtual DbSet<vwReportSummary2> vwReportSummary2s { get; set; }

    public virtual DbSet<vwReportSummaryOriginal> vwReportSummaryOriginals { get; set; }

    public virtual DbSet<vwShowMaxPeriodInReport> vwShowMaxPeriodInReports { get; set; }

    public virtual DbSet<vwStockEntryForValuationAcct> vwStockEntryForValuationAccts { get; set; }

    public virtual DbSet<vwStockEntryForValuationAcctPharmacy> vwStockEntryForValuationAcctPharmacies { get; set; }

    public virtual DbSet<vwStockEntryForValuationAcctStore> vwStockEntryForValuationAcctStores { get; set; }

    public virtual DbSet<vwStockSalesAndCOG> vwStockSalesAndCOGs { get; set; }

    public virtual DbSet<vwStockSalesAndCOGSGrouped> vwStockSalesAndCOGSGroupeds { get; set; }

    public virtual DbSet<vwStockValuationAcct> vwStockValuationAccts { get; set; }

    public virtual DbSet<vwStockValuationAcctGrouped> vwStockValuationAcctGroupeds { get; set; }

    public virtual DbSet<vwStockValuationAcctGroupedSalesAndCOG> vwStockValuationAcctGroupedSalesAndCOGs { get; set; }

    public virtual DbSet<vwTotalAsset> vwTotalAssets { get; set; }

    public virtual DbSet<vwTotalCurrentAsset> vwTotalCurrentAssets { get; set; }

    public virtual DbSet<vwTotalDirectExpense> vwTotalDirectExpenses { get; set; }

    public virtual DbSet<vwTotalDirectIncome> vwTotalDirectIncomes { get; set; }

    public virtual DbSet<vwTotalEquity> vwTotalEquities { get; set; }

    public virtual DbSet<vwTotalExpense> vwTotalExpenses { get; set; }

    public virtual DbSet<vwTotalFixedAsset> vwTotalFixedAssets { get; set; }

    public virtual DbSet<vwTotalInDirectExpense> vwTotalInDirectExpenses { get; set; }

    public virtual DbSet<vwTotalInDirectIncome> vwTotalInDirectIncomes { get; set; }

    public virtual DbSet<vwTotalIncome> vwTotalIncomes { get; set; }

    public virtual DbSet<vwTotalLiability> vwTotalLiabilities { get; set; }

    public virtual DbSet<vwTotalLiabilityAndEquity> vwTotalLiabilityAndEquities { get; set; }

    public virtual DbSet<vwTotalTax> vwTotalTaxes { get; set; }

    public virtual DbSet<vwTranx> vwTranxes { get; set; }

    public virtual DbSet<vwTranxArchive> vwTranxArchives { get; set; }

    public virtual DbSet<vwTranxDebitCreditGroupedByAccountID> vwTranxDebitCreditGroupedByAccountIDs { get; set; }

    public virtual DbSet<vwTranxDebitCreditGroupedByPeriod> vwTranxDebitCreditGroupedByPeriods { get; set; }

    public virtual DbSet<vwTranxForGrid> vwTranxForGrids { get; set; }

    public virtual DbSet<vwTranxForGridTemp> vwTranxForGridTemps { get; set; }

    public virtual DbSet<vwTranxGroupedByAccountID> vwTranxGroupedByAccountIDs { get; set; }

    public virtual DbSet<vwTranxGroupedByAccountIDCrossTab> vwTranxGroupedByAccountIDCrossTabs { get; set; }

    public virtual DbSet<vwTranxJournalTemp> vwTranxJournalTemps { get; set; }

    public virtual DbSet<vwTranxJournalTempWithNoDummyAcctNo> vwTranxJournalTempWithNoDummyAcctNos { get; set; }

    public virtual DbSet<vwTranxNo> vwTranxNos { get; set; }

    public virtual DbSet<vwTranxNoOLD> vwTranxNoOLDs { get; set; }

    public virtual DbSet<vwTranxWithPeriodVal> vwTranxWithPeriodVals { get; set; }

    public virtual DbSet<vwTranxaction> vwTranxactions { get; set; }

    public virtual DbSet<vwTranxaction2> vwTranxaction2s { get; set; }

    public virtual DbSet<vwTranxactionAndChartOfAccount> vwTranxactionAndChartOfAccounts { get; set; }

    public virtual DbSet<vwTranxactionAndChartOfAccountsForRptSummary> vwTranxactionAndChartOfAccountsForRptSummaries { get; set; }

    public virtual DbSet<vwTranxactionAndChartOfAccountsGrouped> vwTranxactionAndChartOfAccountsGroupeds { get; set; }

    public virtual DbSet<vwTranxactionAndChartOfAccountsGroupedTesting> vwTranxactionAndChartOfAccountsGroupedTestings { get; set; }

    public virtual DbSet<vwTranxactionArchiveForGrid> vwTranxactionArchiveForGrids { get; set; }

    public virtual DbSet<vwTranxactionArchiveMonitor> vwTranxactionArchiveMonitors { get; set; }

    public virtual DbSet<vwTranxactionGrouped> vwTranxactionGroupeds { get; set; }

    public virtual DbSet<vwTrialBalance> vwTrialBalances { get; set; }

    public virtual DbSet<vwTrialBalanceByPeriod> vwTrialBalanceByPeriods { get; set; }

    public virtual DbSet<vwTrialBalanceGL> vwTrialBalanceGLs { get; set; }

    public virtual DbSet<vwTrialBalanceGroup> vwTrialBalanceGroups { get; set; }

    public virtual DbSet<vwcheckPeriodExist> vwcheckPeriodExists { get; set; }

    public virtual DbSet<vwtranxJournal> vwtranxJournals { get; set; }

    public virtual DbSet<vwtranxJournalExpress> vwtranxJournalExpresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountMonth>(entity =>
        {
            entity.HasKey(e => new { e.MonthCounter, e.PeriodYr, e.CoyID }).HasName("pk_AccountMonth");

            entity.ToTable("AccountMonth");

            entity.Property(e => e.PeriodYr)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0001", "DF__AccountMo__CoyID__19FFD4FC");
            entity.Property(e => e.AcctMonth).HasColumnType("datetime");
            entity.Property(e => e.Period)
                .HasMaxLength(53)
                .IsUnicode(false)
                .HasComputedColumnSql("((right('00'+CONVERT([varchar](2),[monthcounter],(0)),(2))+'/')+[periodyr])", false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(52)
                .IsUnicode(false)
                .HasComputedColumnSql("([periodyr]+right('00'+CONVERT([varchar](2),[monthcounter],(0)),(2)))", false);
            entity.Property(e => e.PrdClose).HasColumnType("datetime");
            entity.Property(e => e.Suppres).HasDefaultValue(false, "DF__AccountMo__Suppr__4C8B54C9");
        });

        modelBuilder.Entity<AccountMonthOpen>(entity =>
        {
            entity.HasKey(e => new { e.CoyID, e.MonthCounter, e.PeriodYr }).HasName("pk_AccountMonthOpen");

            entity.ToTable("AccountMonthOpen");

            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0001");
            entity.Property(e => e.MonthCounter).HasComment("Part of pri key bcos period is a computed col of yr/mothcounter");
            entity.Property(e => e.PeriodYr)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctMonth).HasColumnType("datetime");
            entity.Property(e => e.Period)
                .HasMaxLength(53)
                .IsUnicode(false)
                .HasComputedColumnSql("((right('00'+CONVERT([varchar](2),[monthcounter],(0)),(2))+'/')+[periodyr])", false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(52)
                .IsUnicode(false)
                .HasComputedColumnSql("([periodyr]+right('00'+CONVERT([varchar](2),[monthcounter],(0)),(2)))", false);
            entity.Property(e => e.PrdClose).HasColumnType("datetime");
            entity.Property(e => e.Suppres).HasDefaultValue(false);
        });

        modelBuilder.Entity<AccountingYear>(entity =>
        {
            entity.HasKey(e => new { e.CoyID, e.FinYear });

            entity.ToTable("AccountingYear");

            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FinYear)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.PrdType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.diffVal).HasComment("nece to det startmonth of fin year diff from calendar year of Jan 1. to be set in db manually by sa");
        });

        modelBuilder.Entity<AcctPeriodType>(entity =>
        {
            entity.HasKey(e => e.PrdType).HasName("PK_PeriodTypes");

            entity.Property(e => e.PrdType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<AcctPeriodTypesDetail>(entity =>
        {
            entity.HasKey(e => new { e.PrdType, e.Mth }).HasName("PK_AcctPeriodTypesDetail_1");

            entity.ToTable("AcctPeriodTypesDetail");

            entity.Property(e => e.PrdType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FinPrd2)
                .HasMaxLength(2)
                .HasComputedColumnSql("(right('00'+CONVERT([nvarchar],[FinPrd],(0)),(2)))", false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<AppDefault>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_AppDefaults_1");

            entity.Property(e => e.ID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IDVal)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AssetDepartment>(entity =>
        {
            entity.HasKey(e => e.SNo).HasName("PK__AssetDep__CA1EE04C3671F678");

            entity.ToTable("AssetDepartment");

            entity.Property(e => e.DptCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasComputedColumnSql("('DPT-'+right('00000'+CONVERT([varchar](5),[SNo],(0)),(5)))", false);
            entity.Property(e => e.DptName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AssetDepreciation>(entity =>
        {
            entity.HasKey(e => new { e.AccountID, e.Period, e.CoyID });

            entity.ToTable("AssetDepreciation");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountNoAccumDepr)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(app_name())", "DF__AssetDepr__AppNa__2B354DF6");
            entity.Property(e => e.CalPeriod)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasComputedColumnSql("((right('00'+CONVERT([varchar](2),datepart(month,[deprdate]),(0)),(2))+'/')+CONVERT([varchar](4),datepart(year,[deprdate]),(0)))", false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(host_name())", "DF__AssetDepr__Clien__2A4129BD");
            entity.Property(e => e.DeprDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))", "DF__AssetDepr__Entry__2858E14B")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryTime)
                .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))", "DF__AssetDepr__Entry__294D0584")
                .HasColumnType("datetime");
            entity.Property(e => e.Mth)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasComputedColumnSql("(right('00'+CONVERT([varchar](2),datepart(month,[deprdate]),(0)),(2)))", false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.Yr)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasComputedColumnSql("(CONVERT([varchar](4),datepart(year,[deprdate]),(0)))", false);
            entity.Property(e => e.suppres).HasDefaultValue(false);
        });

        modelBuilder.Entity<AssetDepreciationMaster>(entity =>
        {
            entity.HasKey(e => e.AccountID).HasName("PK_DepreciationMaster");

            entity.ToTable("AssetDepreciationMaster");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AQuireDate).HasColumnType("datetime");
            entity.Property(e => e.AccumDeprAmount)
                .HasDefaultValue(0m, "DF_DepreciationMaster_AccumDeprAmount")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Active).HasDefaultValue(false, "DF_DepreciationMaster_Active");
            entity.Property(e => e.AssetCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DateLastDepr).HasColumnType("datetime");
            entity.Property(e => e.DeprAmount)
                .HasComputedColumnSql("(([grossvalue]-([AccumDeprAmount]+[ScrapValue]))/([DurationInMths]-[deprcount]))", false)
                .HasColumnType("decimal(31, 13)");
            entity.Property(e => e.DeprCount).HasDefaultValue(0, "DF_DepreciationMaster_DeprCount");
            entity.Property(e => e.DisposalDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate)
                .HasComputedColumnSql("(dateadd(month,[DurationInMths]-(1),[startDate]))", false)
                .HasColumnType("datetime");
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))", "DF_DepreciationMaster_EntryDate")
                .HasColumnType("datetime");
            entity.Property(e => e.GrossValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.ScrapValue)
                .HasDefaultValue(0m, "DF_DepreciationMaster_SalvageValue")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<AssetDisposal>(entity =>
        {
            entity.HasKey(e => e.AssetCode).HasName("PK_AssetDisposal_1");

            entity.ToTable("AssetDisposal");

            entity.Property(e => e.AssetCode)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.AssetName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Dates).HasColumnType("datetime");
            entity.Property(e => e.DisposalReason)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FormNo)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.NetBookValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProfitLoss).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SalesValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UselLife)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AssetGroup>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("AssetGroup");

            entity.Property(e => e.GroupCode)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AssetGroupX>(entity =>
        {
            entity.HasKey(e => e.SNo).HasName("PK_AssetGroup");

            entity.ToTable("AssetGroupX");

            entity.Property(e => e.GroupCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComputedColumnSql("('GC-'+right('00'+CONVERT([varchar](2),[SNo],(0)),(2)))", false);
            entity.Property(e => e.GroupName).HasMaxLength(50);
        });

        modelBuilder.Entity<AssetImg>(entity =>
        {
            entity.HasKey(e => e.SNo).HasName("PK__AssetImg__CA1EE04C0A295FE6");

            entity.ToTable("AssetImg");

            entity.Property(e => e.Img).HasColumnType("image");
            entity.Property(e => e.ImgId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AssetLocation>(entity =>
        {
            entity.HasKey(e => e.SNo).HasName("PK__AssetLoc__CA1EE04C3C2ACFCE");

            entity.ToTable("AssetLocation");

            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.LocationCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComputedColumnSql("('LC-'+right('00'+CONVERT([varchar](2),[SNo],(0)),(2)))", false);
            entity.Property(e => e.LocName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AssetMaster>(entity =>
        {
            entity.HasKey(e => e.SNo).HasName("PK_AssetMaster_1");

            entity.ToTable("AssetMaster");

            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctNoAccumDepr)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(app_name())", "DF__AssetMast__AppNa__2F05DEDA");
            entity.Property(e => e.AssetCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasComputedColumnSql("('AC-'+right('00000'+CONVERT([varchar](5),[SNo],(0)),(5)))", false);
            entity.Property(e => e.AssetDesc)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.AssetName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AssetValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(host_name())", "DF__AssetMast__Clien__2E11BAA1");
            entity.Property(e => e.CoyID)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DepEndDate)
                .HasComputedColumnSql("(dateadd(month,[UserLife]*(12)-(1),[DepStartDate]))", false)
                .HasColumnType("datetime");
            entity.Property(e => e.DepStartDate).HasColumnType("datetime");
            entity.Property(e => e.DepartmentCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DeprActive).HasDefaultValue(true, "DF_AssetMaster_Active");
            entity.Property(e => e.DeprCount).HasDefaultValue(0, "DF_AssetMaster_DeprCount");
            entity.Property(e => e.DepreciationRate)
                .HasComputedColumnSql("(([AssetValue]-[ScrapValue])/([UserLife]*(12)))", false)
                .HasColumnType("decimal(38, 19)");
            entity.Property(e => e.DisposedDate).HasColumnType("datetime");
            entity.Property(e => e.DisposedYN)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.DurationInMths)
                .HasComputedColumnSql("([UserLife]*(12))", false)
                .HasColumnType("decimal(21, 2)");
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))", "DF__AssetMast__Entry__2C29722F")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryTime)
                .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))", "DF__AssetMast__Entry__2D1D9668")
                .HasColumnType("datetime");
            entity.Property(e => e.GroupCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Img).HasColumnType("image");
            entity.Property(e => e.InsuranceAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InsuranceCompany)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InsuranceNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LocationCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.NetBookValue)
                .HasComputedColumnSql("([AssetValue]-([TotalDepreciation]+[ScrapValue]))", false)
                .HasColumnType("decimal(20, 2)");
            entity.Property(e => e.PolicyType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PremiumValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PurchaseDate).HasColumnType("datetime");
            entity.Property(e => e.ScrapValue)
                .HasDefaultValue(0m, "DF_AssetMaster_DepreciationRate1")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SubGroupCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Supplier)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalDepreciation)
                .HasDefaultValue(0m, "DF_AssetMaster_TotalDepreciation")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UserLife).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<AssetSubGroup>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("AssetSubGroup");

            entity.Property(e => e.GroupCode)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SubGroupCode)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.SubGroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AssetSubGroupX>(entity =>
        {
            entity.HasKey(e => e.SNo).HasName("PK__AssetSub__CA1EE04C63449CEF");

            entity.ToTable("AssetSubGroupX");

            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.GroupCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SubGroupCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComputedColumnSql("('SG-'+right('00'+CONVERT([varchar](2),[SNo],(0)),(2)))", false);
            entity.Property(e => e.SubGroupName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AssetTransfer>(entity =>
        {
            entity.HasKey(e => e.FormNo);

            entity.ToTable("AssetTransfer");

            entity.Property(e => e.FormNo)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.AssetCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.AssetName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Dates).HasColumnType("datetime");
            entity.Property(e => e.FrmAssetDpt)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FrmAssetGrp)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FrmAssetLcn)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FrmAssetSbGrp)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.NetBookValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Reason)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.ToAssetDpt)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ToAssetGrp)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ToAssetLcn)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ToAssetSbGrp)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Auditrail>(entity =>
        {
            entity.ToTable("Auditrail");

            entity.Property(e => e.ActionDate).HasColumnType("datetime");
            entity.Property(e => e.ActionTime).HasColumnType("datetime");
            entity.Property(e => e.Remarks).HasMaxLength(550);
            entity.Property(e => e.Src).HasMaxLength(150);
            entity.Property(e => e.TranCode).HasMaxLength(50);
            entity.Property(e => e.UserAction).HasMaxLength(500);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<BalanceSheetHeader>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Accounts_Payable)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Accounts Payable");
            entity.Property(e => e.Accounts_Receivable)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Accounts Receivable");
            entity.Property(e => e.Accrued_Income_Tax)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Accrued Income Tax");
            entity.Property(e => e.Accumulated_Depreciation)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Accumulated Depreciation");
            entity.Property(e => e.All_SUM).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Bank_Accounts)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Bank Accounts");
            entity.Property(e => e.Bank_OD_Accounts)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Bank OD Accounts");
            entity.Property(e => e.Capital_Account)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Capital Account");
            entity.Property(e => e.Cash).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Duties___Trade_Taxes)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Duties & Trade Taxes");
            entity.Property(e => e.Equity).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Inventory).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Land___Building)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Land & Building");
            entity.Property(e => e.Machinery).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Other_Current_Assets)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Other Current Assets");
            entity.Property(e => e.Other_Fixed_Assets)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Other Fixed Assets");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Retained_Earnings)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Retained Earnings");
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Secured_Loans)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Secured Loans");
            entity.Property(e => e.Shares).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Unsecured_Loans)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Unsecured Loans");
            entity.Property(e => e.Vehicles).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<BalanceSheetHeaders2>(entity =>
        {
            entity.HasKey(e => new { e.CoyID, e.Period }).HasName("PK_BalanceSheetHeaders");

            entity.ToTable("BalanceSheetHeaders2");

            entity.Property(e => e.CoyID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Accounts_Payable)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Accounts Payable")
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Accounts Payable");
            entity.Property(e => e.Accounts_Receivable)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Accounts Receivable")
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Accounts Receivable");
            entity.Property(e => e.Accrued_Income_Tax)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Accrued Income Tax")
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Accrued Income Tax");
            entity.Property(e => e.Accumulated_Depreciation)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Accumulated Depreciation")
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Accumulated Depreciation");
            entity.Property(e => e.Bank_Accounts)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Bank Accounts")
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Bank Accounts");
            entity.Property(e => e.Bank_OD_Accounts)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Bank OD Accounts")
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Bank OD Accounts");
            entity.Property(e => e.Capital_Account)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Capital Account")
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Capital Account");
            entity.Property(e => e.Cash)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Cash")
                .HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Deposits)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Deposits")
                .HasColumnType("decimal(38, 2)");
            entity.Property(e => e.DirectCost)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_DirectCost")
                .HasColumnType("decimal(38, 2)");
            entity.Property(e => e.DirectIncome)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_DirectIncome")
                .HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Duties___Trade_Taxes)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Duties & Trade Taxes")
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Duties & Trade Taxes");
            entity.Property(e => e.Equity)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Equity")
                .HasColumnType("decimal(38, 2)");
            entity.Property(e => e.IndirectCost)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_IndirectCost")
                .HasColumnType("decimal(38, 2)");
            entity.Property(e => e.IndirectIncome)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_IndirectIncome")
                .HasColumnType("decimal(38, 2)");
            entity.Property(e => e.InterestPayable)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_InterestPayable")
                .HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Inventory)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Inventory")
                .HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Land___Building)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Land & Building")
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Land & Building");
            entity.Property(e => e.Machinery)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Machinery")
                .HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Other_Current_Assets)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Other Current Assets")
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Other Current Assets");
            entity.Property(e => e.Other_Fixed_Assets)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Other Fixed Assets")
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Other Fixed Assets");
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Retained_Earnings)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Retained Earnings")
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Retained Earnings");
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.Secured_Loans)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Secured Loans")
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Secured Loans");
            entity.Property(e => e.Shares)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Shares")
                .HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Taxation)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Taxation")
                .HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Total)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Total")
                .HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Unsecured_Loans)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Unsecured Loans")
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Unsecured Loans");
            entity.Property(e => e.Vehicles)
                .HasDefaultValue(0m, "DF_BalanceSheetHeaders_Vehicles")
                .HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<BalanceSheetHeaders3>(entity =>
        {
            entity.HasKey(e => new { e.CoyID, e.Period }).HasName("PK_BalanceSheetHeaders_1");

            entity.ToTable("BalanceSheetHeaders3");

            entity.Property(e => e.CoyID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Accounts_Payable)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Accounts Payable");
            entity.Property(e => e.Accounts_Receivable)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Accounts Receivable");
            entity.Property(e => e.Accrued_Income_Tax)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Accrued Income Tax");
            entity.Property(e => e.Accumulated_Depreciation)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Accumulated Depreciation");
            entity.Property(e => e.All_SUM).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Bank_Accounts)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Bank Accounts");
            entity.Property(e => e.Bank_OD_Accounts)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Bank OD Accounts");
            entity.Property(e => e.Capital_Account)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Capital Account");
            entity.Property(e => e.Cash).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Deposits).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Duties___Trade_Taxes)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Duties & Trade Taxes");
            entity.Property(e => e.Equity).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Inventory).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Land___Building)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Land & Building");
            entity.Property(e => e.Machinery).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Other_Current_Assets)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Other Current Assets");
            entity.Property(e => e.Other_Fixed_Assets)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Other Fixed Assets");
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Retained_Earnings)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Retained Earnings");
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Secured_Loans)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Secured Loans");
            entity.Property(e => e.Shares).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Unsecured_Loans)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Unsecured Loans");
            entity.Property(e => e.Vehicles).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<BranchDept>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("BranchDept");

            entity.Property(e => e.DeptID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasComputedColumnSql("(right('000'+CONVERT([varchar](10),[sno],(0)),(3)))", false);
            entity.Property(e => e.DeptName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.DivID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ChartOfAccount>(entity =>
        {
            entity.HasKey(e => new { e.AccountID, e.Period, e.CoyID }).HasName("PK_ChartOfAccounts_20260104_141252");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountSalesTaxNo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DeprAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DeprEndDate).HasColumnType("datetime");
            entity.Property(e => e.DeprNextDate).HasColumnType("datetime");
            entity.Property(e => e.DeprStartDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.EntryTime).HasColumnType("datetime");
            entity.Property(e => e.ExtID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0000", "DF_COA_ExtID_20260114_054745");
            entity.Property(e => e.ExtIDType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("ACCOUNT", "DF_COA_ExtIDType_20260114_054745");
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.isContra)
                .IsRequired()
                .HasDefaultValueSql("('0')", "DF_COA_isContra_20260114_054745");
        });

        modelBuilder.Entity<ChartOfAccountMaster>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ChartOfAccountMaster");

            entity.Property(e => e.AccountAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountSalesTaxNo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DeprAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DeprEndDate).HasColumnType("datetime");
            entity.Property(e => e.DeprNextDate).HasColumnType("datetime");
            entity.Property(e => e.DeprStartDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.EntryTime).HasColumnType("datetime");
            entity.Property(e => e.ExtID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExtIDType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ChartOfAccountMaster010421>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ChartOfAccountMaster010421");

            entity.Property(e => e.AccountAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountSalesTaxNo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DeprAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DeprEndDate).HasColumnType("datetime");
            entity.Property(e => e.DeprNextDate).HasColumnType("datetime");
            entity.Property(e => e.DeprStartDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.EntryTime).HasColumnType("datetime");
            entity.Property(e => e.ExtID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExtIDType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ChartOfAccountMaster_20260101_052234>(entity =>
        {
            entity.HasKey(e => e.AccountID).HasName("PK_ChartOfAccountMaster");

            entity.ToTable("ChartOfAccountMaster_20260101_052234");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountSalesTaxNo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(app_name())", "DF__ChartOfAc__AppNa__702996C1");
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(host_name())", "DF__ChartOfAc__Clien__6F357288");
            entity.Property(e => e.DeprAmount)
                .HasDefaultValue(0m, "DF__ChartOfAc__DeprA__1352D76D")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DeprCount).HasDefaultValue(0, "DF__ChartOfAc__DeprC__1446FBA6");
            entity.Property(e => e.DeprEndDate).HasColumnType("datetime");
            entity.Property(e => e.DeprNextDate).HasColumnType("datetime");
            entity.Property(e => e.DeprStartDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))", "DF__ChartOfAc__Entry__6D4D2A16")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryTime)
                .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))", "DF__ChartOfAc__Entry__6E414E4F")
                .HasColumnType("datetime");
            entity.Property(e => e.ExtID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExtIDType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Hidden).HasDefaultValue(false, "DF_ChartOfAccountMaster_Hidden");
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.isDummy).HasDefaultValue(false, "DF__ChartOfAc__isDum__0F824689");
            entity.Property(e => e.isPerm).HasDefaultValue(false, "DF__ChartOfAc__isPer__5575A085");
        });

        modelBuilder.Entity<ChartOfAccountMaster_20260104_141252>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ChartOfAccountMaster_20260104_141252");

            entity.Property(e => e.AccountAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountSalesTaxNo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DeprAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DeprEndDate).HasColumnType("datetime");
            entity.Property(e => e.DeprNextDate).HasColumnType("datetime");
            entity.Property(e => e.DeprStartDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.EntryTime).HasColumnType("datetime");
            entity.Property(e => e.ExtID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExtIDType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ChartOfAccounts010421>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ChartOfAccounts010421");

            entity.Property(e => e.AccountAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountSalesTaxNo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeprAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DeprEndDate).HasColumnType("datetime");
            entity.Property(e => e.DeprNextDate).HasColumnType("datetime");
            entity.Property(e => e.DeprStartDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.EntryTime).HasColumnType("datetime");
            entity.Property(e => e.ExtID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExtIDType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ChartOfAccounts160221>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ChartOfAccounts160221");

            entity.Property(e => e.AccountAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountSalesTaxNo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeprAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DeprEndDate).HasColumnType("datetime");
            entity.Property(e => e.DeprNextDate).HasColumnType("datetime");
            entity.Property(e => e.DeprStartDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.EntryTime).HasColumnType("datetime");
            entity.Property(e => e.ExtID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExtIDType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ChartOfAccountsArchive>(entity =>
        {
            entity.HasKey(e => new { e.AccountID, e.Period, e.CoyID });

            entity.ToTable("ChartOfAccountsArchive");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountSalesTaxNo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ExtID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0000", "DF_ChartOfAccountsArchive_ExtID");
            entity.Property(e => e.ExtIDType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("ACCOUNT", "DF_ChartOfAccountsArchive_ExtIDType");
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Hidden).HasDefaultValue(false, "DF_ChartOfAccountsArchive_Hidden");
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.isDummy).HasDefaultValue(false, "DF__ChartOfAc__dummy__48EFCE0F");
        });

        modelBuilder.Entity<ChartOfAccountsBalSheet>(entity =>
        {
            entity.HasKey(e => new { e.AccountID, e.Period, e.CoyID });

            entity.ToTable("ChartOfAccountsBalSheet");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountSalesTaxNo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Hidden).HasDefaultValue(false, "DF_ChartOfAccountsBalSheet_Hidden");
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ChartOfAccountsClosedPeriod>(entity =>
        {
            entity.HasKey(e => new { e.AccountID, e.Period, e.CoyID }).HasName("pk_ChartOfAccountsClosedPeriod");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0001", "DF_ChartOfAccountsClosedPeriod_CoyID");
            entity.Property(e => e.AccountAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountSalesTaxNo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(app_name())");
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(host_name())");
            entity.Property(e => e.DeprAmount)
                .HasDefaultValue(0m, "DF__ChartOfAcCL__DeprA__75F77EB0")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DeprCount).HasDefaultValue(0, "DF__ChartOfAcCL__DeprC__76EBA2E9");
            entity.Property(e => e.DeprEndDate).HasColumnType("datetime");
            entity.Property(e => e.DeprNextDate).HasColumnType("datetime");
            entity.Property(e => e.DeprStartDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryTime)
                .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))")
                .HasColumnType("datetime");
            entity.Property(e => e.ExtID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0000", "DF_ChartOfAccountsClosedPeriod_ExtID");
            entity.Property(e => e.ExtIDType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("ACCOUNT", "DF_ChartOfAccountsClosedPeriod_ExtIDType");
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Hidden).HasDefaultValue(false, "DF_ChartOfAccountsClosedPeriod_Hidden");
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.isDummy).HasDefaultValue(false, "DF_ChartOfAccountsClosedPeriod_isDummy");
            entity.Property(e => e.isPerm).HasDefaultValue(false, "DF__ChartOfAcCL__isPer__5669C4BE");
        });

        modelBuilder.Entity<ChartOfAccountsOPBal>(entity =>
        {
            entity.HasKey(e => new { e.AccountID, e.CoyID, e.Period });

            entity.ToTable("ChartOfAccountsOPBal");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CLAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValue("OP BAL", "DF_ChartOfAccountsOPBal_Description");
            entity.Property(e => e.OPAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.TranDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ChartOfAccountsPreArchive>(entity =>
        {
            entity.HasKey(e => new { e.AccountID, e.Period });

            entity.ToTable("ChartOfAccountsPreArchive");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountSalesTaxNo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Hidden).HasDefaultValue(false, "DF__ChartOfAc__Hidde__17F790F9");
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ChartOfAccountsPreArchive2>(entity =>
        {
            entity.HasKey(e => new { e.AccountID, e.Period });

            entity.ToTable("ChartOfAccountsPreArchive2");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountSalesTaxNo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Hidden).HasDefaultValue(false, "DF__ChartOfAc__Hidde__1AD3FDA4");
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ChartOfAccountsTemp>(entity =>
        {
            entity.HasKey(e => new { e.AccountID, e.Period, e.CoyID });

            entity.ToTable("ChartOfAccountsTemp");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountSalesTaxNo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ExtID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExtIDType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Hidden).HasDefaultValue(false, "DF_ChartOfAccountsTemp_Hidden");
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.isDummy).HasDefaultValue(false, "DF__ChartOfAcTemp__dummy__48EFCE0F");
        });

        modelBuilder.Entity<ChartOfAccountsTesting>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ChartOfAccountsTesting");

            entity.Property(e => e.AccountAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountSalesTaxNo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeprAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DeprEndDate).HasColumnType("datetime");
            entity.Property(e => e.DeprNextDate).HasColumnType("datetime");
            entity.Property(e => e.DeprStartDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.EntryTime).HasColumnType("datetime");
            entity.Property(e => e.ExtID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExtIDType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ChartOfAccountsTestingX>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ChartOfAccountsTestingX");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ChartOfAccounts_20260101_052234>(entity =>
        {
            entity.HasKey(e => new { e.AccountID, e.Period, e.CoyID }).HasName("PK_ChartOfAccounts_1");

            entity.ToTable("ChartOfAccounts_20260101_052234");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0001", "DF_ChartOfAccounts_CoyID");
            entity.Property(e => e.AccountAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountSalesTaxNo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(app_name())");
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(host_name())");
            entity.Property(e => e.DeprAmount)
                .HasDefaultValue(0m, "DF__ChartOfAc__DeprA__75F77EB0")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DeprCount).HasDefaultValue(0, "DF__ChartOfAc__DeprC__76EBA2E9");
            entity.Property(e => e.DeprEndDate).HasColumnType("datetime");
            entity.Property(e => e.DeprNextDate).HasColumnType("datetime");
            entity.Property(e => e.DeprStartDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryTime)
                .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))")
                .HasColumnType("datetime");
            entity.Property(e => e.ExtID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0000", "DF_ChartOfAccounts_ExtID");
            entity.Property(e => e.ExtIDType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("ACCOUNT", "DF_ChartOfAccounts_ExtIDType");
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Hidden).HasDefaultValue(false, "DF_ChartOfAccounts_Hidden");
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.isDummy).HasDefaultValue(false, "DF_ChartOfAccounts_isDummy");
            entity.Property(e => e.isPerm).HasDefaultValue(false, "DF__ChartOfAc__isPer__5669C4BE");
        });

        modelBuilder.Entity<ChartOfAccounts_20260104_141252>(entity =>
        {
            entity.HasKey(e => new { e.AccountID, e.Period, e.CoyID }).HasName("PK_ChartOfAccounts_20260101_052234");

            entity.ToTable("ChartOfAccounts_20260104_141252");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountSalesTaxNo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DeprAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DeprEndDate).HasColumnType("datetime");
            entity.Property(e => e.DeprNextDate).HasColumnType("datetime");
            entity.Property(e => e.DeprStartDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.EntryTime).HasColumnType("datetime");
            entity.Property(e => e.ExtID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExtIDType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ChartOfAccounts_BeginBalance_From_Excel>(entity =>
        {
            entity.HasKey(e => new { e.CoyID, e.AccountNo });

            entity.ToTable("ChartOfAccounts_BeginBalance_From_Excel");

            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValue("OP BAL", "DF_ChartOfAccounts_BeginBalance_From_Excel_Description");
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OPBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))", "DF_ChartOfAccounts_BeginBalance_From_Excel_TranID");
        });

        modelBuilder.Entity<ChartOfAccounts_BeginBalance_Monitor>(entity =>
        {
            entity.HasKey(e => e.GroupName);

            entity.ToTable("ChartOfAccounts_BeginBalance_Monitor");

            entity.Property(e => e.GroupName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CoyID).HasName("pk_Companoes");

            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Coyname)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<CostCenter>(entity =>
        {
            entity.HasKey(e => e.CenterID).HasName("pk_CostCenters");

            entity.Property(e => e.CenterID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CenterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.DeptID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<DateListingForPeriod>(entity =>
        {
            entity.HasKey(e => new { e.CoyID, e.TranDate }).HasName("PK_DateListingForPeriods_1");

            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<DateMonitorForTranID>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DateMonitorForTranID");

            entity.Property(e => e.DtDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Departments");

            entity.Property(e => e.DeptAddress).HasMaxLength(100);
            entity.Property(e => e.DeptID).HasMaxLength(50);
            entity.Property(e => e.DeptName).HasMaxLength(50);
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Division>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DivID)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasComputedColumnSql("(right('00'+CONVERT([varchar](10),[sno],(0)),(2)))", false);
            entity.Property(e => e.DivName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Location)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<GroupCat>(entity =>
        {
            entity.HasKey(e => e.CatID);

            entity.ToTable("GroupCat");

            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.HiddenCat).HasDefaultValue(false, "DF_GroupCat_HiddenCat");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<GroupCatMaster>(entity =>
        {
            entity.HasKey(e => e.CatMasterID);

            entity.ToTable("GroupCatMaster");

            entity.Property(e => e.CatMasterID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.BalStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.HiddenMaster).HasDefaultValue(false, "DF_GroupCatMaster_HiddenCat");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<GroupItem>(entity =>
        {
            entity.HasKey(e => e.GroupID);

            entity.Property(e => e.GroupID)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CanDepr)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("NO");
            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Editable).HasDefaultValue(true);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.HiddenGp).HasDefaultValue(false, "DF_GroupItems_HiddenGp");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RptLevel)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComputedColumnSql("(substring([GroupID],(1),(1)))", false);
            entity.Property(e => e.RptTitle)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComputedColumnSql("(CONVERT([varchar](50),case when substring([groupiD],(1),(1))=(3) OR substring([groupiD],(1),(1))=(2) OR substring([groupiD],(1),(1))=(1) then 'BS' when substring([groupiD],(1),(1))=(5) OR substring([groupiD],(1),(1))=(4) then 'PL'  end,(0)))", false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.Suppres).HasDefaultValue(false, "DF_GroupItems_Suppres");
        });

        modelBuilder.Entity<IDgen>(entity =>
        {
            entity.HasKey(e => e.DestName);

            entity.ToTable("IDgen");

            entity.Property(e => e.DestName).HasMaxLength(50);
            entity.Property(e => e.ID).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<LedgerCategory>(entity =>
        {
            entity.HasKey(e => e.Serial);

            entity.ToTable("LedgerCategory");

            entity.Property(e => e.Serial).ValueGeneratedNever();
            entity.Property(e => e.Ledger)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LedgerCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LedgerCodeVal)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Logs_Migration>(entity =>
        {
            entity.HasKey(e => e.LogID).HasName("PK__Logs_Mig__5E5499A8FA5643B7");

            entity.ToTable("Logs_Migration");

            entity.Property(e => e.DestDB)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MigrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProcessedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SourceDB)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Period>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(app_name())");
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(host_name())");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryTime)
                .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))")
                .HasColumnType("datetime");
            entity.Property(e => e.Period1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Period");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<PeriodEndBalance>(entity =>
        {
            entity.HasKey(e => e.SNo).HasName("PK_PeriodEndBalance_1");

            entity.ToTable("PeriodEndBalance");

            entity.Property(e => e.AccountID)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0001");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.isTransfer).HasDefaultValue(true);
        });

        modelBuilder.Entity<PeriodEndBalanceQry>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("PeriodEndBalanceQry");

            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CI_AS");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CI_AS");
        });

        modelBuilder.Entity<PeriodParam>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PeriodParam");

            entity.Property(e => e.Period)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PeriodTempBalance>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PeriodTempBalance");

            entity.Property(e => e.AccountID)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0001");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ProfitAndLossHeader>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.All_SUM).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.DirectCost).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.DirectIncome).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.IndirectCost).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.IndirectIncome).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Taxation).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<ProfitAndLossHeaders2>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ProfitAndLossHeaders2");

            entity.Property(e => e.All_SUM).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.DirectCost).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.DirectIncome).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.IndirectCost).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.IndirectIncome).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.InterestPayable).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Taxation).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<RandomID>(entity =>
        {
            entity.HasKey(e => e.RowNumber).HasName("PK_RandomIDs_1");

            entity.Property(e => e.RowNumber).ValueGeneratedNever();
        });

        modelBuilder.Entity<ReportHeader>(entity =>
        {
            entity.HasKey(e => e.GroupID).HasName("PK_ProfitAndLossParam");

            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CatID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Suppres).HasDefaultValue(false, "DF_ReportHeaders_Suppres");
        });

        modelBuilder.Entity<ReportSummary>(entity =>
        {
            entity.HasKey(e => new { e.CoyID, e.Period, e.GroupID }).HasName("PK_BalanceSheet");

            entity.ToTable("ReportSummary");

            entity.Property(e => e.CoyID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(app_name())", "DF_ReportSummary_AppName");
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(host_name())", "DF_ReportSummary_ClientName");
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))", "DF_ReportSummary_EntryDate")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryTime)
                .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))", "DF_ReportSummary_EntryTime")
                .HasColumnType("datetime");
            entity.Property(e => e.ItemName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasComputedColumnSql("(substring([period],(4),(4))+right('00'+CONVERT([varchar](2),substring([Period],(1),(2)),(0)),(2)))", false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.isClose).HasDefaultValue(false, "DF_BalanceSheet_isClose");
            entity.Property(e => e.isLatest).HasDefaultValue(false);
            entity.Property(e => e.isTransfer).HasDefaultValue(false, "DF_BalanceSheet_isTransfer");
        });

        modelBuilder.Entity<StockValuationAcct>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("StockValuationAcct");

            entity.Property(e => e.AmtAvailBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmtOpBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmtPurch).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(app_name())", "DF_StockValuationAcct_AppName");
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(host_name())", "DF_StockValuationAcct_ClientName");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))", "DF_StockValuationAcct_EntryDate")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryTime)
                .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))", "DF_StockValuationAcct_EntryTime")
                .HasColumnType("datetime");
            entity.Property(e => e.ItemName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<StockValuationAcct2>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("StockValuationAcct2");

            entity.Property(e => e.COGS)
                .HasDefaultValue(0m, "DF__StockValua__COGS__75035A77")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.COGS2)
                .HasComputedColumnSql("((isnull([OpenBal],(0))+isnull([StockPurch],(0)))-isnull([CloseBal],(0)))", false)
                .HasColumnType("decimal(20, 2)");
            entity.Property(e => e.CloseBal)
                .HasDefaultValue(0m, "DF__StockValu__Close__7226EDCC")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CloseBal2)
                .HasComputedColumnSql("((isnull([OpenBal],(0))+isnull([StockPurch],(0)))-isnull([COGS],(0)))", false)
                .HasColumnType("decimal(20, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.EntryTime).HasColumnType("datetime");
            entity.Property(e => e.OpenBal)
                .HasDefaultValue(0m, "DF__StockValu__OpenB__703EA55A")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNO).ValueGeneratedOnAdd();
            entity.Property(e => e.StockAdjust)
                .HasDefaultValue(0m, "DF_StockValuationAcct_StockAdjust")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StockPurch)
                .HasDefaultValue(0m, "DF__StockValu__Stock__7132C993")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StockReconcile)
                .HasDefaultValue(0m, "DF__StockValu__Stock__731B1205")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StockSales)
                .HasDefaultValue(0m, "DF__StockValu__Stock__740F363E")
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<TestTranTable1>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TestTranTable1");

            entity.Property(e => e.id).ValueGeneratedOnAdd();
            entity.Property(e => e.some_int).HasDefaultValue(1);
        });

        modelBuilder.Entity<TranCat>(entity =>
        {
            entity.HasKey(e => e.CatID);

            entity.ToTable("TranCat");

            entity.Property(e => e.CatID)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CatName2)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TranFromAppsTrail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TranFromAppsTrail");

            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.TranDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNoApp)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TranxFromApp>(entity =>
        {
            entity.HasKey(e => new { e.TranID, e.AccountID });

            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostCenterID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0001");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0001");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Prd2).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Tranxaction>(entity =>
        {
            entity.HasKey(e => e.SNo).HasName("PK_Tranxaction_1");

            entity.ToTable("Tranxaction");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(app_name())", "DF_Tranxaction_AppName");
            entity.Property(e => e.BillNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(host_name())", "DF_Tranxaction_ClientName");
            entity.Property(e => e.CostCenterID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0001", "DF_Tranxaction_CoyID");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())", "DF_Tranxaction_EntryDate")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryDate2)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))", "DF_Tranxaction_EntryDate1")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryTime)
                .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))", "DF_Tranxaction_EntryTime")
                .HasColumnType("datetime");
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Prd2).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Reversed).HasDefaultValue(false);
            entity.Property(e => e.ReversedPair).HasDefaultValue(0L);
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.hideInRpt).HasDefaultValue(false, "DF__Tranxacti__hideI__49E3F248");
        });

        modelBuilder.Entity<TranxactionArchive>(entity =>
        {
            entity.HasKey(e => e.SNo);

            entity.ToTable("TranxactionArchive");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostCenterID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Prd2).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.isClose).HasDefaultValue(true, "DF__Tranxacti__isClo__47FBA9D6");
        });

        modelBuilder.Entity<TranxactionArchiveMonitor>(entity =>
        {
            entity.HasKey(e => e.BatchNo);

            entity.ToTable("TranxactionArchiveMonitor");

            entity.Property(e => e.BatchNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctToReconcile)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BatchCat)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BatchName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<TranxactionBalance>(entity =>
        {
            entity.HasKey(e => e.SNo);

            entity.ToTable("TranxactionBalance");

            entity.Property(e => e.AccountID)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0001");
            entity.Property(e => e.DateUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TranxactionBalanceBK>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TranxactionBalanceBK");

            entity.Property(e => e.AccountID)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0001");
            entity.Property(e => e.DateUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<TranxactionDeleted>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TranxactionDeleted");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(app_name())", "DF_TranxactionDeleted_AppName");
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(host_name())", "DF_TranxactionDeleted_ClientName");
            entity.Property(e => e.CostCenterID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate2)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))", "DF_TranxactionDeleted_EntryDate2")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryTime)
                .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))", "DF_TranxactionDeleted_EntryTime")
                .HasColumnType("datetime");
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Prd2).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TranxactionJournal>(entity =>
        {
            entity.HasKey(e => e.SNo);

            entity.ToTable("TranxactionJournal");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(app_name())", "DF_TranxactionJournal_AppName");
            entity.Property(e => e.BillNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(host_name())", "DF_TranxactionJournal_ClientName");
            entity.Property(e => e.CostCenterID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0001", "DF_TranxactionJournal_CoyID_1");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())", "DF_TranxactionJournal_EntryDate_1")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryDate2)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))", "DF_TranxactionJournal_EntryDate2")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryTime)
                .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))", "DF_TranxactionJournal_EntryTime")
                .HasColumnType("datetime");
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Prd2).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Reversed).HasDefaultValue(false);
            entity.Property(e => e.ReversedPair).HasDefaultValue(0L);
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.hideInRpt).HasDefaultValue(false, "DF_TranxactionJournal_hideInRpt");
            entity.Property(e => e.isPost).HasDefaultValue(false, "DF_TranxactionJournal_isClose");
        });

        modelBuilder.Entity<TranxactionJournalExternal>(entity =>
        {
            entity.HasKey(e => e.SNo).HasName("PK_Journal_1");

            entity.ToTable("TranxactionJournalExternal");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(app_name())", "DF_Journal_AppName");
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(host_name())", "DF_Journal_ClientName");
            entity.Property(e => e.CostCenterID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0001", "DF_Journal_CoyID");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())", "DF_Journal_EntryDate")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryDate2)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))", "DF_Journal_EntryDate1")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryTime)
                .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))", "DF_Journal_EntryTime")
                .HasColumnType("datetime");
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Prd2).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.hideInRpt).HasDefaultValue(false);
            entity.Property(e => e.isPost).HasDefaultValue(false);
        });

        modelBuilder.Entity<TranxactionJournalTemp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TranxactionJournalTemp");

            entity.Property(e => e.AccountCredit)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountDebit)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(app_name())", "DF_TranxactionJournalTemp_AppName");
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(host_name())", "DF_TranxactionJournalTemp_ClientName");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0001", "DF_TranxactionJournalTemp_CoyID");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())", "DF_TranxactionJournalTemp_EntryDate")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryTime)
                .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))", "DF_TranxactionJournalTemp_EntryTime")
                .HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("j", "DF_TranxactionJournalTemp_TranCat");
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TranxactionPreArchive>(entity =>
        {
            entity.HasKey(e => e.SNo);

            entity.ToTable("TranxactionPreArchive");

            entity.Property(e => e.SNo).ValueGeneratedNever();
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostCenterID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Prd2).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TranxactionPreArchive2>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TranxactionPreArchive2");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostCenterID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Prd2).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TranxactionSuspense>(entity =>
        {
            entity.HasKey(e => e.SNo);

            entity.ToTable("TranxactionSuspense");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(app_name())", "DF_TranxactionSuspense_AppName");
            entity.Property(e => e.BillNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(host_name())", "DF_TranxactionSuspense_ClientName");
            entity.Property(e => e.CostCenterID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("0001", "DF_TranxactionSuspense_CoyID");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())", "DF_TranxactionSuspense_EntryDate")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryDate2)
                .HasDefaultValueSql("(CONVERT([varchar](10),getdate(),(23)))", "DF_TranxactionSuspense_EntryDate2")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryTime)
                .HasDefaultValueSql("(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100)))", "DF_TranxactionSuspense_EntryTime")
                .HasColumnType("datetime");
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Prd2).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Reversed).HasDefaultValue(false);
            entity.Property(e => e.ReversedPair).HasDefaultValue(0L);
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.hideInRpt).HasDefaultValue(false, "DF_TranxactionSuspense_hideInRpt");
            entity.Property(e => e.isPost).HasDefaultValue(false, "DF_TranxactionSuspense_isClose");
        });

        modelBuilder.Entity<TranxactionTemp>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("TranxactionTemp");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostCenterID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Prd2).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TranxactionTemp_OLD>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TranxactionTemp_OLD");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostCenterID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Prd2).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<XXvwStockCloseBal>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("XXvwStockCloseBal");

            entity.Property(e => e.Amount).HasColumnType("decimal(37, 4)");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitsInStock).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<XXvwStockPurchased>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("XXvwStockPurchased");

            entity.Property(e => e.Amount).HasColumnType("decimal(37, 4)");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Qty).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<groupItemsReset>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("groupItemsReset");

            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RptLevel)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.RptTitle)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<qrySysDateTime>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("qrySysDateTime");

            entity.Property(e => e.sysDT).HasColumnType("datetime");
        });

        modelBuilder.Entity<vwAccountInfoPandL>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAccountInfoPandL");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmountAbs).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmountRev).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CatName2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CenterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Credit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Debit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DeptName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.DivName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Expr1)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Prd2).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwAccountMasterInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAccountMasterInfo");

            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BalStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BalStatusMaster)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Expr1)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwAccountMasterInfo2>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAccountMasterInfo2");

            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BalStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BalStatusMaster)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Expr1)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwAccountMonthInFinYear>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAccountMonthInFinYear");

            entity.Property(e => e.AcctMonth).HasColumnType("datetime");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(53)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(52)
                .IsUnicode(false);
            entity.Property(e => e.PeriodYr)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PrdClose).HasColumnType("datetime");
            entity.Property(e => e.PrdType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwAccountMonthOpen>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAccountMonthOpen");

            entity.Property(e => e.AcctMonth).HasColumnType("datetime");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(53)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(52)
                .IsUnicode(false);
            entity.Property(e => e.PeriodYr)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PrdClose).HasColumnType("datetime");
        });

        modelBuilder.Entity<vwAccountsInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAccountsInfo");

            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RptLevel)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.RptTitle)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RptType2)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwAccountsInfoBalSheet>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAccountsInfoBalSheet");

            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwAccountsInfoBalSheet2>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAccountsInfoBalSheet2");

            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwAccountsInfoBalSheet2Temp>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAccountsInfoBalSheet2Temp");

            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwAccountsInfoCombo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAccountsInfoCombo");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwAccountsInfoForRpt>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAccountsInfoForRpt");

            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RptLevel)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.RptTitle)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RptType2)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwAccountsInfoGL>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAccountsInfoGL");

            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RptLevel)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.RptTitle)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RptType2)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwAccountsInfoGLforConfirm>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAccountsInfoGLforConfirm");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwAcctPeriodTypesDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAcctPeriodTypesDetail");

            entity.Property(e => e.PrdType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwAppDefaults_DeprGp>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAppDefaults_DeprGp");

            entity.Property(e => e.ID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IDVal)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwAssetDepreciation>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAssetDepreciation");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountNoAccumDepr)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CalPeriod)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.CanDepr)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeprDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.EntryTime).HasColumnType("datetime");
            entity.Property(e => e.Mth)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Yr)
                .HasMaxLength(4)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwBalanceSheetHeader>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwBalanceSheetHeaders");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.ItemName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MonthYear).HasMaxLength(8);
            entity.Property(e => e.MonthYearLong).HasMaxLength(61);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal1)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal2)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwBalanceSheetHeaders2>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwBalanceSheetHeaders2");

            entity.Property(e => e.AccountsPayable).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.AccountsReceivable).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.AccruedIncomeTax).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.AccumulatedDepreciation).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.BankODAccounts).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Bank_Accounts)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Bank Accounts");
            entity.Property(e => e.CapitalAccount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Cash).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.DutiesAndTradeTaxes).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Equity).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Inventory).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.LandAndBuilding).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Machinery).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.OtherCurrentAssets).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.OtherFixedAssets).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.RetainedEarnings).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.SecuredLoans).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Shares).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.UnsecuredLoans).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Vehicles).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<vwBalanceSheetHeadersByYear>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwBalanceSheetHeadersByYear");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.ItemName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MonthYear).HasMaxLength(4);
            entity.Property(e => e.MonthYearLong).HasMaxLength(61);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal1)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal2)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwBalanceSheetHeadersByYearPL>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwBalanceSheetHeadersByYearPL");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.ItemName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MonthYear).HasMaxLength(4);
            entity.Property(e => e.MonthYearLong).HasMaxLength(61);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal1)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal2)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwBanksAndCash>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwBanksAndCash");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwBanksAndCashAndBBE>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwBanksAndCashAndBBE");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwBranchDept>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwBranchDept");

            entity.Property(e => e.DeptID)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.DeptName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.DivID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.DivName)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwChartOfAccount>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwChartOfAccounts");

            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AcctBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExtID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExtIDType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwChartOfAccountMasterForDelete>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwChartOfAccountMasterForDelete");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<vwChartOfAccountsClosingPeriodsDue>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwChartOfAccountsClosingPeriodsDue");

            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(53)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(52)
                .IsUnicode(false);
            entity.Property(e => e.PrdClose).HasColumnType("datetime");
        });

        modelBuilder.Entity<vwClosedAndOpenPeriod>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwClosedAndOpenPeriods");

            entity.Property(e => e.AcctMonth).HasColumnType("datetime");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(53)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(52)
                .IsUnicode(false);
            entity.Property(e => e.PeriodYr)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PrdClose).HasColumnType("datetime");
        });

        modelBuilder.Entity<vwClosedAndOpenPeriods2>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwClosedAndOpenPeriods2");

            entity.Property(e => e.AcctMonth).HasColumnType("datetime");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(53)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(52)
                .IsUnicode(false);
            entity.Property(e => e.PeriodYr)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PrdClose).HasColumnType("datetime");
        });

        modelBuilder.Entity<vwClosingAndClosedPeriodsUnion>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwClosingAndClosedPeriodsUnion");

            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(53)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(52)
                .IsUnicode(false);
            entity.Property(e => e.PeriodYr)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwCompany>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwCompanies");

            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Coyname)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<vwCompanyAndOpenPeriod>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwCompanyAndOpenPeriods");

            entity.Property(e => e.AcctMonth).HasColumnType("datetime");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Coyname)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(53)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(52)
                .IsUnicode(false);
            entity.Property(e => e.PrdClose).HasColumnType("datetime");
        });

        modelBuilder.Entity<vwConfirmFinRpt>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwConfirmFinRpt");

            entity.Property(e => e.Asset).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.E).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.L).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.LE).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.MustBeZero).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwConfirmFinRpt_COA>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwConfirmFinRpt_COA");

            entity.Property(e => e.Asset).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.E).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.L).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.LE).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.MustBeZero).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwConfirmFinRpt_COA_2_XXXX>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwConfirmFinRpt_COA_2_XXXX");

            entity.Property(e => e.Asset).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.E).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.L).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.LE).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.MustBeZero).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwCostCenter>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwCostCenters");

            entity.Property(e => e.CenterID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.CenterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Coyname)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.DeptID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.DeptName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.DivID)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.DivName)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwDivision>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwDivisions");

            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Coyname)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.DivID)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.DivName)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwGL>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwGL");

            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CostCenterID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Credit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreditDescription)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Debit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DebitDescription)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.EntryTime).HasColumnType("datetime");
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.LedgerCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RptLevel)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.RptTitle)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwGL2>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwGL2");

            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(303)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Credit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Debit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Ledger)
                .HasMaxLength(303)
                .IsUnicode(false);
            entity.Property(e => e.LedgerCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Periodval).HasMaxLength(6);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwGLCOA>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwGLCOA");

            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.LedgerCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Periodval)
                .HasMaxLength(6)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwGLforRpt>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwGLforRpt");

            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(303)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Credit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Debit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.DrCr).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Ledger)
                .HasMaxLength(303)
                .IsUnicode(false);
            entity.Property(e => e.LedgerBalance).HasColumnType("decimal(19, 2)");
            entity.Property(e => e.LedgerClBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LedgerCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.LedgerCredit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LedgerDebit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LedgerOpBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Periodval)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwGLforRptGrouped>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwGLforRptGrouped");

            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Credit).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Debit).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.DrCr).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Ledger)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LedgerBalance).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.LedgerClBal).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.LedgerCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.LedgerCodeVal)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.LedgerCredit).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.LedgerDebit).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.LedgerOpBal).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Periodval)
                .HasMaxLength(6)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwGLforRptPL>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwGLforRptPL");

            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(303)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Credit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Debit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Ledger)
                .HasMaxLength(303)
                .IsUnicode(false);
            entity.Property(e => e.LedgerCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Periodval).HasMaxLength(6);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwGLforRptPLGrouped>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwGLforRptPLGrouped");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LedgerBalance).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.LedgerClBal).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.LedgerCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.LedgerOpBal).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Periodval)
                .HasMaxLength(6)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwGLforRpt_SelfJoin>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwGLforRpt-SelfJoin");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Credit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreditDescription)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Debit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DebitDescription)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Ledger)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LedgerCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<vwGroupCatForFixedAsset>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwGroupCatForFixedAssets");

            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<vwGroupItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwGroupItems");

            entity.Property(e => e.CanDepr)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Editable)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Expr1)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RptLevel)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.RptTitle)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwGroupItemsFixedAsset>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwGroupItemsFixedAssets");

            entity.Property(e => e.GroupID)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<vwGroupItemsForBalSheet>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwGroupItemsForBalSheet");

            entity.Property(e => e.GroupID)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwGroupItemsForBalSheet2>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwGroupItemsForBalSheet2");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwGroupItemsNoSuppress>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwGroupItemsNoSuppress");

            entity.Property(e => e.GroupID)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<vwGroupItemsWithoutDepr>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwGroupItemsWithoutDepr");

            entity.Property(e => e.GroupID)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwGroupItemsWithoutFixedAssetsOrDepr>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwGroupItemsWithoutFixedAssetsOrDepr");

            entity.Property(e => e.GroupID)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<vwLocationsIP>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwLocationsIP");

            entity.Property(e => e.LocIP)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LocName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<vwPeriodUnionForRpt>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwPeriodUnionForRpt");

            entity.Property(e => e.Period)
                .HasMaxLength(53)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwProfitAndLossDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwProfitAndLossDetails");

            entity.Property(e => e.CoyID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.DirectCost).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.DirectIncome).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.GrossProfit).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.IndirectCost).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.IndirectIncome).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.NetOprProfit).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.NetProfit).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.ProfitBeforeTax).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Taxation).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<vwProfitAndLossHeader>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwProfitAndLossHeaders");

            entity.Property(e => e.Amount).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.ItemName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MonthYear).HasMaxLength(8);
            entity.Property(e => e.MonthYearLong).HasMaxLength(61);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal1)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal2)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwProfitAndLossHeadersByYear>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwProfitAndLossHeadersByYear");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.ItemName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MonthYear).HasMaxLength(4);
            entity.Property(e => e.MonthYearLong).HasMaxLength(61);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal1)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal2)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwProfitAndLossHeadersList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwProfitAndLossHeadersList");

            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ItemName)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwProfitOrLoss>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwProfitOrLoss");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwProfitOrLoss2>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwProfitOrLoss2");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwProfitOrLossClosePrd>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwProfitOrLossClosePrd");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwReportHeader>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwReportHeaders");

            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RptLevel)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.RptTitle)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwReportSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwReportSummary");

            entity.Property(e => e.Amount).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ItemName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MonthYear).HasMaxLength(8);
            entity.Property(e => e.MonthYearLong).HasMaxLength(61);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal1)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal2)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwReportSummary2>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwReportSummary2");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ItemName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RptLevel)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwReportSummaryOriginal>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwReportSummaryOriginal");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ItemName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MonthYear).HasMaxLength(8);
            entity.Property(e => e.MonthYearLong).HasMaxLength(61);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal1)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal2)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwShowMaxPeriodInReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwShowMaxPeriodInReport");

            entity.Property(e => e.ID)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwStockEntryForValuationAcct>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwStockEntryForValuationAcct");

            entity.Property(e => e.AmtPurch).HasColumnType("decimal(37, 4)");
            entity.Property(e => e.AvailBal).HasColumnType("decimal(38, 4)");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.ItemName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LocID).HasMaxLength(50);
            entity.Property(e => e.OPBal).HasColumnType("decimal(37, 4)");
            entity.Property(e => e.Period)
                .HasMaxLength(33)
                .IsUnicode(false);
            entity.Property(e => e.PrevQty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Qty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<vwStockEntryForValuationAcctPharmacy>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwStockEntryForValuationAcctPharmacy");

            entity.Property(e => e.AmtPurch).HasColumnType("decimal(37, 4)");
            entity.Property(e => e.AvailBal).HasColumnType("decimal(38, 4)");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Expr1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ItemName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LocID).HasMaxLength(50);
            entity.Property(e => e.OPBal).HasColumnType("decimal(37, 4)");
            entity.Property(e => e.PrevQty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Qty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<vwStockEntryForValuationAcctStore>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwStockEntryForValuationAcctStore");

            entity.Property(e => e.AmtPurch).HasColumnType("decimal(37, 4)");
            entity.Property(e => e.AvailBal).HasColumnType("decimal(38, 4)");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.ItemName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LocID).HasMaxLength(50);
            entity.Property(e => e.OPBal).HasColumnType("decimal(37, 4)");
            entity.Property(e => e.Period)
                .HasMaxLength(33)
                .IsUnicode(false);
            entity.Property(e => e.PrevQty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Qty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<vwStockSalesAndCOG>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwStockSalesAndCOGS");

            entity.Property(e => e.COGS).HasColumnType("decimal(37, 4)");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.ItemName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(33)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Qty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Sales).HasColumnType("decimal(37, 4)");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(37, 4)");
        });

        modelBuilder.Entity<vwStockSalesAndCOGSGrouped>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwStockSalesAndCOGSGrouped");

            entity.Property(e => e.COGS).HasColumnType("decimal(38, 4)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(33)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Profit).HasColumnType("decimal(38, 4)");
            entity.Property(e => e.Sales).HasColumnType("decimal(38, 4)");
        });

        modelBuilder.Entity<vwStockValuationAcct>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwStockValuationAcct");

            entity.Property(e => e.AmtAvailBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmtOpBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmtPurch).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ItemName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(6)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwStockValuationAcctGrouped>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwStockValuationAcctGrouped");

            entity.Property(e => e.AmtAvailBal).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.AmtOpBal).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.AmtPurch).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(6)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwStockValuationAcctGroupedSalesAndCOG>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwStockValuationAcctGroupedSalesAndCOGS");

            entity.Property(e => e.AmtAvailBal).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.AmtClBal).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.AmtOpBal).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.AmtPurch).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.COGS).HasColumnType("decimal(38, 4)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Profit).HasColumnType("decimal(38, 4)");
            entity.Property(e => e.StockSales).HasColumnType("decimal(38, 4)");
        });

        modelBuilder.Entity<vwTotalAsset>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTotalAsset");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTotalCurrentAsset>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTotalCurrentAsset");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTotalDirectExpense>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTotalDirectExpenses");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTotalDirectIncome>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTotalDirectIncome");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTotalEquity>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTotalEquity");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTotalExpense>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTotalExpenses");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTotalFixedAsset>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTotalFixedAsset");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTotalInDirectExpense>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTotalInDirectExpenses");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTotalInDirectIncome>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTotalInDirectIncome");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTotalIncome>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTotalIncome");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTotalLiability>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTotalLiability");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTotalLiabilityAndEquity>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTotalLiabilityAndEquity");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTotalTax>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTotalTax");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTranx>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranx");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BillNo)
                .HasMaxLength(52)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CatName2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CenterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeptName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.DivName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate2).HasColumnType("datetime");
            entity.Property(e => e.EntryTime).HasColumnType("datetime");
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Prd2).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTranxArchive>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxArchive");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostCenterID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Prd2).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTranxDebitCreditGroupedByAccountID>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxDebitCreditGroupedByAccountID");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Credit).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Debit).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Period).HasMaxLength(50);
        });

        modelBuilder.Entity<vwTranxDebitCreditGroupedByPeriod>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxDebitCreditGroupedByPeriod");

            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Credit).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Debit).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Period).HasMaxLength(50);
        });

        modelBuilder.Entity<vwTranxForGrid>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxForGrid");

            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BillNo)
                .HasMaxLength(52)
                .IsUnicode(false);
            entity.Property(e => e.CostCenter)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Credit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Debit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TranCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTranxForGridTemp>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxForGridTemp");

            entity.Property(e => e.AccountCredit)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountDebit)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountNameCredit)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNameDebit)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Credit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Debit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTranxGroupedByAccountID>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxGroupedByAccountID");

            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Balance).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Credit).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Debit).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<vwTranxGroupedByAccountIDCrossTab>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxGroupedByAccountIDCrossTab");

            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Balance).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Credit).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Debit).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.TB_Credit).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.TB_Debit).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<vwTranxJournalTemp>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxJournalTemp");

            entity.Property(e => e.AccountCredit)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountDebit)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountNameCredit)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNameDebit)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CatName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CatName2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.EntryTime).HasColumnType("datetime");
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTranxJournalTempWithNoDummyAcctNo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxJournalTempWithNoDummyAcctNo");

            entity.Property(e => e.AccountCredit)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountDebit)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.EntryTime).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTranxNo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxNo");

            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTranxNoOLD>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxNoOLD");

            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTranxWithPeriodVal>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxWithPeriodVal");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BillNo)
                .HasMaxLength(52)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CatName2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CenterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeptName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.DivName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate2).HasColumnType("datetime");
            entity.Property(e => e.EntryTime).HasColumnType("datetime");
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(52)
                .IsUnicode(false);
            entity.Property(e => e.Prd2).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTranxaction>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxaction");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AppName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CostCenterID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate2).HasColumnType("datetime");
            entity.Property(e => e.EntryTime).HasColumnType("datetime");
            entity.Property(e => e.GroupID)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTranxaction2>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxaction2");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CatName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CatName2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CenterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CostCenterID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeptName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.DivName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTranxactionAndChartOfAccount>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxactionAndChartOfAccounts");

            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Periodval).HasMaxLength(6);
            entity.Property(e => e.RptLevel)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.RptTitle)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranxAmount).HasColumnType("decimal(19, 2)");
        });

        modelBuilder.Entity<vwTranxactionAndChartOfAccountsForRptSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxactionAndChartOfAccountsForRptSummary");

            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Periodval).HasMaxLength(6);
            entity.Property(e => e.RptLevel)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.RptTitle)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranxAmount).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<vwTranxactionAndChartOfAccountsGrouped>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxactionAndChartOfAccountsGrouped");

            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Periodval).HasMaxLength(6);
            entity.Property(e => e.RptLevel)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.RptTitle)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranxAmount).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<vwTranxactionAndChartOfAccountsGroupedTesting>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxactionAndChartOfAccountsGroupedTesting");

            entity.Property(e => e.CatID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterID)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatMasterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RptLevel)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.RptTitle)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RptType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranxAmount).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<vwTranxactionArchiveForGrid>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxactionArchiveForGrid");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AcctBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AcctToReconcile)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BatchCat)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BatchName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BatchNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTranxactionArchiveMonitor>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxactionArchiveMonitor");

            entity.Property(e => e.AcctToReconcile)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BatchCat)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BatchName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BatchNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SNo).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<vwTranxactionGrouped>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTranxactionGrouped");

            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Period).HasMaxLength(50);
        });

        modelBuilder.Entity<vwTrialBalance>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTrialBalance");

            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ClosingBalance).HasColumnType("decimal(19, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTrialBalanceByPeriod>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTrialBalanceByPeriod");

            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Expr1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<vwTrialBalanceGL>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTrialBalanceGL");

            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountDesc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ClosingBalance).HasColumnType("decimal(19, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwTrialBalanceGroup>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTrialBalanceGroup");

            entity.Property(e => e.AccountCat)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountClAmt).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.AccountOpAmt).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwcheckPeriodExist>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwcheckPeriodExists");

            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PeriodVal)
                .HasMaxLength(52)
                .IsUnicode(false);
            entity.Property(e => e.PeriodYr)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.TranDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<vwtranxJournal>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwtranxJournal");

            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AcctBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CatID)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.CatName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CatName2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CenterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CostCenterID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeptName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.DivName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Prd2).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TranCat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.TranID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwtranxJournalExpress>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwtranxJournalExpress");

            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CenterName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CoyID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeptName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.DivName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Period).HasMaxLength(50);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
