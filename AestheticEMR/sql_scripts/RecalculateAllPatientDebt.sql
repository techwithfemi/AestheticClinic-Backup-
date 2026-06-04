-- =====================================================
-- Script: Batch Recalculate Debt for Multiple Patients
-- Purpose: Identifies and fixes debt history for ALL private patients
--          with potential debt carry-forward issues
-- Usage: EXEC sp_RecalculateAllPatientDebt @applyChanges = 0
-- =====================================================

IF OBJECT_ID('sp_RecalculateAllPatientDebt', 'P') IS NOT NULL
    DROP PROCEDURE sp_RecalculateAllPatientDebt;
GO

CREATE PROCEDURE sp_RecalculateAllPatientDebt
    @applyChanges BIT = 0  -- 0 = Report only, 1 = Apply corrections
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @totalPatientsChecked INT = 0;
    DECLARE @patientsWithIssues INT = 0;
    DECLARE @recordsCorrected INT = 0;

    -- =====================================================
    -- Step 1: Identify All Private Patients with Billings
    -- =====================================================
    PRINT '🔍 Scanning for private patients with debt history issues...';
    PRINT CHAR(13);

    CREATE TABLE #PatientDebIssues
    (
        Pno NVARCHAR(50),
        PatientName NVARCHAR(200),
        RetainCode NVARCHAR(50),
        BillingCount INT,
        IncorrectDebtBFCount INT,
        LastDebt DECIMAL(18,2)
    );

    -- =====================================================
    -- Step 2: Find Patients with Incorrect DebtBF
    -- =====================================================
    INSERT INTO #PatientDebIssues
    WITH CTE_PatientBillings AS
    (
        SELECT
            b.pNo,
            hp.PSurName,
            hp.PFirstname,
            hr.RetainCode,
            b.ID,
            b.BillNo,
            b.bDate,
            b.DebtBF,
            ROW_NUMBER() OVER (PARTITION BY b.pNo ORDER BY b.bDate ASC, b.billNO ASC, b.ID ASC) AS BillSeq,
            LAG(
                CAST(ISNULL(b.AmountBilled, 0) AS DECIMAL(18,2)) 
                - CAST(ISNULL(b.Discount, 0) AS DECIMAL(18,2))
                + CAST(ISNULL(b.Tax, 0) AS DECIMAL(18,2))
                - CAST(ISNULL(b.AmountPaid, 0) AS DECIMAL(18,2))
            ) OVER (PARTITION BY b.pNo ORDER BY b.bDate ASC, b.billNO ASC, b.ID ASC) AS PrevBalance
        FROM Billings b
        INNER JOIN HPatients hp ON b.pNo = hp.Pno
        LEFT JOIN HRetainerships hr ON hp.CoyName = hr.RetainCode
        WHERE hr.RetainCode = '0001'  -- Only private patients
    ),
    CTE_IncorrectRecords AS
    (
        SELECT
            pNo,
            COUNT(*) AS IncorrectCount
        FROM CTE_PatientBillings
        WHERE BillSeq > 1 AND DebtBF != ISNULL(PrevBalance, 0)
        GROUP BY pNo
    )
    SELECT
        pb.pNo,
        hp.PSurName + ' ' + ISNULL(hp.PFirstname, '') AS PatientName,
        hr.RetainCode,
        COUNT(DISTINCT pb.ID) AS BillingCount,
        ISNULL(ir.IncorrectCount, 0) AS IncorrectDebtBFCount,
        (SELECT TOP 1 
            CAST(ISNULL(b.AmountBilled, 0) AS DECIMAL(18,2))
            - CAST(ISNULL(b.Discount, 0) AS DECIMAL(18,2))
            + CAST(ISNULL(b.Tax, 0) AS DECIMAL(18,2))
            - CAST(ISNULL(b.AmountPaid, 0) AS DECIMAL(18,2))
         FROM Billings b
         WHERE b.pNo = pb.pNo
         ORDER BY b.bDate DESC, b.billNO DESC, b.ID DESC) AS LastDebt
    FROM CTE_PatientBillings pb
    INNER JOIN HPatients hp ON pb.pNo = hp.Pno
    INNER JOIN HRetainerships hr ON pb.RetainCode = hr.RetainCode
    LEFT JOIN CTE_IncorrectRecords ir ON pb.pNo = ir.pNo
    WHERE hr.RetainCode = '0001'
    GROUP BY pb.pNo, hp.PSurName, hp.PFirstname, hr.RetainCode, ir.IncorrectCount;

    SET @totalPatientsChecked = @@ROWCOUNT;

    -- =====================================================
    -- Step 3: Display Report
    -- =====================================================
    PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
    PRINT 'DEBT VERIFICATION REPORT - PRIVATE PATIENTS';
    PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
    PRINT '';

    SELECT
        Pno,
        PatientName,
        BillingCount,
        IncorrectDebtBFCount,
        LastDebt,
        CASE WHEN IncorrectDebtBFCount > 0 THEN '⚠ NEEDS FIX' ELSE '✓ OK' END AS Status
    FROM #PatientDebIssues
    ORDER BY IncorrectDebtBFCount DESC, Pno ASC;

    SELECT @patientsWithIssues = COUNT(*) FROM #PatientDebIssues WHERE IncorrectDebtBFCount > 0;

    PRINT '';
    PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
    PRINT 'SUMMARY:';
    PRINT '  • Patients with billings: ' + CAST(@totalPatientsChecked AS NVARCHAR(10));
    PRINT '  • Patients with debt issues: ' + CAST(@patientsWithIssues AS NVARCHAR(10));
    PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';

    -- =====================================================
    -- Step 4: Apply Corrections if Requested
    -- =====================================================
    IF @applyChanges = 1 AND @patientsWithIssues > 0
    BEGIN
        PRINT CHAR(13) + '🔄 Applying corrections to ' + CAST(@patientsWithIssues AS NVARCHAR(10)) + ' patient(s)...';

        DECLARE @pNo NVARCHAR(50);
        DECLARE patientCursor CURSOR FOR
            SELECT Pno FROM #PatientDebIssues WHERE IncorrectDebtBFCount > 0;

        OPEN patientCursor;
        FETCH NEXT FROM patientCursor INTO @pNo;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Execute individual patient debt recalculation
            EXEC sp_RecalculatePatientDebt @pNo = @pNo, @dryRun = 0;
            FETCH NEXT FROM patientCursor INTO @pNo;
        END

        CLOSE patientCursor;
        DEALLOCATE patientCursor;

        PRINT CHAR(13) + '✓ Batch corrections completed';
    END
    ELSE IF @applyChanges = 0 AND @patientsWithIssues > 0
    BEGIN
        PRINT CHAR(13) + '📋 REPORT ONLY MODE';
        PRINT '   To apply corrections, run: EXEC sp_RecalculateAllPatientDebt @applyChanges = 1;';
    END

    DROP TABLE #PatientDebIssues;

    PRINT CHAR(13) + '✓ Script completed successfully';
END
GO

-- =====================================================
-- USAGE EXAMPLES:
-- =====================================================

-- Example 1: Generate report of all patients with debt issues (SAFE - No changes)
-- EXEC sp_RecalculateAllPatientDebt @applyChanges = 0;

-- Example 2: Apply corrections to all private patients with debt issues
-- EXEC sp_RecalculateAllPatientDebt @applyChanges = 1;
