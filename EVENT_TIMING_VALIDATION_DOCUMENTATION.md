# Event Timing Validation - Attendance Scanner

## Overview
The attendance scanner now includes event timing validation to ensure check-ins and check-outs can only occur during the appropriate time windows.

## Validation Rules

### ? **Check-In Validation**

#### Rule: Event Must Have Started
- **Condition**: `Current Time >= Event Start DateTime`
- **Error**: "Event hasn't started yet"
- **Behavior**: Prevents users from checking in before the event begins

**Example Scenario:**
```
Event Start: March 15, 2025 at 2:00 PM
Current Time: March 15, 2025 at 1:30 PM
Result: ? Check-in DENIED
Message: "Event starts in approximately 30 minutes. Please wait until the event begins to check in."
```

### ? **Check-Out Validation**

#### Rule: Event Must Have Ended
- **Condition**: `Current Time >= Event End DateTime`
- **Error**: "Event is still ongoing"
- **Behavior**: Prevents users from checking out before the event ends

**Example Scenario:**
```
Event End: March 15, 2025 at 5:00 PM
Current Time: March 15, 2025 at 4:30 PM
Check-in Time: March 15, 2025 at 2:15 PM
Result: ? Check-out DENIED
Message: "Event ends in approximately 30 minutes. Check-out will be available after the event ends."
```

## Complete Validation Flow

### Check-In Flow (New Attendance Record)

```
Step 1: Scan QR Code
   ?
Step 2: QR Code Valid? ? NO ? ? "QR code not recognized"
   ? YES
Step 3: Registration Approved? ? NO ? ? "Registration not approved"
   ? YES
Step 4: Event Has Ended? ? YES ? ? "Event has already ended"
   ? NO
Step 5: Event Has Started? ? NO ? ? "Event hasn't started yet"
   ? YES
Step 6: Record Check-in ? ? Success!
```

### Check-Out Flow (Existing Check-In Record)

```
Step 1: Scan QR Code
   ?
Step 2: QR Code Valid? ? NO ? ? "QR code not recognized"
   ? YES
Step 3: Registration Approved/Checked-in? ? NO ? ? "Registration not approved"
   ? YES
Step 4: Check-in Exists? ? NO ? (Go to Check-In Flow)
   ? YES
Step 5: Already Checked Out? ? YES ? ?? "Attendance already completed"
   ? NO
Step 6: Event Has Ended? ? NO ? ? "Event is still ongoing"
   ? YES
Step 7: Confirm Check-out? ? NO ? Cancelled
   ? YES
Step 8: Record Check-out ? ? Success!
```

## Time Display Messages

The system provides friendly time-remaining messages:

### Days Remaining
```
"Event starts in approximately 2 days"
"Event ends in approximately 1 day"
```

### Hours Remaining
```
"Event starts in approximately 3 hours"
"Event ends in approximately 2 hours"
```

### Minutes Remaining
```
"Event starts in approximately 45 minutes"
"Event ends in approximately 15 minutes"
```

## UI Enhancements

### Status Label Background
The status label now has a styled background container:
- **Background Color**: `Color.FromArgb(37, 42, 64)` - Dark blue
- **Border**: Fixed single-line border
- **Padding**: 15px horizontal, 10px vertical
- **Alignment**: Middle-Left

### Status Colors

| Status Type | Color | RGB | Emoji |
|-------------|-------|-----|-------|
| **Success** | Green | `76, 175, 80` | ? |
| **Error** | Red | `211, 47, 47` | ? |
| **Warning** | Orange | `255, 152, 0` | ?? |
| **Info** | Yellow | `255, 193, 7` | ? |
| **Processing** | Yellow | `255, 193, 7` | - |
| **Neutral** | White | `255, 255, 255` | - |

## Code Implementation

### Validation 4: Event Start Check
```csharp
// Check if event has started (for check-in)
if (currentTime < eventStartDateTime)
{
    lblStatus.Text = "? Status: Event hasn't started yet.";
    lblStatus.ForeColor = Color.FromArgb(255, 193, 7); // Yellow
    
    TimeSpan timeUntilStart = eventStartDateTime - currentTime;
    // Calculate friendly time message...
    
    MessageBox.Show($"Event starts in approximately {timeMessage}.\n" +
        "Please wait until the event begins to check in.",
        "Event Not Started", MessageBoxButtons.OK, MessageBoxIcon.Information);
    return;
}
```

### Validation 5: Event End Check (for Check-Out)
```csharp
// Check if event is still ongoing (before allowing check-out)
if (currentTime < eventEndDateTime)
{
    lblStatus.Text = "? Status: Event is still ongoing.";
    lblStatus.ForeColor = Color.FromArgb(255, 193, 7); // Yellow
    
    TimeSpan timeUntilEnd = eventEndDateTime - currentTime;
    // Calculate friendly time message...
    
    MessageBox.Show($"Event ends in approximately {timeMessage}.\n" +
        "Check-out will be available after the event ends.",
        "Event Still Ongoing", MessageBoxButtons.OK, MessageBoxIcon.Information);
    return;
}
```

