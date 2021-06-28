create database lab2_3_3;
use lab2_3_3;

create table court
(
	courtNumber enum('1', '2'),
    startTime time,
    finishTime time,
    tariff varchar(10)
);

insert into court values
('1', '09:30:00', '10:30:00', 'Бережливый'),
('1', '11:00:00', '12:00:00', 'Бережливый'),
('1', '14:00:00', '15:30:00', 'Стандарт'),
('2', '10:00:00', '11:30:00', 'Премиум-B'),
('2', '11:30:00', '13:30:00', 'Премиум-B'),
('2', '15:00:00', '16:30:00', 'Премиум-A');

select * from court;