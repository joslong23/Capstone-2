-- Put steps here to set up your database in a default good state for testing
DELETE FROM reservation
DELETE FROM space
DELETE FROM category_venue
DELETE FROM venue
DELETE FROM category
DELETE FROM city
DELETE FROM state

INSERT INTO state (abbreviation , name)
VALUES ('NM', 'New Mexico')

INSERT INTO state (abbreviation , name)
VALUES ('NM', 'New Mexico')
SET IDENTITY_INSERT city ON

INSERT INTO city (id, name, state_abbreviation)
VALUES (1 , 'Albequreque' , 'NM')
SET IDENTITY_INSERT city OFF

SET IDENTITY_INSERT venue ON
INSERT INTO venue (id, name, city_id ,description)
VALUES (1, 'Where The' , 1, 'Proud to be in that number')
SET IDENTITY_INSERT venue OFF

SET IDENTITY_INSERT space ON
INSERT INTO space (id, venue_id, name , is_accessible , open_from, open_to, daily_rate, max_occupancy)
VALUES (1, 1, 'Marching in', 1, 10, 4, 15, 20);
SET IDENTITY_INSERT space OFF

SET IDENTITY_INSERT category ON
INSERT INTO category (id, name) VALUES (1, 'Family Friendly');
SET IDENTITY_INSERT category OFF

INSERT INTO category_venue (venue_id, category_id) VALUES (1, 1);

SET IDENTITY_INSERT reservation ON
INSERT INTO reservation(reservation_id , space_id ,number_of_attendees , start_date, end_date, reserved_for)
VALUES (1, 1, 10 , '10/15/2021' , '10/20/2010' , 'The Saints');
SET IDENTITY_INSERT reservation OFF

