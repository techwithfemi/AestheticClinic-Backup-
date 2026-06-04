-- =====================================================
-- Quick Debt Audit Scripts
-- Purpose: Fast checks for debt-related issues
-- =====================================================

-- =====================================================
-- AUDIT 1: Check Patients with Mismatched Debt Values
-- =====================================================
-- HPatient.DebtBf should match the last invoice's balance for private patients

PRINT '═══════════════════════════════════════════════════════════';
PRINT 'AUDIT 1: Patients with Mismatched Debt Values';
PRINT '═══════════════════════════════════════════════════════════';

WITH LastInvoiceBalance AS
(
    SELECT
        b.pNo,
        b.billNO AS LastBillNo,
        b.bDate,
        CAST(
            (ISNULL(b.AmountBilled, 0) - ISNULL(b.Discount, 0)) 
            + ISNULL(b.DebtBF, 0)
            + ISNULL(b.Tax, 0)
            - ISNULL(b.AmountPaid, 0)
            AS DECIMAL(18,2)
        ) AS CalculatedBalance,
        ROW_NUMBER() OVER (PARTITION BY b.pNo ORDER BY b.bDate DESC, b.billNO DESC, b.ID DESC) AS RN
    FROM Billings b
    INNER JOIN HPatients hp ON b.pNo = hp.Pno
    INNER JOIN HRetainerships hr ON hp.CoyName = hr.RetainCode
    WHERE hr.RetainCode = '0001'  -- Private patients only
)
SELECT
    hp.Pno,
    hp.PSurName + ' ' + ISNULL(hp.PFirstname, '') AS PatientName,
    lib.LastBillNo,
    lib.bDate AS LastBillDate,
    CAST(hp.DebtBf AS DECIMAL(18,2)) AS DebtBf_InHPatient,
    lib.CalculatedBalance AS DebtBf_ShouldBe,
    CASE 
        WHEN CAST(hp.DebtBf AS DECIMAL(18,2)) = lib.CalculatedBalance THEN '✓ OK'
        ELSE '✗ MISMATCH'
    END AS Status,
    CAST(lib.CalculatedBalance - hp.DebtBf AS DECIMAL(18,2)) AS Difference
FROM LastInvoiceBalance lib
INNER JOIN HPatients hp ON lib.pNo = hp.Pno
WHERE lib.RN = 1 AND CAST(hp.DebtBf AS DECIMAL(18,2)) != lib.CalculatedBalance
ORDER BY lib.CalculatedBalance DESC;

-- =====================================================
-- AUDIT 2: Check for Corporate Patients with Non-Zero Debt
-- =====================================================
-- Corporate/HMO patients should never have debt carry-forward

PRINT '';
PRINT '═══════════════════════════════════════════════════════════';
PRINT 'AUDIT 2: Corporate/HMO Patients with Non-Zero Debt';
PRINT '═══════════════════════════════════════════════════════════';

SELECT
    hp.Pno,
    hp.PSurName + ' ' + ISNULL(hp.PFirstname, '') AS PatientName,
    hr.RetainCode,
    CASE WHEN hr.RetainCode != '0001' THEN 'CORPORATE/HMO' ELSE 'PRIVATE' END AS PatientType,
    CAST(hp.DebtBf AS DECIMAL(18,2)) AS DebtBf,
    '✗ ERROR - Should be 0.00' AS Status
FROM HPatients hp
INNER JOIN HRetainerships hr ON hp.CoyName = hr.RetainCode
WHERE hr.RetainCode != '0001' AND hp.DebtBf > 0
ORDER BY hp.DebtBf DESC;

-- =====================================================
-- AUDIT 3: Check for Orphaned CoyName References
-- =====================================================
-- Patients with CoyName that don't exist in HRetainerships

PRINT '';
PRINT '═══════════════════════════════════════════════════════════';
PRINT 'AUDIT 3: Patients with Invalid Retainership References';
PRINT '═══════════════════════════════════════════════════════════';

SELECT
    hp.Pno,
    hp.PSurName + ' ' + ISNULL(hp.PFirstname, '') AS PatientName,
    hp.CoyName,
    CAST(hp.DebtBf AS DECIMAL(18,2)) AS DebtBf,
    '✗ ERROR - RetainCode not found' AS Status
