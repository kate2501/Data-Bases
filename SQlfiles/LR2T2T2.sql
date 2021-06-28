create database lab2_2_2;
use lab2_2_2;

create table kids
(
	EmpID int primary key,
    firstName varchar(4),
    secondName varchar(5),
    childName varchar(20),
    childBirth varchar(50)
);

insert into kids values
(1001, 'Jane', 'Doe', 'Mary, Sam', '1992-01-01, 1994-05-15'),
(1002, 'John', 'Doe', 'Mary, Sam', '1992-01-01, 1994-05-15'),
(1003, 'Jane', 'Smith', 'John, Pat, Lee, Mary', '1994-10-05, 1990-10-12, 1996-06-06, 1994-08-21'),
(1004, 'John', 'Smith', 'Michael', '1996-07-04'),
(1005, 'Jane', 'Jones', 'Edward, Martha', '1995-10-21, 1989-10-15');

select * from kids;