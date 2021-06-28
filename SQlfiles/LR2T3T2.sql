create database lab2_3_2;
use lab2_3_2;

create table workbook
(
	surname enum('A', 'B'),
    course varchar(20),
    workbook varchar(20)
);

insert into workbook values
('A', 'Информатика', 'Информатика'),
('A', 'Сети ЭВМ', 'Информатика'),
('A', 'Информатика', 'Сети ЭВМ'),
('A', 'Сети ЭВМ', 'Сети ЭВМ'),
('B', 'Программирование', 'Программирование'),
('B', 'Программирование', 'Теория алгоритмов');

select * from workbook;