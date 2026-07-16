-- ============================================================================
-- ROSTER DELETE-INSERT TEST SCRIPT
-- ============================================================================
-- Purpose: Verify delete-insert behavior when re-saving a processed roster group
-- 
-- Instructions:
-- 1. Run the "BEFORE TEST" section to see current state
-- 2. In the UI: Click "Add Roster" → Select same group → Save with different selections
-- 3. Run the "AFTER TEST" section to verify delete-insert occurred
-- ============================================================================

-- ============================================================================
-- SECTION 1: BEFORE TEST - Capture Current State
-- ============================================================================

-- Q1: Count total roster records in the database
SELECT 'BEFORE TEST - Total Records' AS [Check],
       COUNT(*) AS [TotalCount]
FROM Roster;

-- Q2: Get details of records for the LAST group that was saved (most recent by date)
-- This will help us identify which group to test
DECLARE @LastGroup INT;
SELECT @LastGroup = GroupID
FROM Roster
ORDER BY RosterDate DESC
LIMIT 1;

SELECT 'BEFORE TEST - Last Saved Group Details' AS [Check],
       GroupID,
       COUNT(*) AS [RecordCount],
       MIN(RosterDate) AS [FirstDate],
       MAX(RosterDate) AS [LastDate],
       COUNT(DISTINCT EmpID) AS [EmployeeCount],
       COUNT(DISTINCT RosterDate) AS [DaysCount]
FROM Roster
WHERE GroupID = @LastGroup
GROUP BY GroupID;

-- Q3: Show all records for the test group (so you can compare later)
SELECT 'BEFORE TEST - Records for Test Group' AS [Check],
       SNo,
       RosterDate,
       GroupID,
       EmpID,
       ShiftID,
       ShiftName,
       ShiftAbbrv,
       isOffDuty,
       CreatedDate
FROM Roster
WHERE GroupID = @LastGroup
ORDER BY RosterDate, ShiftName;

-- Q4: Get the month range for this group (so you know the data you're about to delete)
SELECT 'BEFORE TEST - Month Range' AS [Check],
       @LastGroup AS [GroupID],
       FORMAT(MIN(RosterDate), 'yyyy-MM') AS [MonthRange],
       COUNT(*) AS [TotalRecordsInMonth]
FROM Roster
WHERE GroupID = @LastGroup
  AND YEAR(RosterDate) = YEAR(MAX(RosterDate))
  AND MONTH(RosterDate) = MONTH(MAX(RosterDate));

-- ============================================================================
-- SECTION 2: MAKE YOUR TEST CHANGES IN THE UI
-- ============================================================================
-- 
-- Step 1: Go to Staff Roster → Create Roster
-- Step 2: Click "Add Roster" (NEW MODE)
-- Step 3: Select the SAME group from the query above (the @LastGroup value)
-- Step 4: Select the SAME month/year range
-- Step 5: Make DIFFERENT checkbox selections than before
--         Example: If before you had only morning shifts, now select evening shifts
-- Step 6: Click SAVE
-- Step 7: Wait for success message
-- Step 8: Return here and run SECTION 3
--
-- ============================================================================

-- ============================================================================
-- SECTION 3: AFTER TEST - Verify Delete-Insert Occurred
-- ============================================================================

-- Q5: Count total roster records after save
SELECT 'AFTER TEST - Total Records' AS [Check],
       COUNT(*) AS [TotalCount]
FROM Roster;

-- Q6: Check the test group again
DECLARE @TestGroup INT;
SELECT @TestGroup = GroupID
FROM Roster
ORDER BY RosterDate DESC
LIMIT 1;

SELECT 'AFTER TEST - Test Group Details' AS [Check],
       GroupID,
       COUNT(*) AS [RecordCount],
       MIN(RosterDate) AS [FirstDate],
       MAX(RosterDate) AS [LastDate],
       COUNT(DISTINCT EmpID) AS [EmployeeCount],
       COUNT(DISTINCT RosterDate) AS [DaysCount]
FROM Roster
WHERE GroupID = @TestGroup
GROUP BY GroupID;

-- Q7: Show all NEW records for the test group
SELECT 'AFTER TEST - NEW Records for Test Group' AS [Check],
       SNo,
       RosterDate,
       GroupID,
       EmpID,
       ShiftID,
       ShiftName,
       ShiftAbbrv,
       isOffDuty,
       CreatedDate
FROM Roster
WHERE GroupID = @TestGroup
ORDER BY RosterDate, ShiftName;

-- Q8: Verify the CreatedDate is RECENT (shows records were just inserted)
SELECT 'AFTER TEST - Record Freshness' AS [Check],
       MIN(CreatedDate) AS [OldestRecordCreated],
       MAX(CreatedDate) AS [NewestRecordCreated],
       DATEDIFF(MINUTE, MIN(CreatedDate), MAX(CreatedDate)) AS [SpreadMinutes]
FROM Roster
WHERE GroupID = @TestGroup;

-- ============================================================================
-- SECTION 4: DELETE-INSERT VERIFICATION LOGIC
-- ============================================================================
-- 
-- If DELETE-INSERT is working correctly, you should see:
--
-- ✅ BEFORE TEST → AFTER TEST:
--    - Total record count may stay the same or change
--      (depends on how many selections you made)
--    - ALL old SNo values are GONE (deleted)
--    - ALL new SNo values are HIGHER (newly inserted with auto-increment)
--    - CreatedDate on new records is VERY RECENT (matches save time)
--    - ShiftName/ShiftAbbrv values are DIFFERENT from before
--      (because you selected different shifts)
--    - Same GroupID (proving it's the same group)
--    - Same RosterDate range (same month)
--
-- ❌ If DELETE-INSERT is NOT working:
--    - Old SNo values still exist
--    - Old ShiftName values still present
--    - Duplicate records for the same date/employee
--    - CreatedDate on old records is old
--
-- ============================================================================

-- Q9: Show side-by-side comparison (if you captured SNo before)
-- Example: If old records had SNo 1-20 and new records have SNo 21-40,
-- it proves the delete happened and new inserts used auto-increment
SELECT 'AFTER TEST - SNo Range Analysis' AS [Check],
       MIN(SNo) AS [MinSNo],
       MAX(SNo) AS [MaxSNo],
       COUNT(*) AS [RecordCount],
       CAST(MAX(SNo) - MIN(SNo) AS FLOAT) / COUNT(*) AS [AvgGapBetweenRecords]
FROM Roster
WHERE GroupID = @TestGroup;

-- ============================================================================
-- BONUS: Real-time Transaction Log (if you want to monitor during save)
-- ============================================================================
-- Run this DURING the save to see if DELETE is happening
/*
SELECT 
    dt.database_id,
    dt.database_name,
    dt.transaction_id,
    dt.transaction_begin_time,
    es.login_name,
    es.host_name,
    dt.transaction_type,
    dt.transaction_state
FROM sys.dm_tran_database_transactions AS dt
INNER JOIN sys.dm_exec_sessions AS es 
    ON dt.session_id = es.session_id
WHERE dt.database_name = 'YourDatabaseName'
ORDER BY dt.transaction_begin_time DESC;
*/

