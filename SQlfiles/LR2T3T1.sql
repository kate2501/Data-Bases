create database lab2_3_1;
use lab2_3_1;

create table movie
(
	title varchar(15),
    star varchar(20),
    producer varchar(20)
);

insert into movie values
('Great Film', 'Lovely Lady', 'Money Bags'),
('Great Film', 'Handsome Man', 'Money Bags'),
('Great Film', 'Lovely Lady', 'Helen Pursestrings'),
('Great Film', 'Handsome Man', 'Helen Pursestrings'),
('Boring Movie', 'Lovely Lady', 'Helen Pursestrings'),
('Boring Movie', 'Precocious Child', 'Helen Pursestrings');

select * from movie;