FROM HPatients hp
WHERE hp.CoyName IS NOT NULL 
    AND NOT EXISTS (
        SELECT 1 FROM HRetainerships hr 
        WHERE hr.RetainCode = hp.CoyName
    )
ORDER BY hp.Pno;

-- =====================================================
-- AUDIT 4: Check First Invoice DebtBF Should Be 0
-- =====================================================

PRINT '';
PRINT '═══════════════════════════════════════════════════════════';
PRINT 'AUDIT 4: First Invoices with Non-Zero DebtBF';
PRINT '═══════════════════════════════════════════════════════════';

WITH FirstInvoice AS
(
    SELECT
        b.pNo,
        b.billNO,
        b.bDate,
        b.DebtBF,
        ROW_NUMBER() OVER (PARTITION BY b.pNo ORDER BY b.bDate ASC, b.billNO ASC, b.ID ASC) AS InvoiceSeq
    FROM Billings b
    INNER JOIN HPatients hp ON b.pNo = hp.Pno
    INNER JOIN HRetainerships hr ON hp.CoyName = hr.RetainCode
    WHERE hr.RetainCode = '0001'  -- Private patients
)
SELECT
    hp.Pno,
    hp.PSurName + ' ' + ISNULL(hp.PFirstname, '') AS PatientName,
    fi.billNO,
    fi.bDate,
    CAST(fi.DebtBF AS DECIMAL(18,2)) AS DebtBF,
    '✗ ERROR - Should be 0.00' AS Status
FROM FirstInvoice fi
INNER JOIN HPatients hp ON fi.pNo = hp.Pno
WHERE fi.InvoiceSeq = 1 AND fi.DebtBF != 0
ORDER BY fi.pNo;

-- =====================================================
-- AUDIT 5: Recent Changes to Patient Debt
-- =====================================================
-- Shows patients whose debt has changed in last 7 days

PRINT '';
PRINT '═══════════════════════════════════════════════════════════';
PRINT 'AUDIT 5: Recent Billing Activity (Last 7 Days)';
PRINT '═══════════════════════════════════════════════════════════';

SELECT
    hp.Pno,
    hp.PSurName + ' ' + ISNULL(hp.PFirstname, '') AS PatientName,
    CASE WHEN hr.RetainCode = '0001' THEN 'PRIVATE' ELSE 'OTHER' END AS Type,
    COUNT(b.ID) AS RecentInvoices,
    MAX(b.bDate) AS LatestInvoiceDate,
    CAST(hp.DebtBf AS DECIMAL(18,2)) AS CurrentDebt
FROM HPatients hp
INNER JOIN Billings b ON hp.Pno = b.pNo
LEFT JOIN HRetainerships hr ON hp.CoyName = hr.RetainCode
WHERE b.bDate >= DATEADD(DAY, -7, CAST(GETDATE() AS DATE))
GROUP BY hp.Pno, hp.PSurName, hp.PFirstname, hp.DebtBf, hr.RetainCode
ORDER BY MAX(b.bDate) DESC;

-- =====================================================
-- AUDIT 6: Summary Statistics
-- =====================================================

PRINT '';
PRINT '═══════════════════════════════════════════════════════════';
PRINT 'AUDIT 6: Summary Statistics';
PRINT '═══════════════════════════════════════════════════════════';
PRINT '';

-- Total patients
SELECT COUNT(DISTINCT Pno) AS TotalPatients FROM HPatients;

-- Private patients
SELECT COUNT(DISTINCT hp.Pno) AS PrivatePatients
FROM HPatients hp
INNER JOIN HRetainerships hr ON hp.CoyName = hr.RetainCode
WHERE hr.RetainCode = '0001';

-- Patients with outstanding debt
SELECT COUNT(DISTINCT hp.Pno) AS PatientsWithDebt
FROM HPatients hp
WHERE hp.DebtBf > 0;

-- Total outstanding debt
SELECT CAST(SUM(hp.DebtBf) AS DECIMAL(18,2)) AS TotalOutstandingDebt
FROM HPatients hp;

-- Invoices created today
SELECT COUNT(*) AS InvoicesCreatedToday
FROM Billings
WHERE bDate = CAST(GETDATE() AS DATE);

-- Total invoices
SELECT COUNT(*) AS TotalInvoices FROM Billings;

PRINT '';
PRINT '═══════════════════════════════════════════════════════════';
PRINT 'Audit Complete';
PRINT '═══════════════════════════════════════════════════════════';
