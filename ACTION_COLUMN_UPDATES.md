# Action Column Updates - Event Attendees & Registrations

## Overview
Updated the Action column in both Event Attendees and Registrations forms to display "N/A" for ended events instead of showing an actionable button. Also added important additional columns to the Registrations table.

## Changes Made

### 1. Event Attendees Form (`frmEventAttendees.cs`)

#### A. Action Column Display

**Previous Behavior:**
- Showed "..." button for all registrations regardless of event status

**New Behavior:**
- Shows "**N/A**" text for events that have ended
- Shows "**...**" action button for ongoing or upcoming events

#### B. Implementation Details

**Added Hidden Column:**
```csharp
dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn
{
    Name = "event_end_datetime",
    HeaderText = "Event End",
    ReadOnly = true,
    Visible = false  // Hidden column for checking event status
});
```

**Updated CellPainting Logic:**
```csharp
// Check if event has ended
var eventEndValue = row.Cells["event_end_datetime"].Value;
bool eventHasEnded = eventEndValue != DBNull.Value && 
                     DateTime.Now > Convert.ToDateTime(eventEndValue);

if (eventHasEnded)
{
    // Draw "N/A" text for ended events
    e.Graphics.DrawString("N/A", naFont, textBrush, cellBounds, sf);
}
else
{
    // Draw action button for ongoing/upcoming events
    e.Graphics.DrawString("...", btnFont, textBrush, buttonRect, sf);
}
```

**Updated CellClick Logic:**
```csharp
// Check if event has ended
bool eventHasEnded = eventEndValue != DBNull.Value && 
                     DateTime.Now > Convert.ToDateTime(eventEndValue);

if (eventHasEnded)
{
    // Don't show menu for ended events
    return;
}
```

### 2. Registrations Form (`frmRegistrations.cs`)

#### A. New Columns Added

**Email Column:**
- Displays user's email address
- FillWeight: 22
- Essential contact information

**Event Date Column:**
- Displays formatted event start date
- Format: "MMM dd, YYYY" (e.g., "Jan 15, 2025")
- FillWeight: 15
- Helps identify which event the registration is for

**Hidden Event End Column:**
- Name: `event_end_datetime`
- Used to determine if event has ended
- Not visible to users

#### B. Updated Query

```sql
SELECT 
    r.id, 
    e.name AS event_name, 
    u.name AS user_name,
    u.email,                                              -- NEW
    DATE_FORMAT(e.start_datetime, '%b %d, %Y') AS event_date,  -- NEW
    e.end_datetime,                                       -- NEW
    r.role, 
    r.status, 
    r.qr_code 
FROM registrations r
INNER JOIN events e ON r.event_id = e.id
INNER JOIN users u ON r.user_id = u.id
ORDER BY r.status DESC, e.start_datetime DESC
```

#### C. Column Layout (Before vs After)

**Before:**
| Event Name | User Name | Role | Status | Action |
|------------|-----------|------|---------|---------|
| 30% | 25% | 15% | 15% | 15% |

**After:**
| Event Name | User Name | Email | Event Date | Role | Status | Action |
|------------|-----------|-------|------------|------|---------|---------|
| 25% | 20% | 22% | 15% | 12% | 12% | 12% |

#### D. Action Column Behavior

Same as Event Attendees:
- **"N/A"** for ended events
- **"..."** button for ongoing/upcoming events

## Visual Comparison

### For Ended Events

**Before:**
```
??????????????????????????????????????????
? Event Name     ? User Name    ? Action ?
??????????????????????????????????????????
? Past Event     ? John Doe     ?  ...   ?  ? Clickable button
??????????????????????????????????????????
```

**After:**
```
??????????????????????????????????????????
? Event Name     ? User Name    ? Action ?
??????????????????????????????????????????
? Past Event     ? John Doe     ?  N/A   ?  ? Greyed out text
??????????????????????????????????????????
```

### For Ongoing/Upcoming Events

```
??????????????????????????????????????????
? Event Name     ? User Name    ? Action ?
??????????????????????????????????????????
? Future Event   ? Jane Smith   ?  ...   ?  ? Clickable button
??????????????????????????????????????????
```

## Benefits

### 1. **Clear Visual Indication**
- Users can immediately see which events have ended
- No confusion about why actions aren't available

### 2. **Prevents Unnecessary Clicks**
- No clicking on non-actionable buttons
- Better user experience

### 3. **More Information in Registrations**
- **Email**: Quick access to contact user
- **Event Date**: Easy identification of events
- Better overview at a glance

### 4. **Consistent Behavior**
- Both forms (Event Attendees and Registrations) behave identically
- Predictable user interface

## Event Status Logic

```
Current Time vs Event End DateTime:

???????????????????????????????????????????????
?                                             ?
?  If DateTime.Now > Event.end_datetime       ?
?  ? Event Has Ended                          ?
?  ? Show "N/A"                               ?
?  ? Disable click actions                    ?
?                                             ?
?  Else                                       ?
?  ? Event is Ongoing or Upcoming             ?
?  ? Show "..." button                        ?
?  ? Enable click actions                     ?
?                                             ?
???????????????????????????????????????????????
```

## Color & Styling

### "N/A" Text Styling:
```csharp
Font: Segoe UI, 10pt, Regular
Color: RGB(158, 161, 178)  // Muted gray
Alignment: Center
```

### Action Button (Unchanged):
```csharp
Font: Segoe UI, 12pt, Bold
Background: RGB(0, 126, 249)  // Blue
Text: White
Text: "..."
Shape: Rounded rectangle (10px radius)
```

## Testing Checklist

### Event Attendees Form
- [ ] Past events show "N/A" in Action column
- [ ] Ongoing events show "..." button in Action column
- [ ] Future events show "..." button in Action column
- [ ] Clicking "N/A" does nothing (no menu appears)
- [ ] Clicking "..." shows context menu
- [ ] All other columns display correctly

### Registrations Form
- [ ] Email column displays user emails correctly
- [ ] Event Date column shows formatted dates
- [ ] Past events show "N/A" in Action column
- [ ] Ongoing events show "..." button
- [ ] Future events show "..." button
- [ ] Clicking "N/A" does nothing
- [ ] Clicking "..." shows context menu
- [ ] Column widths are balanced

## Database Impact

**No schema changes required.**

Only query changes:
- Added `email` field from `users` table
- Added formatted `event_date` from `events.start_datetime`
- Added `end_datetime` from `events` table

## Summary

? **Action Column:**
- Shows "N/A" for ended events
- Shows "..." button for active events
- Prevents clicks on ended events

? **Registrations Table Enhanced:**
- Email column added (22% width)
- Event Date column added (15% width)
- Better information overview

? **Consistent UI:**
- Both forms behave identically
- Clear visual feedback
- Improved user experience

The changes provide a clearer, more informative interface while preventing confusion about why certain actions aren't available for past events.
