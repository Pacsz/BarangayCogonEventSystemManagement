# QR Code Attendance Scanner - Implementation Guide

## Overview
The QR code attendance scanner has been fully implemented with proper validation and check-in/check-out functionality.

## Database Schema Reference
```sql
-- ATTENDANCE TABLE
CREATE TABLE attendance (
  id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
  registration_id INT UNSIGNED NOT NULL,
  check_in_time DATETIME DEFAULT CURRENT_TIMESTAMP,
  check_out_time DATETIME NULL,

  CONSTRAINT fk_att_reg FOREIGN KEY (registration_id)
    REFERENCES registrations(id) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
```

## What Was Fixed

### 1. **Database Column Name Correction**
- **Previous Issue**: Code was using `time_in` but database column is `check_in_time`
- **Fixed**: Updated all queries to use correct column name `check_in_time`

### 2. **Comprehensive QR Code Validation**

#### Validation Layer 1: QR Code Recognition
- Checks if the scanned QR code exists in the `registrations` table
- If not found, shows error: "QR code not recognized"

#### Validation Layer 2: Registration Status Check
- Verifies that the registration status is "Approved"
- Prevents attendance recording for "Pending" or "Rejected" registrations
- Shows detailed error message with user and event information

#### Validation Layer 3: Event Timing Validation
- Checks if the event has already ended (compares with `end_datetime`)
- Prevents check-in for past events
- Shows event end date/time in error message

#### Validation Layer 4: Duplicate Prevention
- Checks if attendance record already exists for the registration
- Prevents duplicate check-in entries
- Implements 3-second cooldown to prevent accidental re-scans

### 3. **Enhanced User Feedback**
When a QR code is scanned, the system now displays:
- ? User's full name
- ?? Email address
- ?? Event name
- ?? Check-in/Check-out timestamp
- Status icons (????) for visual clarity

## Check-In/Check-Out Flow

### **Check-In Process**
1. User shows QR code to camera
2. System scans and validates QR code
3. System performs all validations (see above)
4. If all validations pass:
   - Insert new record in `attendance` table
   - Set `check_in_time` to current timestamp
   - Set `check_out_time` to NULL
   - Display success message with user details

### **Check-Out Process** (Automatic Detection)

#### Design Approach:
The system automatically detects if a user is checking in or checking out:

1. **First Scan (Check-in not recorded):**
   - No attendance record exists ? Record CHECK-IN
   - Insert new row with `check_in_time = NOW()`, `check_out_time = NULL`

2. **Second Scan (Check-in already recorded, no check-out):**
   - Attendance record exists with `check_out_time = NULL`
   - System prompts: "User has already checked in. Record CHECK-OUT?"
   - If YES ? Update existing record: `check_out_time = NOW()`
   - If NO ? Cancel and keep current state

3. **Third Scan (Both check-in and check-out recorded):**
   - Shows informational message
   - Displays both check-in and check-out times
   - No further action taken

### **Alternative Check-Out Designs**

#### **Option 1: Automatic Check-Out (Current Implementation) ?**
**Pros:**
- Simple user experience
- No additional UI needed
- Same QR code for both check-in and check-out

**Cons:**
- Requires confirmation dialog to prevent accidental check-outs

#### **Option 2: Separate Check-Out Scanner**
```csharp
// Add a toggle button or separate form for check-out mode
private bool isCheckOutMode = false;

private void btnToggleMode_Click(object sender, EventArgs e)
{
    isCheckOutMode = !isCheckOutMode;
    lblMode.Text = isCheckOutMode ? "MODE: CHECK-OUT" : "MODE: CHECK-IN";
}
```

**Pros:**
- Clear separation of check-in and check-out
- No confirmation dialog needed

**Cons:**
- Requires manual mode switching
- More complex UI

#### **Option 3: Time-Based Auto Check-Out**
```sql
-- Schedule or trigger to auto check-out after event ends
UPDATE attendance a
INNER JOIN registrations r ON a.registration_id = r.id
INNER JOIN events e ON r.event_id = e.id
SET a.check_out_time = e.end_datetime
WHERE a.check_out_time IS NULL 
  AND NOW() > e.end_datetime;
```

**Pros:**
- Fully automated
- No user action required

**Cons:**
- Assumes all attendees stayed until the end
- Inaccurate for early leavers

#### **Option 4: Manual Check-Out Button in Admin Panel**
Add a button in the Event Attendees form to manually check out users:

```csharp
private void CheckOutUser(int registrationId)
{
    string query = @"
        UPDATE attendance 
        SET check_out_time = NOW() 
        WHERE registration_id = @regId 
          AND check_out_time IS NULL";
    // Execute query...
}
```

**Pros:**
- Admin has full control
- Can correct mistakes

