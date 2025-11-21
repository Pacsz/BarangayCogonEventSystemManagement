-- ===================================================================
-- BARANGAY EVENT MANAGEMENT SYSTEM (BEMS) - UPDATED DATABASE SCHEMA
-- Database: bems_db
-- Updated: Aligned with new status configuration
-- Status Values: Pending, Approved, Checked-in, Attended, Rejected, Didn't Attend
-- ===================================================================

CREATE DATABASE IF NOT EXISTS bems_db;
USE bems_db;

SET FOREIGN_KEY_CHECKS = 0;
DROP TABLE IF EXISTS attendance;
DROP TABLE IF EXISTS reports;
DROP TABLE IF EXISTS registrations;
DROP TABLE IF EXISTS events;
DROP TABLE IF EXISTS users;
SET FOREIGN_KEY_CHECKS = 1;

-- ===================================================================
-- USERS TABLE
-- Stores user information with first_name and last_name
-- system_role: admin or user
-- ===================================================================
CREATE TABLE users (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    first_name VARCHAR(255) NOT NULL,
    last_name VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    system_role ENUM('admin','user') NOT NULL DEFAULT 'user',
    address TEXT,
    contact_number VARCHAR(50),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_email (email),
    INDEX idx_system_role (system_role)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ===================================================================
-- EVENTS TABLE
-- Stores event information with start and end datetime
-- ===================================================================
CREATE TABLE events (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    start_datetime DATETIME NOT NULL,
    end_datetime DATETIME NOT NULL,
    venue VARCHAR(255),
    type VARCHAR(100),
    organizer VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_start_datetime (start_datetime),
    INDEX idx_end_datetime (end_datetime),
    INDEX idx_type (type)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ===================================================================
-- REGISTRATIONS TABLE
-- Stores event registrations with role and status
-- UPDATED: Status now uses specific values instead of generic 'approved'
-- 
-- Status Values:
-- - Pending: Awaiting admin approval
-- - Approved: Approved by admin, QR generated
-- - Checked-in: User checked in at event
-- - Attended: User fully attended (checked in + checked out)
-- - Rejected: Registration rejected by admin
-- - Didn't Attend: Event ended without check-in
-- ===================================================================
CREATE TABLE registrations (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    event_id INT UNSIGNED NOT NULL,
    user_id INT UNSIGNED NOT NULL,
    role ENUM('attendee','volunteer','speaker') NOT NULL,
    status ENUM('Pending', 'Approved', 'Checked-in', 'Attended', 'Rejected', 'Didn''t Attend') DEFAULT 'Pending',
    qr_code VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY uniq_event_user (event_id, user_id),
    INDEX idx_status (status),
    INDEX idx_role (role),
    INDEX idx_event_status (event_id, status),
    INDEX idx_user_status (user_id, status),
    CONSTRAINT fk_reg_event FOREIGN KEY (event_id) 
        REFERENCES events(id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_reg_user FOREIGN KEY (user_id) 
        REFERENCES users(id) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ===================================================================
-- ATTENDANCE TABLE
-- Stores check-in and check-out times
-- NOTE: Status is now tracked in registrations.status field
-- This table is kept for historical data and time tracking
-- ===================================================================
CREATE TABLE attendance (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    registration_id INT UNSIGNED NOT NULL,
    check_in_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    check_out_time DATETIME NULL,
    INDEX idx_registration (registration_id),
    INDEX idx_check_in (check_in_time),
    INDEX idx_check_out (check_out_time),
    CONSTRAINT fk_att_reg FOREIGN KEY (registration_id) 
        REFERENCES registrations(id) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ===================================================================
-- REPORTS TABLE
-- Stores aggregated report data per event
-- UPDATED: Now tracks different attendance statuses
-- ===================================================================
CREATE TABLE reports (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    event_id INT UNSIGNED NOT NULL,
    total_registered INT UNSIGNED DEFAULT 0,
    total_attendees INT UNSIGNED DEFAULT 0,
    total_volunteers INT UNSIGNED DEFAULT 0,
    total_speakers INT UNSIGNED DEFAULT 0,
    total_checked_in INT UNSIGNED DEFAULT 0,
    total_attended INT UNSIGNED DEFAULT 0,
    total_pending INT UNSIGNED DEFAULT 0,
    total_rejected INT UNSIGNED DEFAULT 0,
    total_no_show INT UNSIGNED DEFAULT 0,
    attendance_rate DECIMAL(5,2) DEFAULT 0.00,
    generated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_event (event_id),
    CONSTRAINT fk_rep_event FOREIGN KEY (event_id) 
        REFERENCES events(id) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ===================================================================
-- VIEW: Event Summary
-- UPDATED: Reflects new status values
-- Provides comprehensive event statistics
-- ===================================================================
CREATE OR REPLACE VIEW vw_event_summary AS
SELECT 
    e.id AS event_id,
    e.name AS event_name,
    e.start_datetime,
    e.end_datetime,
    e.venue,
    e.type,
    
    -- Total Registered (all active statuses)
    COALESCE(SUM(
        CASE WHEN r.status IN ('Approved', 'Checked-in', 'Attended') 
        THEN 1 ELSE 0 END
    ), 0) AS total_registered,
    
    -- Attendees (role-specific)
    COALESCE(SUM(
        CASE WHEN r.role = 'attendee' 
             AND r.status IN ('Approved', 'Checked-in', 'Attended') 
        THEN 1 ELSE 0 END
    ), 0) AS total_attendees,
    
    -- Volunteers (role-specific)
    COALESCE(SUM(
        CASE WHEN r.role = 'volunteer' 
             AND r.status IN ('Approved', 'Checked-in', 'Attended') 
        THEN 1 ELSE 0 END
    ), 0) AS total_volunteers,
    
    -- Speakers (role-specific)
    COALESCE(SUM(
        CASE WHEN r.role = 'speaker' 
             AND r.status IN ('Approved', 'Checked-in', 'Attended') 
        THEN 1 ELSE 0 END
    ), 0) AS total_speakers,
    
    -- Checked-in (currently at event)
    COALESCE(SUM(
        CASE WHEN r.status = 'Checked-in' 
        THEN 1 ELSE 0 END
    ), 0) AS total_checked_in,
    
    -- Fully Attended (completed attendance)
    COALESCE(SUM(
        CASE WHEN r.status = 'Attended' 
        THEN 1 ELSE 0 END
    ), 0) AS total_attended,
    
    -- Pending Approvals
    COALESCE(SUM(
        CASE WHEN r.status = 'Pending' 
        THEN 1 ELSE 0 END
    ), 0) AS total_pending,
    
    -- Rejected
    COALESCE(SUM(
        CASE WHEN r.status = 'Rejected' 
        THEN 1 ELSE 0 END
    ), 0) AS total_rejected,
    
    -- No Show (Didn't Attend)
    COALESCE(SUM(
        CASE WHEN r.status = 'Didn''t Attend' 
        THEN 1 ELSE 0 END
    ), 0) AS total_no_show,
    
    -- Attendance Rate (Attended / Registered)
    CASE 
        WHEN SUM(CASE WHEN r.status IN ('Approved', 'Checked-in', 'Attended') THEN 1 ELSE 0 END) > 0
        THEN ROUND(
            (SUM(CASE WHEN r.status = 'Attended' THEN 1 ELSE 0 END) * 100.0) / 
            SUM(CASE WHEN r.status IN ('Approved', 'Checked-in', 'Attended') THEN 1 ELSE 0 END),
            2
        )
        ELSE 0
    END AS attendance_rate

FROM events e
LEFT JOIN registrations r ON r.event_id = e.id
GROUP BY e.id, e.name, e.start_datetime, e.end_datetime, e.venue, e.type;

-- ===================================================================
-- STORED PROCEDURE: Generate Event Report
-- UPDATED: Calculates all new status-based metrics
-- ===================================================================
DELIMITER $$

CREATE PROCEDURE generate_report_for_event(IN p_event_id INT)
BEGIN
    DECLARE v_registered INT DEFAULT 0;
    DECLARE v_attendees INT DEFAULT 0;
    DECLARE v_volunteers INT DEFAULT 0;
    DECLARE v_speakers INT DEFAULT 0;
    DECLARE v_checked_in INT DEFAULT 0;
    DECLARE v_attended INT DEFAULT 0;
    DECLARE v_pending INT DEFAULT 0;
    DECLARE v_rejected INT DEFAULT 0;
    DECLARE v_no_show INT DEFAULT 0;
    DECLARE v_rate DECIMAL(5,2) DEFAULT 0.00;
    
    -- Calculate total registered (Approved, Checked-in, Attended)
    SELECT COUNT(*) INTO v_registered
    FROM registrations
    WHERE event_id = p_event_id 
      AND status IN ('Approved', 'Checked-in', 'Attended');
    
    -- Calculate attendees by role
    SELECT 
        SUM(CASE WHEN role = 'attendee' THEN 1 ELSE 0 END),
        SUM(CASE WHEN role = 'volunteer' THEN 1 ELSE 0 END),
        SUM(CASE WHEN role = 'speaker' THEN 1 ELSE 0 END)
    INTO v_attendees, v_volunteers, v_speakers
    FROM registrations
    WHERE event_id = p_event_id 
      AND status IN ('Approved', 'Checked-in', 'Attended');
    
    -- Calculate status counts
    SELECT 
        SUM(CASE WHEN status = 'Checked-in' THEN 1 ELSE 0 END),
        SUM(CASE WHEN status = 'Attended' THEN 1 ELSE 0 END),
        SUM(CASE WHEN status = 'Pending' THEN 1 ELSE 0 END),
        SUM(CASE WHEN status = 'Rejected' THEN 1 ELSE 0 END),
        SUM(CASE WHEN status = 'Didn''t Attend' THEN 1 ELSE 0 END)
    INTO v_checked_in, v_attended, v_pending, v_rejected, v_no_show
    FROM registrations
    WHERE event_id = p_event_id;
    
    -- Calculate attendance rate
    IF v_registered > 0 THEN
        SET v_rate = (v_attended * 100.0) / v_registered;
    ELSE
        SET v_rate = 0;
    END IF;
    
    -- Insert or update report
    INSERT INTO reports (
        event_id, 
        total_registered,
        total_attendees, 
        total_volunteers, 
        total_speakers,
        total_checked_in,
        total_attended,
        total_pending,
        total_rejected,
        total_no_show,
        attendance_rate
    ) VALUES (
        p_event_id, 
        v_registered,
        v_attendees, 
        v_volunteers, 
        v_speakers,
        v_checked_in,
        v_attended,
        v_pending,
        v_rejected,
        v_no_show,
        v_rate
    )
    ON DUPLICATE KEY UPDATE
        total_registered = v_registered,
        total_attendees = v_attendees,
        total_volunteers = v_volunteers,
        total_speakers = v_speakers,
        total_checked_in = v_checked_in,
        total_attended = v_attended,
        total_pending = v_pending,
        total_rejected = v_rejected,
        total_no_show = v_no_show,
        attendance_rate = v_rate,
        generated_at = CURRENT_TIMESTAMP;
        
END$$

DELIMITER ;

-- ===================================================================
-- STORED PROCEDURE: Update Registration Status on Event End
-- Automatically marks approved registrations as "Didn't Attend" 
-- when event ends without check-in
-- ===================================================================
DELIMITER $$

CREATE PROCEDURE update_no_show_for_ended_events()
BEGIN
    UPDATE registrations r
    INNER JOIN events e ON r.event_id = e.id
    SET r.status = 'Didn''t Attend'
    WHERE e.end_datetime < NOW()
      AND r.status = 'Approved'
      AND NOT EXISTS (
          SELECT 1 FROM attendance a 
          WHERE a.registration_id = r.id
      );
END$$

DELIMITER ;

-- ===================================================================
-- SAMPLE DATA
-- ===================================================================

-- Insert Admin User
INSERT INTO users (first_name, last_name, email, password, system_role, address, contact_number) 
VALUES ('Admin', 'User', 'admin@bems.com', 'admin123', 'admin', 'Barangay Hall', '09000000000');

-- Insert Sample Users
INSERT INTO users (first_name, last_name, email, password, system_role, address, contact_number) 
VALUES 
('Juan', 'Dela Cruz', 'juan@gmail.com', 'password123', 'user', '123 Main St', '09111111111'),
('Maria', 'Santos', 'maria@gmail.com', 'password123', 'user', '456 Oak Ave', '09222222222'),
('Jose', 'Reyes', 'jose@gmail.com', 'password123', 'user', '789 Pine Rd', '09333333333');

-- Insert Sample Events
INSERT INTO events (name, description, start_datetime, end_datetime, venue, type, organizer) 
VALUES 
(
    'Community Clean-up Drive',
    'Monthly community clean-up around the barangay area.',
    '2025-02-15 07:00:00',
    '2025-02-15 11:00:00',
    'Barangay Plaza',
    'Community Service',
    'Barangay Council'
),
(
    'Health and Wellness Seminar',
    'Free health screening and wellness seminar for residents.',
    '2025-02-20 09:00:00',
    '2025-02-20 15:00:00',
    'Barangay Hall',
    'Health',
    'Barangay Health Center'
),
(
    'Youth Leadership Training',
    'Leadership development program for barangay youth.',
    '2025-03-01 08:00:00',
    '2025-03-03 17:00:00',
    'Community Center',
    'Training',
    'SK Federation'
);

-- Insert Sample Registrations with different statuses
INSERT INTO registrations (event_id, user_id, role, status, qr_code) 
VALUES 
-- Event 1: Clean-up Drive
(1, 2, 'attendee', 'Approved', 'QR_EVENT1_USER2'),
(1, 3, 'volunteer', 'Checked-in', 'QR_EVENT1_USER3'),
(1, 4, 'attendee', 'Pending', NULL),

-- Event 2: Health Seminar
(2, 2, 'attendee', 'Attended', 'QR_EVENT2_USER2'),
(2, 3, 'attendee', 'Approved', 'QR_EVENT2_USER3'),
(2, 4, 'volunteer', 'Rejected', NULL),

-- Event 3: Youth Training
(3, 2, 'speaker', 'Approved', 'QR_EVENT3_USER2'),
(3, 3, 'attendee', 'Pending', NULL),
(3, 4, 'volunteer', 'Approved', 'QR_EVENT3_USER4');

-- Insert Sample Attendance Records
INSERT INTO attendance (registration_id, check_in_time, check_out_time)
VALUES
(2, '2025-02-15 07:15:00', NULL), -- Checked-in only
(4, '2025-02-20 09:10:00', '2025-02-20 14:50:00'); -- Full attendance

-- Generate Initial Reports
CALL generate_report_for_event(1);
CALL generate_report_for_event(2);
CALL generate_report_for_event(3);

-- ===================================================================
-- USEFUL QUERIES FOR REPORTS
-- ===================================================================

-- Query 1: Get all events with statistics
SELECT * FROM vw_event_summary ORDER BY start_datetime DESC;

-- Query 2: Get detailed attendance breakdown for specific event
SELECT 
    e.name AS event_name,
    COUNT(DISTINCT r.id) AS total_registrations,
    SUM(CASE WHEN r.status = 'Pending' THEN 1 ELSE 0 END) AS pending,
    SUM(CASE WHEN r.status = 'Approved' THEN 1 ELSE 0 END) AS approved,
    SUM(CASE WHEN r.status = 'Checked-in' THEN 1 ELSE 0 END) AS checked_in,
    SUM(CASE WHEN r.status = 'Attended' THEN 1 ELSE 0 END) AS attended,
    SUM(CASE WHEN r.status = 'Rejected' THEN 1 ELSE 0 END) AS rejected,
    SUM(CASE WHEN r.status = 'Didn''t Attend' THEN 1 ELSE 0 END) AS no_show
FROM events e
LEFT JOIN registrations r ON e.id = r.event_id
WHERE e.id = 1
GROUP BY e.id, e.name;

-- Query 3: Overall system statistics
SELECT 
    (SELECT COUNT(*) FROM events) AS total_events,
    (SELECT COUNT(*) FROM users WHERE system_role = 'user') AS total_users,
    (SELECT COUNT(*) FROM registrations WHERE status IN ('Approved', 'Checked-in', 'Attended')) AS total_registered,
    (SELECT COUNT(*) FROM registrations WHERE status = 'Attended') AS total_attended,
    (SELECT COUNT(*) FROM registrations WHERE status = 'Checked-in') AS total_checked_in,
    (SELECT COUNT(*) FROM registrations WHERE status = 'Pending') AS total_pending,
    ROUND(
        (SELECT COUNT(*) FROM registrations WHERE status = 'Attended') * 100.0 / 
        NULLIF((SELECT COUNT(*) FROM registrations WHERE status IN ('Approved', 'Checked-in', 'Attended')), 0),
        2
    ) AS overall_attendance_rate;

-- Query 4: Events by status breakdown
SELECT 
    e.id,
    e.name,
    e.start_datetime,
    SUM(CASE WHEN r.status IN ('Approved', 'Checked-in', 'Attended') THEN 1 ELSE 0 END) AS registered,
    SUM(CASE WHEN r.status = 'Attended' THEN 1 ELSE 0 END) AS attended,
    SUM(CASE WHEN r.status = 'Checked-in' THEN 1 ELSE 0 END) AS currently_at_event,
    SUM(CASE WHEN r.status = 'Pending' THEN 1 ELSE 0 END) AS pending,
    CASE 
        WHEN SUM(CASE WHEN r.status IN ('Approved', 'Checked-in', 'Attended') THEN 1 ELSE 0 END) > 0
        THEN ROUND(
            SUM(CASE WHEN r.status = 'Attended' THEN 1 ELSE 0 END) * 100.0 / 
            SUM(CASE WHEN r.status IN ('Approved', 'Checked-in', 'Attended') THEN 1 ELSE 0 END),
            2
        )
        ELSE 0
    END AS attendance_rate
FROM events e
LEFT JOIN registrations r ON e.id = r.event_id
GROUP BY e.id, e.name, e.start_datetime
ORDER BY e.start_datetime DESC;

-- ===================================================================
-- MAINTENANCE QUERIES
-- ===================================================================

-- Update no-shows for ended events (run periodically)
CALL update_no_show_for_ended_events();

-- Regenerate all reports
DELIMITER $$
CREATE PROCEDURE regenerate_all_reports()
BEGIN
    DECLARE done INT DEFAULT FALSE;
    DECLARE v_event_id INT;
    DECLARE event_cursor CURSOR FOR SELECT id FROM events;
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;
    
    OPEN event_cursor;
    
    read_loop: LOOP
        FETCH event_cursor INTO v_event_id;
        IF done THEN
            LEAVE read_loop;
        END IF;
        CALL generate_report_for_event(v_event_id);
    END LOOP;
    
    CLOSE event_cursor;
END$$
DELIMITER ;

-- ===================================================================
-- INDEXES FOR PERFORMANCE
-- ===================================================================

-- Additional composite indexes for common queries
CREATE INDEX idx_reg_event_status_role ON registrations(event_id, status, role);
CREATE INDEX idx_reg_user_event ON registrations(user_id, event_id);
CREATE INDEX idx_events_datetime_range ON events(start_datetime, end_datetime);

-- ===================================================================
-- END OF SCHEMA
-- ===================================================================
