INSERT INTO Agencies (Name, Address, Email, Phone)
VALUES
  ('Agency 1', '123 Main Street, City 1', 'agency1@example.com', '+1234567890'),
  ('Agency 2', '456 Elm Avenue, City 2', 'agency2@example.com', '+2345678901'),
  ('Agency 3', '789 Oak Boulevard, City 3', 'agency3@example.com', '+3456789012'),
  ('Agency 4', '101 Pine Road, City 4', 'agency4@example.com', '+4567890123'),
  ('Agency 5', '202 Cedar Lane, City 5', 'agency5@example.com', '+5678901234'),
  ('Agency 6', '303 Maple Drive, City 6', 'agency6@example.com', '+6789012345'),
  ('Agency 7', '404 Birch Street, City 7', 'agency7@example.com', '+7890123456'),
  ('Agency 8', '505 Spruce Avenue, City 8', 'agency8@example.com', '+8901234567'),
  ('Agency 9', '606 Willow Road, City 9', 'agency9@example.com', '+9012345678'),
  ('Agency 10', '707 Redwood Court, City 10', 'agency10@example.com', '+0123456789');

INSERT INTO Customers (FirstName, LastName, Address, Email, Phone)
VALUES
  ('John', 'Doe', '123 Main Street, City 1', 'john@example.com', '+1234567890'),
  ('Jane', 'Smith', '456 Elm Avenue, City 2', 'jane@example.com', '+2345678901'),
  ('Michael', 'Johnson', '789 Oak Boulevard, City 3', 'michael@example.com', '+3456789012'),
  ('Emily', 'Brown', '101 Pine Road, City 4', 'emily@example.com', '+4567890123'),
  ('David', 'Davis', '202 Cedar Lane, City 5', 'david@example.com', '+5678901234'),
  ('Sarah', 'Wilson', '303 Maple Drive, City 6', 'sarah@example.com', '+6789012345'),
  ('Christopher', 'Lee', '404 Birch Street, City 7', 'christopher@example.com', '+7890123456'),
  ('Jennifer', 'Martinez', '505 Spruce Avenue, City 8', 'jennifer@example.com', '+8901234567'),
  ('William', 'White', '606 Willow Road, City 9', 'william@example.com', '+9012345678'),
  ('Olivia', 'Taylor', '707 Redwood Court, City 10', 'olivia@example.com', '+0123456789');

INSERT INTO OffDays (AgencyId, AppointmentDate, OffDayStatus) VALUES
(1, DATE_ADD(CURRENT_DATE, INTERVAL 1 + (4 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 2 + (5 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 3 + (6 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 7 + (4 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 8 + (5 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 9 + (6 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 14 + (4 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 15 + (5 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 16 + (6 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 21 + (4 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 22 + (5 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 23 + (6 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 28 + (4 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 29 + (5 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 30 + (6 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 35 + (4 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 36 + (5 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 37 + (6 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 42 + (4 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 43 + (5 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1),
(1, DATE_ADD(CURRENT_DATE, INTERVAL 44 + (6 - DAYOFWEEK(CURRENT_DATE)) % 7 DAY), 1);

INSERT INTO MaximumAppointments (AgencyId, AppointmentDate, MaximumAppointmentNumber, MaximumAppointmentStatus) VALUES
(1, '2023-10-01', 10, 1),
(1, '2023-10-02', 10, 1),
(1, '2023-10-03', 10, 1),
(1, '2023-10-04', 10, 1),
(1, '2023-10-05', 10, 1),
(1, '2023-10-06', 10, 1),
(1, '2023-10-07', 10, 1),
(1, '2023-10-08', 10, 1),
(1, '2023-10-09', 10, 1),
(1, '2023-10-10', 10, 1),
(1, '2023-10-11', 10, 1),
(1, '2023-10-12', 10, 1),
(1, '2023-10-13', 10, 1),
(1, '2023-10-14', 10, 1),
(1, '2023-10-15', 10, 1),
(1, '2023-10-16', 10, 1),
(1, '2023-10-17', 10, 1),
(1, '2023-10-18', 10, 1),
(1, '2023-10-19', 10, 1),
(1, '2023-10-20', 10, 1);

-- Generate dummy data for Appointments with AgencyId = 1, GUID tokens, and specified date range
INSERT INTO Appointments (CustomerId, AgencyId, Token, AppointmentDate)
VALUES
(1, 1, UUID(), '2023-10-01'),
(2, 1, UUID(), '2023-10-02'),
(3, 1, UUID(), '2023-10-03'),
(4, 1, UUID(), '2023-10-04'),
(1, 1, UUID(), '2023-10-05'),
(2, 1, UUID(), '2023-10-06'),
(3, 1, UUID(), '2023-10-01'),
(4, 1, UUID(), '2023-10-02'),
(1, 1, UUID(), '2023-10-03'),
(2, 1, UUID(), '2023-10-04'),
(3, 1, UUID(), '2023-10-05'),
(4, 1, UUID(), '2023-10-06'),
(1, 1, UUID(), '2023-10-01'),
(2, 1, UUID(), '2023-10-02'),
(3, 1, UUID(), '2023-10-03'),
(4, 1, UUID(), '2023-10-04'),
(1, 1, UUID(), '2023-10-05'),
(2, 1, UUID(), '2023-10-06'),
(3, 1, UUID(), '2023-10-01'),
(4, 1, UUID(), '2023-10-02');
