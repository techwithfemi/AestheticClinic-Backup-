using AestheticEMR.Core.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AestheticEMR.Core.Migrations.Accounting;

[DbContext(typeof(AccountingDbContext))]
[Migration("20260705131500_SetJournalIdentityPrimaryKeys")]
public partial class SetJournalIdentityPrimaryKeys : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
IF OBJECT_ID(N'dbo.TranxactionJournalTemp', N'U') IS NOT NULL
BEGIN
    DECLARE @TempNeedsRebuild bit = 0;

    IF COLUMNPROPERTY(OBJECT_ID(N'dbo.TranxactionJournalTemp'), N'SNo', 'IsIdentity') <> 1
        SET @TempNeedsRebuild = 1;

    IF NOT EXISTS (
        SELECT 1
        FROM sys.key_constraints kc
        WHERE kc.parent_object_id = OBJECT_ID(N'dbo.TranxactionJournalTemp')
          AND kc.type = 'PK')
        SET @TempNeedsRebuild = 1;

    IF @TempNeedsRebuild = 1
    BEGIN
        IF OBJECT_ID(N'dbo.TranxactionJournalTemp_Mig', N'U') IS NOT NULL
            DROP TABLE dbo.TranxactionJournalTemp_Mig;

        CREATE TABLE dbo.TranxactionJournalTemp_Mig
        (
            SNo BIGINT IDENTITY(1,1) NOT NULL,
            TranDate DATETIME NOT NULL,
            TranID VARCHAR(50) NULL,
            AccountDebit VARCHAR(50) NOT NULL,
            AccountCredit VARCHAR(50) NOT NULL,
            CoyID VARCHAR(50) NOT NULL CONSTRAINT DF_TranxactionJournalTemp_Mig_CoyID DEFAULT('0001'),
            Amount DECIMAL(18,2) NOT NULL,
            Description VARCHAR(250) NULL,
            TranCat VARCHAR(1) NOT NULL CONSTRAINT DF_TranxactionJournalTemp_Mig_TranCat DEFAULT('j'),
            IsPost BIT NOT NULL,
            Remarks VARCHAR(250) NULL,
            UserName VARCHAR(50) NOT NULL,
            EntryDate DATETIME NOT NULL CONSTRAINT DF_TranxactionJournalTemp_Mig_EntryDate DEFAULT(getdate()),
            EntryTime DATETIME NOT NULL CONSTRAINT DF_TranxactionJournalTemp_Mig_EntryTime DEFAULT(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100))),
            AppName VARCHAR(500) NOT NULL CONSTRAINT DF_TranxactionJournalTemp_Mig_AppName DEFAULT(app_name()),
            ClientName VARCHAR(500) NOT NULL CONSTRAINT DF_TranxactionJournalTemp_Mig_ClientName DEFAULT(host_name()),
            CONSTRAINT PK_TranxactionJournalTemp PRIMARY KEY CLUSTERED (SNo)
        );

        SET IDENTITY_INSERT dbo.TranxactionJournalTemp_Mig ON;

        INSERT INTO dbo.TranxactionJournalTemp_Mig
        (
            SNo, TranDate, TranID, AccountDebit, AccountCredit, CoyID, Amount,
            Description, TranCat, IsPost, Remarks, UserName, EntryDate, EntryTime, AppName, ClientName
        )
        SELECT
            SNo, TranDate, TranID, AccountDebit, AccountCredit, CoyID, Amount,
            Description, TranCat, IsPost, Remarks, UserName, EntryDate, EntryTime, AppName, ClientName
        FROM dbo.TranxactionJournalTemp WITH (HOLDLOCK, TABLOCKX);

        SET IDENTITY_INSERT dbo.TranxactionJournalTemp_Mig OFF;

        DROP TABLE dbo.TranxactionJournalTemp;
        EXEC sp_rename N'dbo.TranxactionJournalTemp_Mig', N'TranxactionJournalTemp';
    END
