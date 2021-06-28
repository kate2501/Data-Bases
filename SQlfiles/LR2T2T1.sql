create database lab2_2_1;
use lab2_2_1;

create table bookStore
(
	author1 varchar(50) not null,
    author2 varchar(50),
    title varchar(20),
    ISBN char(10) not null,
    price double not null,
    customerName varchar(20) not null,
    customerAddress varchar(50),
    purchaseDate date
);

insert into bookStore values
('David Sklar', 'Adam Trachtenberg', 'PHP Cookbook', '0596101015', 44.99, 'Emme Brown', '1565 Rainbow Road, Los Angeles, CA 90014', '2009-03-03'),
('Danny Goodman', null, 'Dynamic HTML', '0596527403', 59.99, 'Darren Ryder', '4758 Emily Drive, Richmond, VA 23219', '2008-12-19'),
('Hudh E. Williams', 'David Lane', 'PHP and MySQL', '0596005436', 44.95, 'Earl B. Thurston', '862 Gregory Lane, Frankfort, KY 40601', '2009-06-22'),
('David Sklar', 'Adam Trachtenberg', 'PHP Cookbook', '0596101015', 44.99, 'Darren Ryder', '4758 Emily Drive, Richmond, VA 23219', '2008-12-19'),
('Rasmus Lerdorf', 'Kevin Tatroe & Peter MacIntyre', 'Programming PHP', '0596006815', 39.99, 'David Miller', '3647 Cedar Lane, Waltham, MA 02154', '2009-01-16');

select * from bookStore;