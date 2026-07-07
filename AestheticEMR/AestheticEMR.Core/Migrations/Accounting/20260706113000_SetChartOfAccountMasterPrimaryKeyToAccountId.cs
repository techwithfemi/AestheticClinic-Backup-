using AestheticEMR.Core.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AestheticEMR.Core.Migrations.Accounting;

[DbContext(typeof(AccountingDbContext))]
[Migration("20260706113000_SetChartOfAccountMasterPrimaryKeyToAccountId")]
public partial class SetChartOfAccountMasterPrimaryKeyToAccountId : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
IF OBJECT_ID(N'dbo.ChartOfAccountMaster', N'U') IS NOT NULL
BEGIN
    DECLARE @tableId int = OBJECT_ID(N'dbo.ChartOfAccountMaster', N'U');
    DECLARE @currentPk sysname;

    SELECT @currentPk = kc.name
    FROM sys.key_constraints kc
    WHERE kc.parent_object_id = @tableId
      AND kc.type = 'PK';

    DECLARE @isAccountIdPk bit = 0;

    IF @currentPk IS NOT NULL
       AND EXISTS
       (
           SELECT 1
           FROM sys.indexes i
           JOIN sys.index_columns ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id
           JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
           WHERE i.object_id = @tableId
             AND i.name = @currentPk
           GROUP BY i.object_id, i.index_id
           HAVING COUNT(*) = 1
              AND MAX(CASE WHEN c.name = 'AccountID' THEN 1 ELSE 0 END) = 1
       )
    BEGIN
        SET @isAccountIdPk = 1;
    END

    IF @isAccountIdPk = 0
    BEGIN
        IF @currentPk IS NOT NULL
        BEGIN
            EXEC(N'ALTER TABLE dbo.ChartOfAccountMaster DROP CONSTRAINT [' + @currentPk + N']');
        END

        IF EXISTS
        (
            SELECT 1
            FROM dbo.ChartOfAccountMaster
            WHERE AccountID IS NULL OR LTRIM(RTRIM(AccountID)) = ''
        )
        BEGIN
            THROW 50001, 'Cannot set PK on ChartOfAccountMaster.AccountID because null/empty AccountID values exist.', 1;
        END

        IF EXISTS
        (
            SELECT AccountID
            FROM dbo.ChartOfAccountMaster
            GROUP BY AccountID
            HAVING COUNT(*) > 1
        )
        BEGIN
            THROW 50002, 'Cannot set PK on ChartOfAccountMaster.AccountID because duplicate AccountID values exist.', 1;
        END

        ALTER TABLE dbo.ChartOfAccountMaster
        ADD CONSTRAINT PK_ChartOfAccountMaster_AccountID PRIMARY KEY CLUSTERED (AccountID);
    END
END
");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
IF OBJECT_ID(N'dbo.ChartOfAccountMaster', N'U') IS NOT NULL
BEGIN
    IF EXISTS
    (
        SELECT 1
        FROM sys.key_constraints kc
        WHERE kc.parent_object_id = OBJECT_ID(N'dbo.ChartOfAccountMaster', N'U')
          AND kc.type = 'PK'
          AND kc.name = N'PK_ChartOfAccountMaster_AccountID'
    )
    BEGIN
        ALTER TABLE dbo.ChartOfAccountMaster DROP CONSTRAINT PK_ChartOfAccountMaster_AccountID;
    END
END
");
    }
}

