create database coffee_shops;

use coffee_shops;

create table coffee_shops_networks
(
network_id integer primary key auto_increment,
director varchar(50) not null,
sub_director varchar(50),
main_office_address varchar(50)
);

create table coffee_shop
(
coffee_shop_id integer primary key,
coffee_shop_man varchar(50) not null,
coffee_shop_address varchar(50) not null,
network_id integer,
constraint cn1 foreign key(network_id) references coffee_shops_networks(network_id)
);

create table staff
(
employee_id integer auto_increment,
employee_name varchar(45) not null,
employee_surname varchar(50) not null,
employee_post varchar(50) not null,
coffee_shop_id integer,
primary key(employee_id,coffee_shop_id),
constraint cn2 foreign key(coffee_shop_id) references coffee_shop(coffee_shop_id)
);

create table contracts
(
contract_id integer primary key auto_increment,
contract_start_date date not null,
contract_end_date date not null,
contract_salary float not null,
constraint cn3 foreign key(contract_id) references staff(employee_id)
);

create table storages
(
storage_id integer auto_increment,
storage_address varchar(50),
primary key(storage_id)
);

create table products
(
product_id integer primary key auto_increment,
product_name varchar(50) not null,
product_type varchar(50),
product_producer varchar(50)
);

create table storages_products
(
storage_id integer not null,
product_id integer not null,
quantity integer,
primary key(storage_id, product_id),
constraint cn6 foreign key(storage_id) references storages(storage_id),
constraint cn7 foreign key(product_id) references products(product_id)
);

create table sells
(
sell_id integer,
sell_date datetime not null,
sell_amount integer not null,
employee_id integer not null,
primary key(sell_id,employee_id),
constraint cn12 foreign key(employee_id) references staff(employee_id)
);

create table sells_products
(
sell_id integer not null, 
product_id integer not null, 
quantity integer,
primary key(sell_id, product_id),
constraint cn13 foreign key(sell_id) references sells(sell_id),
constraint cn14 foreign key(product_id) references products(product_id)
);


alter table coffee_shops_networks 
add network_name varchar(45) not null after network_id;

alter table coffee_shop 
add coffee_shop_name varchar(45) not null after coffee_shop_id;

insert into coffee_shops_networks
(network_name, director, sub_director, main_office_address)
values
("GoodCoffee", "Ivan Ivanov", "Petr Petrov", "Lesnaya str,12"),
("CoffeeTut", "Oleg Olegov", "Sergey Sergeev", "Belaya str,15"),
("HotCup", "Maxim Maximov", "Petr Olegov", "Svetlaya str,12");

select * from coffee_shops_networks;
select * from coffee_shop;

insert into coffee_shop
(coffee_shop_id, coffee_shop_name, coffee_shop_man, coffee_shop_address, network_id)
values
(1, "HotCup", "Igor Igorev","Temnaya str,24", 3),
(2, "GoodCoffee", "Sidor Sidorov", "Veselaya str,31", 1),
(3, "CoffeeTut", "Evgeniy Evgenyev", "Morskaya str,16",2);

insert into coffee_shop
(coffee_shop_id, coffee_shop_name, coffee_shop_man, coffee_shop_address)
values
(4, "CoffeeTee", "Igor Ivanov", "Krasivaya str, 17"),
(5, "BestCoffee", "Svetlana Maximova", "Krasivaya str, 57");

select * from staff;
insert into staff
(employee_name, employee_surname, employee_post, coffee_shop_id)
values
("Alexandr","Svetlov","barista", 5),
("Tatiana","Chernaya","administrator", 3),
("Anna","Chernova","barista", 3),
("Zhanna","Chernova","cleaner", 1);

select * from contracts;
insert into contracts
(contract_start_date, contract_end_date, contract_salary)
values
("2019-12-12","2020-12-12",700),
("2018-11-11","2020-11-11",1000),
("2019-10-10","2021-10-10",850),
("2020-04-04","2022-04-04",400);

select * from storages;
insert into storages
(storage_address)
values
("Lesnaya str,12"),
("Belaya str,19"),
("Krasivaya str, 17"),
("Krasivaya str, 57"),
("Svetlaya str,18");

select * from products;
insert into products
(product_name, product_type)
values
("Coffee Neskafe", "coffe"),
("Green tea ahmad", "tea"),
("Moloko minskoe", "milk");

select * from storages_products;
insert into storages_products
(storage_id, product_id, quantity)
values
(1, 1, 5),
(1, 3, 4),
(3, 3, 1), 
(4, 2, 10),
(4, 3, 5),
(2, 1, 15),
(5, 2, 11);

select * from sells;
insert into sells
(sell_id, sell_date, sell_amount, employee_id)
values
(1, "2020-09-09", 4, 1),
(2, "2020-08-08", 3, 3),
(3, "2020-07-07", 1, 3),
(4, "2020-06-06", 2, 1);

create table contracts1
(
contract_id char(8) primary key default (left(UUID(),8)),
contract_company varchar(45),
contract_start_date date default(current_date),
nick varchar(16) default (concat(left(contract_company,3), contract_id))
);

insert into contracts1
(contract_company)
values
("me"), ("you"), ("she");

insert into contracts1
(contract_company)
values
("me1"), ("you1"), ("she1");

select * from contracts1;

#sign_up_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

create table types_b
(
id integer primary key auto_increment,
state char(2),
post enum("barman", "cook"),
year_of_birth year,
sign_up timestamp,
small_descr tinytext,
summary text,
pos_post set("admin", "cleaner", "manager"),
married tinyint(1),
bank_account decimal(10,2)
);

insert into types_b
(state,post,year_of_birth,sign_up,small_descr,summary,pos_post,married,bank_account)
values 
("la","barman",1988, NOW(),"very cool person", "summary here","admin",1, 555.33),
("la","cook",1989, NOW(),"very good person",  "summary here","cleaner",0, 666.89)
;
select * from types_b;