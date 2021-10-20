DELETE FROM project_employee
DELETE FROM employee
DELETE FROM department
DELETE FROM project

SET IDENTITY_INSERT department ON

INSERT INTO department (department_id, name)
VALUES (1, 'Apeture')

SET IDENTITY_INSERT department OFF

SET IDENTITY_INSERT project ON

INSERT INTO project (project_id, name, from_date, to_date)
VALUES (1, 'Capstone', '2021-10-18', '2021-10-22')

SET IDENTITY_INSERT project OFF

SET IDENTITY_INSERT employee ON

INSERT INTO employee (employee_id, department_id, first_name, last_name, job_title, birth_date, hire_date)
VALUES (1, 1, 'John', 'Doe', 'Unknown', '1969-01-01', '2020-04-01')

SET IDENTITY_INSERT employee OFF

INSERT INTO project_employee (project_id, employee_id)
VALUES (1, 1)

