use world;

select * from city;
select * from countrylanguage;

#список всех стран и столиц
select country.name, city.name
from country
inner join city
on country.capital=city.id;

#средний процент использования английского языка по странам
select avg(Percentage) from countrylanguage
where Language = "English";

#страны превышающие по площади территорию самой большой страны в Африке
select country.name, SurfaceArea from country, 
(select country.name, MAX(SurfaceArea) as ar from country
where Continent = "Africa") as res
where SurfaceArea>res.ar;

#страны, которые по суммарному количество владеющих официальными языками
#превосходят суммарное население двух самых крупных городов

select country.name, round(country.Population/100*s) as newpop, Population from country,
(select CountryCode as c, SUM(Percentage) as s from countrylanguage
where IsOfficial = "T"
group by CountryCode) as res 
where country.Code = c
having newpop > (select sum(tb.Population) from (select Population from city
order by Population desc
limit 2) as tb);


#вывести города, которые имеют два разных слова и располагаются в странах,
#имеющих внп, превосходящее среднее по миру

select country.name, res.Name from country
inner join 
(
select Name, CountryCode from city
where length(Name)-length(replace(name, ' ', '')) = 1 and 
substring_index(Name, ' ', 1) <> right(Name, length(Name)-length(substring_index(Name, ' ', 1))-1)
) as res
on res.CountryCode = country.Code
where GNP>(select avg(GNP) as av from country);

#3---------------------------------------------------------------------

use coffee_shops;

#a
select sells.*, employee_name, products.product_name 
from sells, sells_products, products, staff
where sells.sell_id=sells_products.sell_id and
products.product_id=sells_products.product_id and
sells.employee_id=staff.employee_id;
select * from staff;

#b
select product_id, avg(quantity) from storages_products
group by product_id;

select product_id, avg(quantity) from sells_products
group by product_id;

select employee_post, avg(contract_salary) from staff
inner join contracts
on contract_id=employee_id
group by employee_post;

select coffee_shop_name, count(network_name)  from coffee_shop
inner join coffee_shops_networks
on coffee_shop.network_id=coffee_shops_networks.network_id
group by network_name;

#d
(select employee_name, employee_surname from sells
inner join staff
on staff.employee_id=sells.employee_id)
union
(select employee_name, employee_surname from staff);

#c-b(1)
select coffee_shop.coffee_shop_name, AVG(s) from coffee_shop,
(select contract_salary as s, coffee_shop_id as c from contracts, staff
where contract_id=employee_id) as res
where coffee_shop.coffee_shop_id=res.c
group by coffee_shop.coffee_shop_name;

select sell_date, sell_amount, tb.nm from sells,
(select sells_products.sell_id as id, products.product_name as nm
from sells_products, products
where sells_products.product_id=products.product_id
) as tb
where sells.sell_id=tb.id;

select storage_address, tb.nm from storages,
(select storages_products.storage_id as id, products.product_name as nm
from storages_products, products
where storages_products.product_id=products.product_id
) as tb
where storages.storage_id=tb.id and nm="Coffee Neskafe";

#e
update storages
inner join
(select storages_products.storage_id as id, products.product_name as nm
from storages_products, products
where storages_products.product_id=products.product_id) as  res on storages.storage_id=res.id
set storages.storage_address="no data"
where nm="Coffee Neskafe";

#4---------------------------------------------------------------------
create or replace view admin
(sell_date, sell_amount, pr_name, name, surname) as 
select sell_date, sell_amount, tb.nm, employee_name, employee_surname from sells, staff,
(select sells_products.sell_id as id, products.product_name as nm
from sells_products, products
where sells_products.product_id=products.product_id
) as tb
where sells.sell_id=tb.id and sells.employee_id=staff.employee_id;

select * from admin;

create or replace view worker
(products) as
select product_name from products;

select * from worker;

create or replace view pers_dep
(name, surname, post, start_date, end_date, salary) as
select employee_name, employee_surname, employee_post, contract_start_date,
contract_end_date, contract_salary from staff
inner join contracts
on contract_id=employee_id;

select * from pers_dep;