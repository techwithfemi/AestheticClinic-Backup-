-- =====================================================
-- Script: Validate Debt Carry-Forward Logic
-- Purpose: Quick validation of debt calculation for any patient
-- =====================================================

-- =====================================================
-- Query 1: Check a Specific Patient's Debt History
-- =====================================================
-- Replace 'P001' with the patient number to check
DECLARE @pNo NVARCHAR(50) = 'P001';

SELECT 
    '=== PATIENT INFO ===' AS Section,
    hp.Pno,
    hp.PSurName + ' ' + ISNULL(hp.PFirstname, '') AS PatientName,
    hp.CoyName,
    CASE WHEN hr.RetainCode = '0001' THEN 'PRIVATE' ELSE 'CORPORATE/HMO' END AS PatientType,
    hp.DebtBf AS CurrentDebtBf,
    hp.Debt AS CurrentDebt
FROM HPatients hp
LEFT JOIN HRetainerships hr ON hp.CoyName = hr.RetainCode
WHERE hp.Pno = @pNo;

PRINT '';
SELECT 
    '=== BILLING HISTORY ===' AS Section,
    b.billNO,
    b.bDate,
    CAST(ISNULL(b.AmountBilled, 0) AS DECIMAL(18,2)) AS Billed,
    CAST(ISNULL(b.Discount, 0) AS DECIMAL(18,2)) AS Discount,
    CAST(ISNULL(b.Tax, 0) AS DECIMAL(18,2)) AS Tax,
    CAST(ISNULL(b.AmountPaid, 0) AS DECIMAL(18,2)) AS Paid,
    CAST(ISNULL(b.DebtBF, 0) AS DECIMAL(18,2)) AS DebtBF,
    CAST(
        (ISNULL(b.AmountBilled, 0) - ISNULL(b.Discount, 0)) 
        + ISNULL(b.DebtBF, 0)
        + ISNULL(b.Tax, 0)
        - ISNULL(b.AmountPaid, 0)
        AS DECIMAL(18,2)
    ) AS Balance
FROM Billings b
WHERE b.pNo = @pNo
ORDER BY b.bDate ASC, b.billNO ASC, b.ID ASC;

-- =====================================================
-- Query 2: Find All Private Patients with Debt
-- =====================================================
PRINT '';
PRINT '=== PRIVATE PATIENTS WITH OUTSTANDING DEBT ===';
SELECT 
    hp.Pno,
    hp.PSurName + ' ' + ISNULL(hp.PFirstname, '') AS PatientName,
    COUNT(b.ID) AS InvoiceCount,
    MAX(b.bDate) AS LastInvoiceDate,
    CAST(hp.DebtBf AS DECIMAL(18,2)) AS OutstandingDebt
FROM HPatients hp
INNER JOIN HRetainerships hr ON hp.CoyName = hr.RetainCode
LEFT JOIN Billings b ON hp.Pno = b.pNo
WHERE hr.RetainCode = '0001'
    AND hp.DebtBf > 0
GROUP BY hp.Pno, hp.PSurName, hp.PFirstname, hp.DebtBf
ORDER BY hp.DebtBf DESC;

-- =====================================================
-- Query 3: Check Debt Carry-Forward Errors
-- =====================================================
PRINT '';
PRINT '=== POTENTIAL DEBT CARRY-FORWARD ERRORS ===';
WITH BillingSequence AS
(
    SELECT
        b.ID,
        b.billNO,
        b.pNo,
        b.bDate,
        b.DebtBF AS CurrentDebtBF,
        ROW_NUMBER() OVER (PARTITION BY b.pNo ORDER BY b.bDate ASC, b.billNO ASC, b.ID ASC) AS SeqNum,
        LAG(
            CAST(ISNULL(b.AmountBilled, 0) AS DECIMAL(18,2))
            - CAST(ISNULL(b.Discount, 0) AS DECIMAL(18,2))
            + CAST(ISNULL(b.Tax, 0) AS DECIMAL(18,2))
            - CAST(ISNULL(b.AmountPaid, 0) AS DECIMAL(18,2))
        ) OVER (PARTITION BY b.pNo ORDER BY b.bDate ASC, b.billNO ASC, b.ID ASC) AS ExpectedDebtBF
    FROM Billings b
    INNER JOIN HPatients hp ON b.pNo = hp.Pno
    INNER JOIN HRetainerships hr ON hp.CoyName = hr.RetainCode
    WHERE hr.RetainCode = '0001'  -- Private patients only
)
SELECT
    pNo,
    billNO,
    bDate,
    SeqNum,
    CAST(ISNULL(CurrentDebtBF, 0) AS DECIMAL(18,2)) AS CurrentDebtBF,
    CAST(ISNULL(ExpectedDebtBF, 0) AS DECIMAL(18,2)) AS ExpectedDebtBF,
    CASE 
        WHEN SeqNum = 1 AND CurrentDebtBF = 0 THEN '✓ OK (First invoice)'
        WHEN SeqNum > 1 AND CurrentDebtBF = ISNULL(ExpectedDebtBF, 0) THEN '✓ OK'
        ELSE '✗ ERROR - Needs correction'
    END AS Status
FROM BillingSequence
WHERE SeqNum > 1 AND CurrentDebtBF != ISNULL(ExpectedDebtBF, 0)
ORDER BY pNo, bDate ASC, billNO ASC;

-- =====================================================
-- Query 4: Summary Statistics
-- =====================================================
PRINT '';
PRINT '=== SUMMARY STATISTICS ===';
SELECT
    'Total Private Patients' AS Metric,
    COUNT(DISTINCT hp.Pno) AS Value
FROM HPatients hp
INNER JOIN HRetainerships hr ON hp.CoyName = hr.RetainCode
WHERE hr.RetainCode = '0001'

UNION ALL

SELECT
    'Private Patients with Billings',
    COUNT(DISTINCT b.pNo)
FROM Billings b
INNER JOIN HPatients hp ON b.pNo = hp.Pno
INNER JOIN HRetainerships hr ON hp.CoyName = hr.RetainCode
WHERE hr.RetainCode = '0001'

UNION ALL

SELECT
    'Total Billing Records',
    COUNT(*)
FROM Billings b
INNER JOIN HPatients hp ON b.pNo = hp.Pno
INNER JOIN HRetainerships hr ON hp.CoyName = hr.RetainCode
WHERE hr.RetainCode = '0001'

UNION ALL

SELECT
    'Patients with Outstanding Debt',
    COUNT(DISTINCT hp.Pno)
FROM HPatients hp
WHERE hp.DebtBf > 0 AND EXISTS
    (SELECT 1 FROM HRetainerships hr 
     WHERE hr.RetainCode = hp.CoyName AND hr.RetainCode = '0001');
