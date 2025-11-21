# User My Events - Action Column Update

## Overview
Updated the `frmMyEvents` form to automatically display "N/A" in the Action column for rejected registrations or events that have ended, matching the behavior of the admin forms.

## Changes Made

### 1. Added Hidden Column

**Event End DateTime Column:**
```csharp
dgvMyEvents.Columns.Add(new DataGridViewTextBoxColumn
{
    Name = "event_end_datetime",
    HeaderText = "Event End",
    ReadOnly = true,
    Visible = false  // Hidden column for checking event status
});
```

### 2. Updated Query

**Added `end_datetime` to Query:**
```sql
SELECT 
    r.id AS registration_id,
    e.id AS event_id,
    e.name AS event_name,
    -- ... other fields ...
    e.end_datetime,  -- NEW: Added to check if event has ended
    r.role,
    r.status
FROM registrations r
INNER JOIN events e ON r.event_id = e.id
WHERE r.user_id = @user_id
ORDER BY e.start_datetime DESC
```

### 3. Updated CellPainting Logic

**Show "N/A" for:**
1. Events that have ended
2. Rejected registrations
3. "Didn't Attend" status

```csharp
// Check if event has ended or status is rejected
bool eventHasEnded = eventEndValue != DBNull.Value && 
                     DateTime.Now > Convert.ToDateTime(eventEndValue);
bool isRejected = status == "Rejected" || status == "Didn't Attend";

if (eventHasEnded || isRejected)
{
    // Draw "N/A" text
    e.Graphics.DrawString("N/A", naFont, textBrush, cellBounds, sf);
}
else
{
    // Draw "..." action button
    e.Graphics.DrawString("...", btnFont, textBrush, buttonRect, sf);
}
```

### 4. Updated CellClick Logic

**Prevent Menu for:**
- Ended events
- Rejected registrations
- "Didn't Attend" status

```csharp
if (eventHasEnded || isRejected)
{
    // Don't show menu for ended events or rejected registrations
    return;
}
```

### 5. Enhanced Status Support

Added support for "Checked-in" status:
```csharp
else if (status == "Approved" || status == "Attended" || status == "Checked-in")
{
    // Show View QR for approved/checked-in/attended registrations
    ToolStripMenuItem viewQRItem = new ToolStripMenuItem("?? View QR");
    // ...
}
```

## Visual Behavior

### For Rejected/Ended Events

**Before:**
```
??????????????????????????????????????
? Event Name     ? Status   ? Action ?
??????????????????????????????????????
? Past Event     ? Attended ?  ...   ? ? Clickable but no actions
? Rejected Event ? Rejected ?  ...   ? ? Shows "N/A" in menu
??????????????????????????????????????
```

**After:**
```
??????????????????????????????????????
? Event Name     ? Status   ? Action ?
??????????????????????????????????????
? Past Event     ? Attended ?  N/A   ? ? Greyed out text
? Rejected Event ? Rejected ?  N/A   ? ? Greyed out text
??????????????????????????????????????
```

### For Active Events

```
??????????????????????????????????????
? Event Name     ? Status   ? Action ?
??????????????????????????????????????
? Upcoming Event ? Pending  ?  ...   ? ? Shows Unregister
? Approved Event ? Approved ?  ...   ? ? Shows View QR
? Checked-in     ? Checked-in? ...   ? ? Shows View QR
??????????????????????????????????????
```

## Action Column Rules

| Condition | Display | Clickable | Menu Items |
|-----------|---------|-----------|------------|
| **Status: Pending** | `...` button | ? Yes | ? Unregister |
| **Status: Approved** | `...` button | ? Yes | ?? View QR |
| **Status: Checked-in** | `...` button | ? Yes | ?? View QR |
| **Status: Attended** (ongoing) | `...` button | ? Yes | ?? View QR |
| **Status: Rejected** | `N/A` text | ? No | None |
| **Status: Didn't Attend** | `N/A` text | ? No | None |
| **Event Ended** | `N/A` text | ? No | None |

## Status Flow with Action Column

