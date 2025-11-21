# Quick Reference: Event Timing Validation

## Validation Matrix

| Scenario | Event State | Check-In | Check-Out |
|----------|-------------|----------|-----------|
| Before event starts | Not Started | ? DENIED | N/A |
| Event is ongoing | In Progress | ? ALLOWED | ? DENIED |
| Event has ended | Ended | ? DENIED | ? ALLOWED |

## Visual Timeline

```
?????????????????????????????????????????????????????????????
        ?                          ?                    ?
    Event Start              Current Time          Event End
    2:00 PM                                         5:00 PM
```

### Zone 1: BEFORE Event Start (? No Check-In)
```
NOW: 1:30 PM
???????????????????
        ? TOO EARLY
        ?????????????????????????????????????????????
                ?                                  ?
            Event Start                        Event End
            2:00 PM                            5:00 PM
            
Action: Check-In
Result: ? "Event hasn't started yet. Event starts in 30 minutes."
```

### Zone 2: DURING Event (? Check-In, ? No Check-Out)
```
????????????????????????????
                ?           NOW: 3:00 PM          ?
            Event Start     ? CAN CHECK-IN   Event End
            2:00 PM         ? NO CHECK-OUT    5:00 PM
            
Action: Check-In
Result: ? "Check-in recorded successfully!"

Action: Check-Out (if already checked in)
Result: ? "Event is still ongoing. Event ends in 2 hours."
```

### Zone 3: AFTER Event Ends (? No Check-In, ? Check-Out)
```
??????????????????????????????????????????????????
                ?                                  ?      NOW: 5:10 PM
            Event Start                        Event End  ? CAN CHECK-OUT
            2:00 PM                            5:00 PM    ? NO CHECK-IN
            
Action: Check-In (new user)
Result: ? "Event has already ended."

Action: Check-Out (if already checked in)
Result: ? "Check-out recorded successfully!"
```

## Status Messages Quick Reference

| Symbol | Meaning | Color | When Shown |
|--------|---------|-------|------------|
| ? | Success | Green | Check-in/out successful |
| ? | Error | Red | Critical validation failure |
| ? | Timing Issue | Yellow | Event timing constraint |
| ?? | Information | Orange | Already processed |

## Error Messages

### Check-In Errors

#### "Event hasn't started yet"
- **When**: Current time < Event start time
- **Status Color**: Yellow (?)
- **Action**: Wait until event starts
- **Example**: "Event starts in approximately 30 minutes"

#### "Event has already ended"
- **When**: Current time > Event end time
- **Status Color**: Red (?)
- **Action**: Cannot check in to past events
- **Example**: "Cannot record attendance for past events"

### Check-Out Errors

#### "Event is still ongoing"
- **When**: Current time < Event end time (and user already checked in)
- **Status Color**: Yellow (?)
- **Action**: Wait until event ends
- **Example**: "Event ends in approximately 1 hour"

#### "Attendance already completed"
- **When**: User already checked in AND checked out
- **Status Color**: Orange (??)
- **Action**: No further action needed
- **Example**: Shows both check-in and check-out times

## Status Label Styling

```css
Background Color: #252A40 (Dark Blue)
Border: 1px solid (Single line)
Padding: 15px horizontal, 10px vertical
Text Alignment: Middle-Left
Font: Segoe UI, 14pt
```

## Color Palette

| Status | Hex Color | RGB |
|--------|-----------|-----|
| Success (Green) | #4CAF50 | 76, 175, 80 |
| Error (Red) | #D32F2F | 211, 47, 47 |
| Warning (Orange) | #FF9800 | 255, 152, 0 |
| Info (Yellow) | #FFC107 | 255, 193, 7 |
| Background (Dark Blue) | #252A40 | 37, 42, 64 |

## Time Calculation

The system calculates remaining time and displays it in the most appropriate unit:

```
? 1 day    ? "X day(s)"
? 1 hour   ? "X hour(s)"
< 1 hour   ? "X minute(s)"
```

### Examples:
- `48 hours` ? "2 days"
- `90 minutes` ? "1 hour"
- `45 minutes` ? "45 minutes"
- `1 minute` ? "1 minute"

## Implementation Notes

### Check-In Validation Order:
1. QR Code exists?
2. Registration approved?
3. Event NOT ended?
4. Event HAS started? ? **NEW**
5. Not already checked in?
6. ? Record check-in ?

### Check-Out Validation Order:
1. QR Code exists?
2. Registration approved/checked-in?
3. Event NOT ended? (for timeline context)
4. Already checked in?
5. NOT already checked out?
6. Event HAS ended? ? **NEW**
7. User confirms?
8. ? Record check-out ?

## Admin Tips

### Scenario: User arrives early
**What they see**: "Event hasn't started yet"
**What admin should do**: Inform them to wait until event starts

### Scenario: User wants to leave early
**What they see**: "Event is still ongoing"
**What admin should do**: Explain they must stay until event ends to check out

### Scenario: User arrives after event starts
**What they see**: "Check-in recorded successfully!"
**What admin should do**: No action needed, system works as expected

### Scenario: User leaves after event ends
**What they see**: Prompt to check out
**What admin should do**: Confirm their check-out

## FAQ

**Q: What if someone needs to check in early?**
A: The system enforces event start time. You can modify the event start time in the database if needed.

**Q: What if someone needs to leave before the event ends?**
A: They cannot check out until the event end time. You can manually update the attendance record in the database if absolutely necessary.

**Q: Can we allow check-in X minutes before the event?**
A: Yes, you can modify the validation logic to allow early check-in (see configuration section in main documentation).

**Q: What happens if the event time is extended?**
A: Update the event end time in the database. Users already checked in can check out after the new end time.

**Q: Does this affect existing attendance records?**
A: No, this only validates new check-ins and check-outs. Existing records are not modified.
