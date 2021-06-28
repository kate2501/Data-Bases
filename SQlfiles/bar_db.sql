create database bar;
use bar;

create table products
(
product_id integer primary key auto_increment,
product_name varchar(50),
product_type varchar(50),
product_price float
);

create table contracts
(
contract_id integer primary key auto_increment,
contract_start_date date,
contract_end_date date,
salary integer
);

create table staff
(
employee_id integer primary key auto_increment,
employee_name varchar(50) not null,
employee_surname varchar(50) not null,
employee_position enum("barman", "cook"),
constraint cn1 foreign key(employee_id) references contracts(contract_id)
);

create table sells
(
sell_id integer primary key,
sell_date datetime not null,
barman_id integer not null,
sell_amount float not null,
constraint cn2 foreign key(barman_id) references staff(employee_id)
);

create table products_sells
(
sell_id integer not null,
product_id integer not null,
quantity integer,
primary key(sell_id, product_id),
constraint cn3 foreign key(sell_id) references sells(sell_id),
constraint cn4 foreign key(product_id) references products(product_id)
);


insert into products
(product_name, product_type, product_price)
values
("Margarita", "cocktail", 100),
("Alivaria", "beer", 80),
("Kadarka", "wine", 100), 
("Bonaqua", "water", 30);

select * from products;

insert into contracts
(contract_start_date, contract_end_date, salary)
values
("2019-07-20", "2020-09-09", 500);

select * from contracts;

insert into staff
(employee_name, employee_surname, employee_position)
values
("Ivan", "Ivanov", "barman");

select * from staff;

insert into contracts
(contract_start_date, contract_end_date, salary)
values
("2019-10-10", "2020-10-10", 700),
("2019-05-12", "2021-06-21", 600);

insert into staff
(employee_name, employee_surname, employee_position)
values
("Denis", "Denisov", "cook"),
("Petr", "Petrov", "barman");

select * from staff;
select * from contracts;

insert into sells
(sell_id, sell_date, barman_id, sell_amount)
values
(1, "2020-05-05", 1, 5),
(2, "2020-06-06", 1, 3),
(3, "2020-07-07", 3, 1),
(4, "2020-08-08", 3, 2);

select * from sells;

insert into products_sells
(sell_id, product_id, quantity)
values
(1, 2, 3),
(1, 1, 2),
(2, 3, 3),
(3, 4, 1),
(4, 2, 2);

select* from products_sells;

set sql_safe_updates = 0;

update sells 
set sell_date=date_add(sell_date, interval (((5-DAYOFWEEK(sell_date)) % 7)+7) % 7 day);

select* from sells;

alter table staff
modify employee_position set("barman", "cook");

select*from staff;

update staff
set employee_position="barman,cook"
where employee_id=1;


#update staff
#set employee_position=concat_ws(',', 'nobody',employee_position)
#where employee_id=1;