END
");

        migrationBuilder.Sql(@"
IF OBJECT_ID(N'dbo.TranxactionJournal', N'U') IS NOT NULL
BEGIN
    DECLARE @JournalNeedsRebuild bit = 0;

    IF COLUMNPROPERTY(OBJECT_ID(N'dbo.TranxactionJournal'), N'SNo', 'IsIdentity') <> 1
        SET @JournalNeedsRebuild = 1;

    IF NOT EXISTS (
        SELECT 1
        FROM sys.key_constraints kc
        WHERE kc.parent_object_id = OBJECT_ID(N'dbo.TranxactionJournal')
          AND kc.type = 'PK')
        SET @JournalNeedsRebuild = 1;

    IF @JournalNeedsRebuild = 1
    BEGIN
        IF OBJECT_ID(N'dbo.TranxactionJournal_Mig', N'U') IS NOT NULL
            DROP TABLE dbo.TranxactionJournal_Mig;

        CREATE TABLE dbo.TranxactionJournal_Mig
        (
            SNo BIGINT IDENTITY(1,1) NOT NULL,
            TranID VARCHAR(50) NOT NULL,
            AccountID VARCHAR(50) NOT NULL,
            TranNo VARCHAR(50) NOT NULL,
            TranDate DATETIME NOT NULL,
            CostCenterID VARCHAR(50) NOT NULL,
            Amount DECIMAL(18,2) NOT NULL,
            Description VARCHAR(250) NULL,
            TranCat VARCHAR(1) NOT NULL,
            EntryDate DATETIME NOT NULL CONSTRAINT DF_TranxactionJournal_Mig_EntryDate DEFAULT(getdate()),
            Period VARCHAR(50) NOT NULL,
            Prd2 DATETIME NULL,
            UserName VARCHAR(50) NOT NULL,
            Remarks VARCHAR(250) NULL,
            CoyID VARCHAR(50) NULL CONSTRAINT DF_TranxactionJournal_Mig_CoyID DEFAULT('0001'),
            AcctBal DECIMAL(18,2) NULL,
            isPost BIT NULL CONSTRAINT DF_TranxactionJournal_Mig_isPost DEFAULT(0),
            hideInRpt BIT NULL CONSTRAINT DF_TranxactionJournal_Mig_hideInRpt DEFAULT(0),
            EntryDate2 DATETIME NOT NULL CONSTRAINT DF_TranxactionJournal_Mig_EntryDate2 DEFAULT(CONVERT([varchar](10),getdate(),(23))),
            EntryTime DATETIME NOT NULL CONSTRAINT DF_TranxactionJournal_Mig_EntryTime DEFAULT(CONVERT([varchar](15),CONVERT([time],getdate(),(0)),(100))),
            AppName VARCHAR(500) NOT NULL CONSTRAINT DF_TranxactionJournal_Mig_AppName DEFAULT(app_name()),
            ClientName VARCHAR(500) NOT NULL CONSTRAINT DF_TranxactionJournal_Mig_ClientName DEFAULT(host_name()),
            BillNo VARCHAR(50) NULL,
            SNoID BIGINT NULL,
            Reversed BIT NULL CONSTRAINT DF_TranxactionJournal_Mig_Reversed DEFAULT(0),
            ReversedPair BIGINT NULL CONSTRAINT DF_TranxactionJournal_Mig_ReversedPair DEFAULT(0),
            CONSTRAINT PK_TranxactionJournal PRIMARY KEY CLUSTERED (SNo)
        );

        SET IDENTITY_INSERT dbo.TranxactionJournal_Mig ON;

        INSERT INTO dbo.TranxactionJournal_Mig
        (
            SNo, TranID, AccountID, TranNo, TranDate, CostCenterID, Amount, Description,
            TranCat, EntryDate, Period, Prd2, UserName, Remarks, CoyID, AcctBal,
            isPost, hideInRpt, EntryDate2, EntryTime, AppName, ClientName,
            BillNo, SNoID, Reversed, ReversedPair
        )
        SELECT
            SNo, TranID, AccountID, TranNo, TranDate, CostCenterID, Amount, Description,
            TranCat, EntryDate, Period, Prd2, UserName, Remarks, CoyID, AcctBal,
            isPost, hideInRpt, EntryDate2, EntryTime, AppName, ClientName,
            BillNo, SNoID, Reversed, ReversedPair
        FROM dbo.TranxactionJournal WITH (HOLDLOCK, TABLOCKX);

        SET IDENTITY_INSERT dbo.TranxactionJournal_Mig OFF;

        DROP TABLE dbo.TranxactionJournal;
        EXEC sp_rename N'dbo.TranxactionJournal_Mig', N'TranxactionJournal';
    END
END
");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Intentionally left empty. Reverting identity/primary key rebuilds is destructive and manual.
    }
}