```
User Registers
    ?
???????????
? Pending ? ? Action: ... (Unregister available)
???????????
    ? (Admin Approves)
????????????
? Approved ? ? Action: ... (View QR available)
????????????
    ? (Scans QR for check-in)
??????????????
? Checked-in ? ? Action: ... (View QR available)
??????????????
    ? (Scans QR for check-out)
????????????
? Attended ? ? Action: ... (View QR available)
????????????
    ? (Event Ends)
????????????
? Attended ? ? Action: N/A (No actions available)
????????????

Alternative Paths:
????????????
? Rejected ? ? Action: N/A (No actions available)
????????????

???????????????????
? Didn't Attend   ? ? Action: N/A (No actions available)
???????????????????
```

## Comparison with Admin Forms

| Form | Shows "N/A" For |
|------|----------------|
| **frmEventAttendees** | Ended events |
| **frmRegistrations** | Ended events |
| **frmMyEvents** (User) | Ended events OR Rejected registrations |

**Why the difference?**
- **Admin forms**: Admins need to manage all registrations regardless of status
- **User form**: Users don't need actions for rejected registrations or past events

## Benefits

### 1. **Clear Visual Feedback**
- Users immediately see which events/registrations have no available actions
- Consistent with admin interface design

### 2. **Prevents Confusion**
- No clicking on buttons that show empty menus
- Clear indication that registration was rejected or event has ended

### 3. **Better User Experience**
- Matches admin-side behavior
- Intuitive interface
- Reduces unnecessary clicks

### 4. **Status Awareness**
- "Rejected" ? Shows N/A (user knows registration was declined)
- "Didn't Attend" ? Shows N/A (event ended without attendance)
- Event ended ? Shows N/A (no further actions possible)

## Implementation Details

### Color & Styling

**"N/A" Text:**
```csharp
Font: Segoe UI, 10pt, Regular
Color: RGB(158, 161, 178)  // Muted gray
Alignment: Center
```

**Action Button (Active Events):**
```csharp
Font: Segoe UI, 12pt, Bold
Background: RGB(0, 126, 249)  // Blue
Text: White
Text: "..."
Shape: Rounded rectangle (10px radius)
```

### Context Menu Behavior

**Previous Code (Kept Unchanged):**
```csharp
if (status == "Pending")
{
    // Show Unregister for pending registrations
    ToolStripMenuItem unregisterItem = new ToolStripMenuItem("? Unregister");
    // ...
}
else if (status == "Approved" || status == "Attended" || status == "Checked-in")
{
    // Show View QR for approved/checked-in/attended registrations
    ToolStripMenuItem viewQRItem = new ToolStripMenuItem("?? View QR");
    // ...
}
// Note: No else block needed - menu won't show for N/A cases
```

## Testing Checklist

### Status-Based Testing
- [ ] **Pending**: Shows "..." button, Unregister menu appears
- [ ] **Approved**: Shows "..." button, View QR menu appears
- [ ] **Checked-in**: Shows "..." button, View QR menu appears
- [ ] **Attended** (ongoing): Shows "..." button, View QR menu appears
- [ ] **Rejected**: Shows "N/A" text, no menu appears
- [ ] **Didn't Attend**: Shows "N/A" text, no menu appears

### Time-Based Testing
- [ ] Ongoing event with Approved status: Shows "..." button
- [ ] Ended event with Attended status: Shows "N/A" text
- [ ] Future event with Pending status: Shows "..." button

### Click Behavior
- [ ] Clicking "N/A" does nothing (no menu)
- [ ] Clicking "..." shows appropriate menu
- [ ] Menu items work correctly

## Database Impact

**No schema changes required.**

Only query modification:
- Added `end_datetime` from `events` table to check if event has ended

## Summary

? **Action Column Updated:**
- Shows "N/A" for rejected registrations
- Shows "N/A" for events that have ended
- Shows "..." button for active registrations

? **Consistent Behavior:**
- Matches admin-side forms
- Clear visual feedback
- Prevents unnecessary clicks

? **Enhanced Status Support:**
- Includes "Checked-in" status
- Handles all registration states appropriately

The user interface now provides clear, consistent feedback about which registrations have available actions, improving the overall user experience while maintaining consistency with the admin interface.
