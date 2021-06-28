create database shop;

use shop;

create table manufact
(
manuf_id int primary key auto_increment,
manuf_name varchar(50) not null,
manuf_country varchar(20) not null,
manuf_phone varchar(20),
manuf_adress varchar(50)
);

create table product
(
prod_id int primary key auto_increment,
prod_name varchar(30) not null,
prod_type enum('food','drink'),
prod_manuf_ref_id int not null,
prod_quantity int,
constraint cn1 foreign key(prod_manuf_ref_id) references manufact(manuf_id) on delete cascade
);

insert into manufact (manuf_name,manuf_country,manuf_phone,manuf_adress) values
("Coca cola", "Belarus", "+375171111111", "Minsk"),
("Brest-Litovsk","Belarus", "+375172222222", "Brest");

insert into product(prod_name, prod_type, prod_manuf_ref_id, prod_quantity) values
("Sprite", "drink",1,20),
("Bonakva","drink", 1,25),
("Cheese","food",2,13),
("Milk","drink",2,16);

alter table product add column prod_price float not null;

select * from product;

update product set prod_price=1.5;

set sql_safe_updates=0; 

create table customer
(
cust_id int primary key auto_increment,
cust_card varchar(20) not null,
cust_name varchar(20) not null,
cust_surname varchar(20) not null
);

create table pr_cust
(
prod_id int,
cust_id int,
quantity int,
purchase date,
primary key(prod_id, cust_id, purchase),
constraint cn2 foreign key(prod_id) references product(prod_id),
constraint cn3 foreign key(cust_id) references customer(cust_id)
);

insert into customer (cust_card, cust_name, cust_surname) values
("2020 1414 1515 1616", "Anna", "Petrova"),
("2090 1313 1212 1717", "Petr", "Ivanov");

insert into pr_cust () values
(2, 1, 1,current_date()),
(3,1,2, current_date()),
(2,2, 3, current_date()),
(1,2,1,current_date());

update product set prod_name="Bonaqua" where prod_id=2;

select * from manufact;