**Cons:**
- Manual process
- Time-consuming for large events

## Recommended Approach

**Current Implementation (Option 1)** is recommended because:
1. ? Uses the same QR code infrastructure
2. ? User-friendly (attendees just scan again when leaving)
3. ? Includes safety confirmation dialog
4. ? Works with existing mobile app QR codes
5. ? Real-time check-out recording

## Usage Instructions

### For Administrators:

1. **Start Scanner**
   - Open "Attendance Scanner" from Admin Dashboard
   - Select camera from dropdown
   - Click "Start Scanner"

2. **Check-In Process**
   - Ask attendee to show QR code
   - System automatically scans and validates
   - Confirmation message shows check-in details
   - Status updates to "? Check-in recorded successfully!"

3. **Check-Out Process**
   - Ask attendee to show QR code again when leaving
   - System detects existing check-in
   - Prompts: "Record CHECK-OUT?"
   - Click YES to record check-out time
   - Confirmation shows check-out details

4. **Stop Scanner**
   - Click "Stop" button when done
   - Camera feed stops

### For Attendees:

1. **Check-In:**
   - Show your QR code from mobile app or printed version
   - Wait for confirmation beep/message
   - You're checked in! ?

2. **Check-Out:**
   - Show the same QR code when leaving the event
   - Wait for admin to confirm check-out
   - You're checked out! ?

## Attendance Status Display

The system shows different attendance statuses:

| Status | Meaning | Display Conditions |
|--------|---------|-------------------|
| **N/A** | Not applicable | Event is upcoming or ongoing, no check-in |
| **Checked-in** | User checked in, not out yet | `check_in_time` exists, `check_out_time` is NULL |
| **Attended** | Full attendance | Both `check_in_time` and `check_out_time` exist |
| **Didn't Attend** | Registered but didn't show | Event ended, no attendance record |

## Error Handling

The scanner handles these error scenarios:

1. ? **Invalid QR Code**: Not found in database
2. ? **Not Approved**: Registration status is Pending/Rejected
3. ? **Event Ended**: Cannot check-in to past events
4. ?? **Already Checked In**: Offers check-out option
5. ?? **Already Checked Out**: Shows completion message
6. ? **Database Error**: Shows technical error message

## Technical Implementation Details

### Key Features:
- **3-second scan cooldown**: Prevents rapid duplicate scans
- **QR code caching**: Tracks last scanned QR to prevent duplicates
- **Atomic transactions**: Ensures data consistency
- **Real-time validation**: Checks database state before recording
- **User-friendly messages**: Clear, emoji-enhanced status updates
- **Cross-reference validation**: Joins users, events, and registrations tables

### Database Queries:

**Check-In Query:**
```sql
INSERT INTO attendance (registration_id, check_in_time) 
VALUES (@id, NOW())
```

**Check-Out Query:**
```sql
UPDATE attendance 
SET check_out_time = NOW() 
WHERE id = @id
```

**Validation Query:**
```sql
SELECT 
    r.id AS registration_id,
    r.status,
    u.name AS user_name,
    u.email,
    e.name AS event_name,
    e.start_datetime,
    e.end_datetime
FROM registrations r
INNER JOIN users u ON r.user_id = u.id
INNER JOIN events e ON r.event_id = e.id
WHERE r.qr_code = @qr
```

## Future Enhancements (Optional)

1. **Audio Feedback**: Add beep sound on successful scan
2. **Attendance Statistics**: Show live count of checked-in users
3. **Offline Support**: Cache scans and sync when online
4. **Multi-Event Selection**: Filter scans by specific event
5. **Export Logs**: Generate attendance reports in real-time
6. **Face Recognition**: Combine QR with facial verification
7. **SMS Notifications**: Send check-in/out confirmations to attendees

## Testing Checklist

- [ ] Scan valid QR code ? Check-in successful
- [ ] Scan same QR code again ? Prompt for check-out
- [ ] Scan QR code for rejected registration ? Error shown
- [ ] Scan QR code for ended event ? Error shown
- [ ] Scan invalid QR code ? Error shown
- [ ] Check database: `check_in_time` populated correctly
- [ ] Check database: `check_out_time` populated on second scan
- [ ] Verify 3-second cooldown works
- [ ] Verify user details displayed correctly
- [ ] Stop scanner ? Camera stops properly

## Summary

? **QR Code Scanner is now fully functional with:**
- Complete validation pipeline
- Automatic check-in/check-out detection
- User-friendly error messages
- Database integrity checks
- Duplicate prevention
- Event timing validation

? **Check-Out is handled automatically:**
- Same QR code for both check-in and check-out
- System intelligently detects current state
- Confirmation dialog prevents accidents
- Real-time database updates

The implementation follows best practices and provides a seamless experience for both administrators and attendees.
