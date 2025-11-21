# Registration Status Update Implementation

## Overview
The attendance column has been removed from the Event Attendees list, and the system now automatically updates the registration status in the database based on attendance conditions.

## Changes Made

### 1. **Removed Attendance Column**
- The separate "Attendance" column has been removed from the DataGridView
- The attendance information is now reflected directly in the **Status** column

### 2. **Dynamic Status Updates**

The registration status is now automatically updated based on attendance records:

| Condition | Old Behavior | New Behavior |
|-----------|-------------|--------------|
| **Check-in + Check-out** | Status: "Approved", Attendance: "Attended" | Status: **"Attended"** |
| **Check-in only** | Status: "Approved", Attendance: "Checked-in" | Status: **"Checked-in"** |
| **No attendance + Event ended** | Status: "Approved", Attendance: "Didn't Attend" | Status: **"Didn't Attend"** |
| **No attendance + Event ongoing** | Status: "Approved", Attendance: "N/A" | Status: **"Approved"** |
| **Pending/Rejected** | Status: "Pending/Rejected", Attendance: "N/A" | Status: **"Pending/Rejected"** |

## Database Schema Impact

### Registration Status Values
The `registrations.status` column now stores the following values:

```sql
-- Before check-in
'Pending'        -- Awaiting admin approval
'Approved'       -- Approved but not yet checked in
'Rejected'       -- Registration rejected

-- After check-in
'Checked-in'     -- User has checked in but not checked out

-- After check-out or event ended
'Attended'       -- User completed attendance (checked in + out)
'Didn't Attend'  -- Event ended, user was approved but didn't attend
```

**Note:** The database schema doesn't require modification. The `status` column already accepts VARCHAR values, so these new statuses work without schema changes.

## Code Changes

### A. frmEventAttendees.cs

#### 1. **CustomizeDataGridView() - Removed Attendance Column**
```csharp
// REMOVED:
// dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn
// {
//     Name = "attendance_status",
//     HeaderText = "Attendance",
//     ReadOnly = true,
//     FillWeight = 12
// });

// Column fill weights adjusted to compensate for removed column
```

#### 2. **LoadAttendees() - Status Logic**
```csharp
// Determine the new status based on check_in_time and check_out_time
string newStatus;
bool hasCheckIn = dr["check_in_time"] != DBNull.Value;
bool hasCheckOut = dr["check_out_time"] != DBNull.Value;
DateTime eventEndDateTime = Convert.ToDateTime(dr["end_datetime"]);
bool eventHasEnded = DateTime.Now > eventEndDateTime;

if (hasCheckIn && hasCheckOut)
{
    // Both check-in and check-out recorded
    newStatus = "Attended";
}
else if (hasCheckIn && !hasCheckOut)
{
    // Only check-in recorded
    newStatus = "Checked-in";
}
else if (!hasCheckIn && !hasCheckOut)
{
    // No attendance record
    if (eventHasEnded && currentStatus == "Approved")
    {
        // Event has ended and no attendance
        newStatus = "Didn't Attend";
    }
    else
    {
        // Keep current status (Pending, Approved, Rejected)
        newStatus = currentStatus;
    }
}
else
{
    // This shouldn't happen (check-out without check-in), but keep current status
    newStatus = currentStatus;
}

// Update the status in the database if it has changed
if (newStatus != currentStatus)
{
    UpdateRegistrationStatus(registrationId, newStatus);
}
```

#### 3. **New Method: UpdateRegistrationStatus()**
```csharp
private void UpdateRegistrationStatus(int registrationId, string newStatus)
{
    try
    {
        string updateQuery = "UPDATE registrations SET status = @status WHERE id = @id";
        MySqlParameter[] parameters = {
            new MySqlParameter("@status", newStatus),
            new MySqlParameter("@id", registrationId)
        };
        DatabaseHelper.ExecuteNonQuery(updateQuery, parameters);
    }
    catch (Exception ex)
    {
        // Log error silently, don't interrupt the UI
        System.Diagnostics.Debug.WriteLine($"Error updating status for registration {registrationId}: {ex.Message}");
    }
}
```

### B. frmAttendanceScanner.cs

#### 1. **RecordAttendance() - Check-in Status Update**
```csharp
// After successful check-in
if (rowsAffected > 0)
{
    // Update registration status to "Checked-in"
    UpdateRegistrationStatus(regId, "Checked-in");
    
    lblStatus.Text = "? Status: Check-in recorded successfully!";
    // ...
}
```

#### 2. **RecordCheckOut() - Attended Status Update**
```csharp
private void RecordCheckOut(int attendanceId, int registrationId, string userName, string eventName)
{
    // ...
    if (rowsAffected > 0)
    {
        // Update registration status to "Attended"
        UpdateRegistrationStatus(registrationId, "Attended");
        
        lblStatus.Text = "? Status: Check-out recorded successfully!";
        // ...
    }
}
```

#### 3. **Enhanced Validation**
```csharp
// Now accepts both "Approved" and "Checked-in" statuses
if (status != "Approved" && status != "Checked-in")
{
    // Show error message
    return;
}
```

