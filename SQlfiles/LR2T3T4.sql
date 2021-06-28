create database lab2_3_4;
use lab2_3_4;

create table products
(
	productName varchar(30),
    SupplierName varchar(20),
    packSize varchar(5)
);

insert into products values
('Chai', 'Exotic Liquids', '16 oz'),
('Chai', 'Exotic Liquids', '12 oz'),
('Chai', 'Exotic Liquids', '8 oz'),
('Chef Anton`s Seasoning', 'New Orleans', '16 oz'),
('Chef Anton`s Seasoning', 'New Orleans', '12 oz'),
('Chef Anton`s Seasoning', 'New Orleans', '8 oz'),
('Pavlova', 'Pavlova Ltd', '16 oz'),
('Pavlova', 'Pavlova Ltd', '12 oz'),
('Pavlova', 'Pavlova Ltd', '8 oz');

select * from products;