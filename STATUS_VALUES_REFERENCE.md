# Quick Reference: Status Values & Transitions

## Status Values

| Status | Meaning | How it's Set |
|--------|---------|--------------|
| **Pending** | Registration awaiting approval | User registers for event |
| **Approved** | Registration approved, not yet checked in | Admin approves registration |
| **Checked-in** | User has arrived and checked in | QR code scanned for check-in |
| **Attended** | User completed full attendance | QR code scanned for check-out |
| **Didn't Attend** | User didn't show up | Auto-set when event ends without check-in |
| **Rejected** | Registration not approved | Admin rejects registration |

## Status Transitions

### Normal Flow
```
Pending ? Approved ? Checked-in ? Attended
```

### No-Show Flow
```
Pending ? Approved ? Didn't Attend (when event ends)
```

### Rejection Flow
```
Pending ? Rejected
Approved ? Rejected (can still be rejected after approval)
```

### Re-approval Flow
```
Rejected ? Approved (admin can re-approve)
```

## Color Coding Recommendations

For visual clarity in the UI, you can add color coding based on status:

```csharp
// In dgvAttendees_CellFormatting event or LoadAttendees method
switch (status)
{
    case "Attended":
        cell.ForeColor = Color.FromArgb(76, 175, 80);  // Green
        break;
    case "Checked-in":
        cell.ForeColor = Color.FromArgb(33, 150, 243); // Blue
        break;
    case "Approved":
        cell.ForeColor = Color.FromArgb(255, 193, 7);  // Yellow/Gold
        break;
    case "Pending":
        cell.ForeColor = Color.FromArgb(158, 161, 178); // Gray
        break;
    case "Didn't Attend":
        cell.ForeColor = Color.FromArgb(255, 152, 0);  // Orange
        break;
    case "Rejected":
        cell.ForeColor = Color.FromArgb(211, 47, 47);  // Red
        break;
}
```

## When Status Changes Happen

| Trigger | Status Change |
|---------|--------------|
| User submits registration | ? **Pending** |
| Admin clicks "Approve" | Pending ? **Approved** |
| Admin clicks "Reject" | Any ? **Rejected** |
| QR code scanned (first time) | Approved ? **Checked-in** |
| QR code scanned (second time) | Checked-in ? **Attended** |
| Event ends (auto-check) | Approved ? **Didn't Attend** |
| Admin re-approves | Rejected ? **Approved** |

## Important Notes

1. **"Checked-in" status allows re-scanning**: Users can still scan their QR code even if status is "Checked-in" (for check-out).

2. **"Didn't Attend" is auto-set**: This happens when LoadAttendees() is called after the event has ended.

3. **Database persistence**: All status changes are immediately saved to the database, not just displayed.

4. **No manual "Attended" setting**: Users cannot be manually marked as "Attended" - they must check in and check out via QR scanner.
