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

-- USERS TABLE
CREATE TABLE users (
  id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(255),
  email VARCHAR(255) NOT NULL UNIQUE,
  password VARCHAR(255) NOT NULL,
  role VARCHAR(50),
  address TEXT,
  contact_number VARCHAR(50),
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- EVENTS TABLE
CREATE TABLE events (
  id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  description TEXT,
  date DATE,
  time TIME,
  venue VARCHAR(255),
  type VARCHAR(100),
  organizer VARCHAR(255),
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- REGISTRATIONS TABLE
CREATE TABLE registrations (
  id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
  event_id INT UNSIGNED NOT NULL,
  user_id INT UNSIGNED NOT NULL,
  role VARCHAR(50),
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
  check_in_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_att_reg FOREIGN KEY (registration_id)
    REFERENCES registrations(id) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- REPORTS TABLE
CREATE TABLE reports (
  id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
  event_id INT UNSIGNED NOT NULL,
  total_attendees INT UNSIGNED DEFAULT 0,
  total_volunteers INT UNSIGNED DEFAULT 0,
  generated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_rep_event FOREIGN KEY (event_id)
    REFERENCES events(id) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- VIEW
CREATE OR REPLACE VIEW vw_event_summary AS
SELECT
  e.id AS event_id,
  e.name AS event_name,
  e.date,
  e.time,
  COALESCE(SUM(CASE WHEN r.role = 'attendee' AND r.status = 'approved' THEN 1 ELSE 0 END), 0) AS approved_attendees,
  COALESCE(SUM(CASE WHEN r.role = 'volunteer' AND r.status = 'approved' THEN 1 ELSE 0 END), 0) AS approved_volunteers,
  COALESCE(COUNT(a.id), 0) AS total_checked_in
FROM events e
LEFT JOIN registrations r ON r.event_id = e.id
LEFT JOIN attendance a ON a.registration_id = r.id
GROUP BY e.id, e.name, e.date, e.time;

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

  IF v_att IS NULL THEN SET v_att = 0; END IF;
  IF v_vol IS NULL THEN SET v_vol = 0; END IF;

  INSERT INTO reports (event_id, total_attendees, total_volunteers)
  VALUES (p_event_id, v_att, v_vol);
END$$
DELIMITER ;

-- SAMPLE DATA
INSERT INTO users (name, email, password, role, address, contact_number) VALUES
('Admin User', 'admin@bems.com', 'admin123', 'admin', 'Barangay Hall', '09000000000'),
('Juan Dela Cruz', 'juan@gmail.com', 'juan123', 'resident', 'Purok 1', '09111111111'),
('Maria Santos', 'maria@gmail.com', 'maria123', 'resident', 'Purok 2', '09222222222');

INSERT INTO events (name, description, date, time, venue, type, organizer) VALUES
('Clean-up Drive', 'Community clean-up around the barangay area.', '2025-11-10', '07:30:00', 'Barangay Plaza', 'Community Service', 'Barangay Council'),
('Tree Planting', 'Planting trees to promote a greener barangay.', '2025-11-20', '08:00:00', 'Barangay Park', 'Environmental', 'Barangay Youth');

INSERT INTO registrations (event_id, user_id, role, status, qr_code) VALUES
(1, 2, 'volunteer', 'approved', 'QR001'),
(1, 3, 'attendee', 'approved', 'QR002'),
(2, 2, 'attendee', 'pending', NULL);

INSERT INTO attendance (registration_id) VALUES
(1),
(2);

CALL generate_report_for_event(1);
