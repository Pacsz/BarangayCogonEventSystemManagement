-- Barangay Event Management System (BEMS)
-- Database name: bems_db

CREATE DATABASE IF NOT EXISTS bems_db;
USE bems_db;

SET FOREIGN_KEY_CHECKS = 0;
DROP TABLE IF EXISTS attendance;
DROP TABLE IF EXISTS reports;
DROP TABLE IF EXISTS registrations;
DROP TABLE IF EXISTS events;
DROP TABLE IF EXISTS users;
SET FOREIGN_KEY_CHECKS = 1;

-- USERS TABLE (SYSTEM ROLE ADDED)
CREATE TABLE users (
  id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  email VARCHAR(255) NOT NULL UNIQUE,
  password VARCHAR(255) NOT NULL,
  system_role ENUM('admin','user') NOT NULL DEFAULT 'user',
  address TEXT,
  contact_number VARCHAR(50),
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- EVENTS TABLE (USING DATETIME)
CREATE TABLE events (
  id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  description TEXT,
  start_datetime DATETIME NOT NULL,
  end_datetime DATETIME NOT NULL,
  venue VARCHAR(255),
  type VARCHAR(100),
  organizer VARCHAR(255),
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- REGISTRATIONS TABLE (ROLE IS HERE)
CREATE TABLE registrations (
  id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
  event_id INT UNSIGNED NOT NULL,
  user_id INT UNSIGNED NOT NULL,
  role ENUM('attendee','volunteer','speaker') NOT NULL,
  status VARCHAR(50) DEFAULT 'pending',
  qr_code VARCHAR(255),
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  UNIQUE KEY uniq_event_user (event_id, user_id),
  CONSTRAINT fk_reg_event FOREIGN KEY (event_id)
    REFERENCES events(id) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT fk_reg_user FOREIGN KEY (user_id)
    REFERENCES users(id) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ATTENDANCE TABLE
CREATE TABLE attendance (
  id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
  registration_id INT UNSIGNED NOT NULL,
  check_in_time DATETIME DEFAULT CURRENT_TIMESTAMP,
  check_out_time DATETIME NULL,
  CONSTRAINT fk_att_reg FOREIGN KEY (registration_id)
    REFERENCES registrations(id) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- REPORTS TABLE
CREATE TABLE reports (
  id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
  event_id INT UNSIGNED NOT NULL,
  total_attendees INT UNSIGNED DEFAULT 0,
  total_volunteers INT UNSIGNED DEFAULT 0,
  generated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_rep_event FOREIGN KEY (event_id)
    REFERENCES events(id) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- VIEW (UPDATED FOR DATETIME)
CREATE OR REPLACE VIEW vw_event_summary AS
SELECT
  e.id AS event_id,
  e.name AS event_name,
  e.start_datetime,
  e.end_datetime,
  COALESCE(SUM(
    CASE 
      WHEN r.role = 'attendee' AND r.status = 'approved' 
      THEN 1 ELSE 0 
    END
  ), 0) AS approved_attendees,
  COALESCE(SUM(
    CASE 
      WHEN r.role = 'volunteer' AND r.status = 'approved' 
      THEN 1 ELSE 0 
    END
  ), 0) AS approved_volunteers,
  COALESCE(COUNT(a.id), 0) AS total_checked_in
FROM events e
LEFT JOIN registrations r ON r.event_id = e.id
LEFT JOIN attendance a ON a.registration_id = r.id
GROUP BY e.id, e.name, e.start_datetime, e.end_datetime;

-- PROCEDURE
DELIMITER $$
CREATE PROCEDURE generate_report_for_event(IN p_event_id INT)
BEGIN
  DECLARE v_att INT DEFAULT 0;
  DECLARE v_vol INT DEFAULT 0;

  SELECT
    SUM(CASE WHEN role = 'attendee' AND status = 'approved' THEN 1 ELSE 0 END),
    SUM(CASE WHEN role = 'volunteer' AND status = 'approved' THEN 1 ELSE 0 END)
  INTO v_att, v_vol
  FROM registrations
  WHERE event_id = p_event_id;

  SET v_att = IFNULL(v_att, 0);
  SET v_vol = IFNULL(v_vol, 0);

  INSERT INTO reports (event_id, total_attendees, total_volunteers)
  VALUES (p_event_id, v_att, v_vol);
END$$
DELIMITER ;

-- SAMPLE USERS
INSERT INTO users
(name, email, password, system_role, address, contact_number)
VALUES
('Admin User', 'admin@bems.com', 'admin123', 'admin', 'Barangay Hall', '09000000000');

-- SAMPLE EVENT
INSERT INTO events 
(name, description, start_datetime, end_datetime, venue, type, organizer)
VALUES
(
  'Clean-up Drive',
  'Community clean-up around the barangay area.',
  '2025-11-10 07:30:00',
  '2025-11-10 11:30:00',
  'Barangay Plaza',
  'Community Service',
  'Barangay Council'
);

CALL generate_report_for_event(1);