#### 4. **New Method: UpdateRegistrationStatus()**
```csharp
private void UpdateRegistrationStatus(int registrationId, string newStatus)
{
    try
    {
        string updateQuery = "UPDATE registrations SET status = @status WHERE id = @id";
        MySqlParameter[] parameters = {
            new MySqlParameter("@status", newStatus),
            new MySqlParameter("@id", registrationId)
        };
        DatabaseHelper.ExecuteNonQuery(updateQuery, parameters);
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"Error updating registration status: {ex.Message}");
    }
}
```

## Status Flow Diagram

```
???????????????????????????????????????????????????????????????
?                    REGISTRATION LIFECYCLE                    ?
???????????????????????????????????????????????????????????????

  User Registers
       ?
       ?
  ????????????
  ? Pending  ? ????????????????
  ????????????                ?
       ?                      ?
       ? (Admin Approves)     ? (Admin can change)
       ?                      ?
  ????????????                ?
  ? Approved ? ????????????????
  ????????????                ?
       ?                      ?
       ? (User Scans QR)      ?
       ?                      ?
  ??????????????              ?
  ? Checked-in ? ??????????????
  ??????????????              ?
       ?                      ?
       ? (User Scans QR Again)?
       ?                      ?
  ????????????                ?
  ? Attended ?                ?
  ????????????                ?
                              ?
  ??????????????????          ?
  ? Didn't Attend  ? ??????????
  ??????????????????
  (Event ended, no check-in)
  
  ????????????
  ? Rejected ? (Admin rejects)
  ????????????
```

## Benefits

### 1. **Simplified UI**
- One less column to display
- Cleaner, more intuitive interface
- All information in a single Status column

### 2. **Real-time Status Tracking**
- Registration status always reflects current attendance state
- No need to check multiple columns
- Status persists in database, not just calculated on display

### 3. **Better Reporting**
- Can query database directly for attendance statistics
- Status values are standardized
- Easier to generate reports

### 4. **Data Integrity**
- Status is automatically updated when attendance changes
- No manual intervention required
- Consistent across all views

## SQL Queries for Reporting

### Count Attendees by Status
```sql
SELECT 
    status,
    COUNT(*) as count
FROM registrations
WHERE event_id = 1
GROUP BY status;
```

### Get Attendance Rate
```sql
SELECT 
    COUNT(CASE WHEN status = 'Attended' THEN 1 END) as attended,
    COUNT(CASE WHEN status = 'Approved' THEN 1 END) as approved,
    COUNT(CASE WHEN status = 'Checked-in' THEN 1 END) as checked_in,
    COUNT(CASE WHEN status = 'Didn''t Attend' THEN 1 END) as no_show,
    COUNT(*) as total
FROM registrations
WHERE event_id = 1;
```

### Attendance Percentage
```sql
SELECT 
    ROUND(
        COUNT(CASE WHEN status = 'Attended' THEN 1 END) * 100.0 / 
        COUNT(CASE WHEN status IN ('Approved', 'Checked-in', 'Attended', 'Didn''t Attend') THEN 1 END),
        2
    ) as attendance_percentage
FROM registrations
WHERE event_id = 1;
```

## Testing Checklist

- [ ] **Pending Registration**: Status shows "Pending"
- [ ] **Approved Registration**: Status shows "Approved"
- [ ] **After Check-in**: Status changes to "Checked-in"
- [ ] **After Check-out**: Status changes to "Attended"
- [ ] **Event Ended + No Check-in**: Status changes to "Didn't Attend"
- [ ] **Event Ongoing + No Check-in**: Status remains "Approved"
- [ ] **Rejected Registration**: Status shows "Rejected"
- [ ] **Database Updates**: Verify status persists after reload
- [ ] **Scanner Validation**: Can check-in users with "Approved" or "Checked-in" status
- [ ] **UI Display**: Attendees list shows correct status without attendance column

## Migration Notes

### For Existing Data
If you have existing registrations in your database, you may want to run this update script to sync the status with attendance records:

```sql
-- Update registrations based on existing attendance records

-- Set to "Attended" if both check-in and check-out exist
UPDATE registrations r
INNER JOIN attendance a ON a.registration_id = r.id
SET r.status = 'Attended'
WHERE a.check_in_time IS NOT NULL 
  AND a.check_out_time IS NOT NULL
  AND r.status = 'Approved';

-- Set to "Checked-in" if only check-in exists
UPDATE registrations r
INNER JOIN attendance a ON a.registration_id = r.id
SET r.status = 'Checked-in'
WHERE a.check_in_time IS NOT NULL 
  AND a.check_out_time IS NULL
  AND r.status = 'Approved';

-- Set to "Didn't Attend" for approved registrations with no attendance after event ended
UPDATE registrations r
INNER JOIN events e ON e.id = r.event_id
LEFT JOIN attendance a ON a.registration_id = r.id
SET r.status = 'Didn''t Attend'
WHERE r.status = 'Approved'
  AND a.id IS NULL
  AND NOW() > e.end_datetime;
```

## Summary

? **Attendance column removed** from the UI
? **Status column** now shows dynamic attendance-based statuses
? **Database automatically updated** when attendance changes
? **Real-time status tracking** via QR scanner
? **Cleaner UI** with fewer columns
? **Better data integrity** with persisted status values
? **Easier reporting** with standardized status values

The implementation provides a more streamlined approach to tracking attendance by consolidating the information into a single status field that automatically reflects the current state of each registration.
