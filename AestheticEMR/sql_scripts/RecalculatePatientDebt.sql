-- =====================================================
-- Script: Recalculate Patient Debt Carry-Forward
-- Purpose: Recalculates and corrects DebtBF for all invoices
--          of a given patient in chronological order
-- Author: Development Team
-- Usage: EXEC sp_RecalculatePatientDebt @pNo = 'PATIENT_NUMBER'
-- =====================================================

-- Create the procedure
IF OBJECT_ID('sp_RecalculatePatientDebt', 'P') IS NOT NULL
    DROP PROCEDURE sp_RecalculatePatientDebt;
GO

CREATE PROCEDURE sp_RecalculatePatientDebt
    @pNo NVARCHAR(50),
    @dryRun BIT = 1  -- 1 = Show what will change, 0 = Apply changes
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @patientExists BIT = 0;
    DECLARE @isPrivate BIT = 0;
    DECLARE @currentDebt DECIMAL(18,2) = 0;
    DECLARE @billCount INT = 0;
    DECLARE @lastDebtBF DECIMAL(18,2) = 0;

    -- =====================================================
    -- Step 1: Validate Patient Exists
    -- =====================================================
    IF NOT EXISTS (SELECT 1 FROM HPatients WHERE Pno = @pNo)
    BEGIN
        RAISERROR('Patient %s not found in HPatients table.', 16, 1, @pNo);
        RETURN;
    END

    SELECT @patientExists = 1;
    PRINT '✓ Patient ' + @pNo + ' found in HPatients table';

    -- =====================================================
    -- Step 2: Check if Patient is Private
    -- =====================================================
    DECLARE @coyName NVARCHAR(50);
    SELECT @coyName = CoyName FROM HPatients WHERE Pno = @pNo;

    IF EXISTS (SELECT 1 FROM HRetainerships 
               WHERE RetainCode = @coyName AND RetainCode = '0001')
    BEGIN
        SET @isPrivate = 1;
        PRINT '✓ Patient is PRIVATE (RetainCode = 0001) - Debt carry-forward will be applied';
    END
    ELSE
    BEGIN
        PRINT '⚠ Patient is NOT private (RetainCode ≠ 0001) - Debt carry-forward will NOT be applied';
        SET @isPrivate = 0;
    END

    -- =====================================================
    -- Step 3: Create Temp Table with Current Debt Calculations
    -- =====================================================
    CREATE TABLE #DebtRecalculation
    (
        RowNum INT IDENTITY(1,1),
        BillingID BIGINT,
        BillNo NVARCHAR(50),
        BDate DATE,
        PNo NVARCHAR(50),
        CurrentDebtBF DECIMAL(18,2),
        AmountBilled DECIMAL(18,2),
        Discount DECIMAL(18,2),
        Tax DECIMAL(18,2),
        AmountPaid DECIMAL(18,2),
        CalculatedBalance DECIMAL(18,2),
        NewDebtBF DECIMAL(18,2),
        IsCorrect BIT
    );

    -- =====================================================
    -- Step 4: Populate Temp Table with Chronological Data
    -- =====================================================
    INSERT INTO #DebtRecalculation 
    (BillingID, BillNo, BDate, PNo, CurrentDebtBF, AmountBilled, Discount, Tax, AmountPaid, NewDebtBF, IsCorrect)
    SELECT
        b.ID,
        b.billNO,
        b.bDate,
        b.pNo,
        b.DebtBF,
        ISNULL(b.AmountBilled, 0),
        ISNULL(b.Discount, 0),
        CAST(ISNULL(b.Tax, 0) AS DECIMAL(18,2)),
        ISNULL(b.AmountPaid, 0),
        0,  -- Will be calculated below
        0   -- Will be determined below
    FROM Billings b
    WHERE b.pNo = @pNo
    ORDER BY b.bDate ASC, b.billNO ASC, b.ID ASC;

    SET @billCount = @@ROWCOUNT;

    IF @billCount = 0
    BEGIN
        PRINT '⚠ No billing records found for patient ' + @pNo;
        DROP TABLE #DebtRecalculation;
        RETURN;
    END

    PRINT CHAR(13) + '📊 Found ' + CAST(@billCount AS NVARCHAR(10)) + ' billing record(s) for patient ' + @pNo + CHAR(13);

    -- =====================================================
    -- Step 5: Calculate Correct DebtBF for Each Invoice
    -- =====================================================
    DECLARE @rowNum INT = 1;
    DECLARE @totalRows INT;

    SELECT @totalRows = MAX(RowNum) FROM #DebtRecalculation;

    WHILE @rowNum <= @totalRows
    BEGIN
        DECLARE @balance DECIMAL(18,2);
        DECLARE @newDebtBF DECIMAL(18,2);

        -- For first record: DebtBF should be 0 (unless it's private with existing debt)
        IF @rowNum = 1
        BEGIN
            SET @newDebtBF = 0;  -- First invoice starts with 0 debt brought forward
        END
        ELSE
        BEGIN
            -- For subsequent records: Use the balance from previous invoice as DebtBF
            SELECT @newDebtBF = CalculatedBalance
            FROM #DebtRecalculation
            WHERE RowNum = @rowNum - 1;
        END

        -- Calculate balance: ((AmountBilled - Discount) + DebtBF + Tax) - AmountPaid
        SELECT @balance = 
            (((AmountBilled - Discount) + @newDebtBF + Tax) - AmountPaid)
        FROM #DebtRecalculation
        WHERE RowNum = @rowNum;

        -- Update temp table
        UPDATE #DebtRecalculation
        SET 
            NewDebtBF = @newDebtBF,
            CalculatedBalance = @balance,
            IsCorrect = CASE WHEN @isPrivate = 1 AND CurrentDebtBF = @newDebtBF THEN 1
                             WHEN @isPrivate = 0 AND @newDebtBF = 0 THEN 1
                             ELSE 0 END
        WHERE RowNum = @rowNum;

        SET @rowNum = @rowNum + 1;
    END

    -- =====================================================
    -- Step 6: Display Summary of Changes
    -- =====================================================
    PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
    PRINT 'DEBT RECALCULATION SUMMARY';
    PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';

    SELECT
        BillNo,
        BDate,
        AmountBilled,
        Discount,
        Tax,
        AmountPaid,
        CurrentDebtBF AS 'Current DebtBF',
        NewDebtBF AS 'Correct DebtBF',
        CASE WHEN CurrentDebtBF = NewDebtBF THEN '✓ CORRECT' ELSE '✗ INCORRECT' END AS Status,
        CalculatedBalance AS 'Balance Due'
    FROM #DebtRecalculation
    ORDER BY BDate ASC, BillNo ASC;

    -- Count incorrect records
    DECLARE @incorrectCount INT;
    SELECT @incorrectCount = COUNT(*) FROM #DebtRecalculation WHERE IsCorrect = 0;

    PRINT CHAR(13) + '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
    PRINT 'RESULT: ' + CAST(@incorrectCount AS NVARCHAR(10)) + ' record(s) need correction';
    PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';

    -- =====================================================
    -- Step 7: Apply Changes if Not Dry Run
    -- =====================================================
    IF @dryRun = 0 AND @incorrectCount > 0
    BEGIN
        PRINT CHAR(13) + '🔄 Applying corrections...';

        BEGIN TRANSACTION;
        TRY
            -- Update Billing table with corrected DebtBF
            UPDATE b
            SET b.DebtBF = dr.NewDebtBF
            FROM Billings b
            INNER JOIN #DebtRecalculation dr ON b.ID = dr.BillingID
            WHERE b.pNo = @pNo AND dr.IsCorrect = 0;

            -- Get the last calculated debt for the patient
            SELECT @lastDebtBF = CalculatedBalance
            FROM #DebtRecalculation
            WHERE RowNum = @totalRows;

            -- Update HPatient with last debt amount
            UPDATE HPatients
            SET DebtBf = @lastDebtBF,
                Debt = @lastDebtBF,
                IsRev = 1
            WHERE Pno = @pNo;

            COMMIT TRANSACTION;
            PRINT '✓ Successfully updated ' + CAST(@incorrectCount AS NVARCHAR(10)) + ' billing record(s)';
            PRINT '✓ Updated HPatient.DebtBf = ' + FORMAT(@lastDebtBF, '0.00');
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            PRINT '✗ ERROR: ' + ERROR_MESSAGE();
        END CATCH
    END
    ELSE IF @dryRun = 1
    BEGIN
        PRINT CHAR(13) + '📋 DRY RUN MODE - No changes applied';
        PRINT '   Run with @dryRun = 0 to apply these corrections';
    END

    -- =====================================================
    -- Cleanup
    -- =====================================================
    DROP TABLE #DebtRecalculation;

    PRINT CHAR(13) + '✓ Script completed successfully';
END
GO

-- =====================================================
-- USAGE EXAMPLES:
-- =====================================================

-- Example 1: Check debt for patient P001 (DRY RUN - Safe to run)
-- EXEC sp_RecalculatePatientDebt @pNo = 'P001', @dryRun = 1;

-- Example 2: Apply corrections for patient P001
-- EXEC sp_RecalculatePatientDebt @pNo = 'P001', @dryRun = 0;

-- Example 3: Check debt for another patient (DRY RUN)
-- EXEC sp_RecalculatePatientDebt @pNo = 'P002', @dryRun = 1;