## Use Cases

### Use Case 1: Early Bird Attendee
**Scenario**: User arrives 30 minutes before the event starts

**Action**: Scans QR code for check-in

**Result**: 
```
Status: ? Event hasn't started yet.
Message: "Event starts in approximately 30 minutes. 
         Please wait until the event begins to check in."
```

**Outcome**: Check-in denied, user must wait

---

### Use Case 2: Mid-Event Check-Out Attempt
**Scenario**: User wants to leave early while the event is still running

**Action**: Scans QR code (already checked in)

**Result**:
```
Status: ? Event is still ongoing.
Message: "Event ends in approximately 1 hour. 
         Check-out will be available after the event ends."
```

**Outcome**: Check-out denied, user must wait until event ends

---

### Use Case 3: Normal Check-In
**Scenario**: User arrives after event has started

**Event Start**: 2:00 PM
**Current Time**: 2:15 PM

**Action**: Scans QR code

**Result**:
```
Status: ? Check-in recorded successfully!
Message: "CHECK-IN SUCCESSFUL!"
```

**Outcome**: Check-in successful

---

### Use Case 4: Normal Check-Out
**Scenario**: User leaves after event has ended

**Event End**: 5:00 PM
**Current Time**: 5:05 PM

**Action**: Scans QR code (already checked in)

**Result**:
```
Prompt: "Would you like to record CHECK-OUT now?"
User clicks: YES

Status: ? Check-out recorded successfully!
Message: "CHECK-OUT SUCCESSFUL!"
```

**Outcome**: Check-out successful

## Benefits

### 1. **Data Accuracy**
- Prevents premature check-ins
- Ensures check-outs only occur after event completion
- Maintains integrity of attendance records

### 2. **Event Control**
- Admins have better control over attendance timing
- Prevents gaming the system
- Ensures attendees stay for the entire event

### 3. **User Experience**
- Clear feedback on why action is denied
- Friendly time-remaining messages
- Visual status indicators with emojis

### 4. **Business Logic**
- Aligns with real-world event attendance policies
- Supports event organizers' requirements
- Provides audit trail with accurate timestamps

## Configuration

### Adjusting Validation Rules (Optional)

If you need to allow early check-ins or early check-outs:

#### Allow Check-In X Minutes Before Start
```csharp
// Allow check-in 15 minutes early
int earlyCheckInMinutes = 15;
if (currentTime < eventStartDateTime.AddMinutes(-earlyCheckInMinutes))
{
    // Show "too early" message
}
```

#### Allow Check-Out Before Event Ends
```csharp
// Remove the check-out timing validation
// Comment out or remove the validation in RecordAttendance()
```

## Testing Scenarios

### Test Case 1: Check-In Too Early
- [ ] Set event start to future date/time
- [ ] Scan QR code
- [ ] Verify: ? Error shown with time remaining
- [ ] Verify: Check-in NOT recorded in database

### Test Case 2: Check-Out Too Early
- [ ] Check in to an ongoing event
- [ ] Try to check out before event ends
- [ ] Verify: ? Error shown with time remaining
- [ ] Verify: Check-out NOT recorded in database

### Test Case 3: Valid Check-In
- [ ] Set event start to past date/time
- [ ] Scan QR code
- [ ] Verify: ? Success message shown
- [ ] Verify: Check-in recorded in database

### Test Case 4: Valid Check-Out
- [ ] Check in to an event
- [ ] Wait for event to end (or set end time to past)
- [ ] Scan QR code again
- [ ] Verify: Prompt to check out
- [ ] Verify: ? Success message shown
- [ ] Verify: Check-out recorded in database

### Test Case 5: Status Label Styling
- [ ] Start scanner
- [ ] Verify: Status label has dark background
- [ ] Verify: Status label has border
- [ ] Verify: Status text is readable with padding

## Database Impact

No database schema changes are required. The validation logic only affects:
- **INSERT** operations to `attendance` table
- **UPDATE** operations to `attendance` table
- **UPDATE** operations to `registrations.status` column

All validations are performed **before** database operations, preventing invalid data from being written.

## Summary

? **Check-in validation**: Event must have started
? **Check-out validation**: Event must have ended
? **Friendly time messages**: Shows remaining time until valid
? **Visual indicators**: Colored status with emojis
? **Styled status container**: Background color and border added
? **Better UX**: Clear feedback on why actions are denied

The system now enforces proper event timing constraints while maintaining a user-friendly experience with helpful error messages and visual feedback.
