create database lab8;

use lab8;

create table stock
(
id int primary key auto_increment,
cost float,
s_type varchar(20),
s_year year,
s_desc varchar(50),
ref_coll int,
constraint cn7 foreign key(ref_coll) references collection(id)
);

alter table stock drop primary key, change id id int primary key;
alter table st_mat add constraint cn3 foreign key (st_id) references stock(id) on delete cascade;
alter table st_mat drop foreign key cn3;

SET GLOBAL log_bin_trust_function_creators = 1;
SET SQL_SAFE_UPDATES = 0;

delimiter //
create function idgenerator()
returns int
begin
return floor(rand()*90000+10000);
end//
delimiter;

delimiter //
create procedure createRow(in cost float, in stype varchar(20), in syear year, in sdesc varchar(50), in ref int, in d date, out id int)
begin
declare localid int;
set localid=idgenerator();
insert into stock
()
values
(localid,cost,stype,syear,sdesc,ref,d);
set id=localid;
end//
delimiter;

select * from stock;
create table collection
(
id int primary key auto_increment,
name varchar(20),
class enum("premium", "usual"), 
st_date date,
end_date date
);


create table material
(
id int primary key auto_increment,
name varchar(50),
property varchar(100)
);

select * from material;
alter table material add column u_cost float;

create table st_mat
(
st_id int,
mat_id int,
quantity float,
primary key(st_id, mat_id),
constraint cn3 foreign key(st_id) references stock(id) on delete cascade,
constraint cn4 foreign key(mat_id) references material(id) on delete cascade
);

create table supplier
(
id int primary key auto_increment,
name varchar(50),
manager varchar(50),
address varchar(50),
start_date date, 
enddate date
);

alter table supplier rename column enddate to end_date;

create table sup_mat
(
sup_id int,
mat_id int,
deliver date,
cost float,
quantity int,
primary key(sup_id, mat_id, deliver),
constraint cn5 foreign key(sup_id) references supplier(id) on delete cascade,
constraint cn6 foreign key(mat_id) references material(id) on delete cascade
);

insert into collection (name, class, st_date, end_date)
values 
("Barbara", "premium", "2010-10-10", null), 
("Monica", "usual", "2012-09-09", null);

insert into stock (cost, s_type, s_year, s_desc, ref_coll) values
(500, "Sofa", 2019, "stylish, new", 1),
(300, "Table", 2020, "comfortable", 2);

insert into material (name, property) values
("plank", "thin"), 
("plank", "thick"), 
("fabric", "brown"), 
("leather", "white");

select * from material;
update material set u_cost=(id+1)*10;

set sql_safe_updates=0;

insert into st_mat (st_id, mat_id, quantity) values
(1, 1, 10), (1, 2, 10), (1, 3, 7), (1, 4, 8), (2, 1,9), (2, 2,6);

insert into supplier (name, manager, address, start_date, end_date) values
("Wood plant", "Ivanov", "Moskovaskaya st., 18", "2019-05-05", null), 
("Fabric plant", "Petrov", "Minskaya st., 25", "2017-06-06", null);

insert into sup_mat () values
(1, 1, "2019-06-06", 1000, 500),
(1, 2, "2019-06-06", 1000, 350),
(2, 3, "2019-06-06", 800, 400),
(2, 4, "2019-06-06", 1500, null);

create index ind1 on stock(s_desc(4));
create index ind2 on supplier(name(4));

delimiter ;;
create trigger checker before insert on sup_mat
for each row 
begin
    declare dd date;
	select end_date into dd from supplier where id=new.sup_id;
    if new.deliver>dd then
		signal sqlstate '45000';
    end if;
end ;;
delimiter ;

delimiter ;;
create trigger up before insert on sup_mat
for each row 
begin
	declare q int;
    select count(sup_id) into q from sup_mat where sup_id=new.sup_id;
    if (q>5) then
		set new.cost=new.cost*0.95;
    end if;
end ;;
delimiter ;

delimiter ;;
create trigger ch1 before insert on supplier
for each row 
begin
	if (new.end_date<end.start_date) then
		signal sqlstate '45000';
    end if;
end;;
delimiter ;

delimiter ;;
create trigger ch2 before insert on collection
for each row 
begin
	if (new.end_date<end.st_date) then
		signal sqlstate '45000';
    end if;
end;;
delimiter ;


delimiter //
create procedure fill()
begin
	update collection 
    set end_date=date_add(st_date, interval 1 year)
    where end_date=null;
end//
delimiter ; 

select * from collection;
select * from stock;
select * from material;
select * from st_mat;

delimiter //
create procedure predict(in id int)
begin
	select sum(quantity*u_cost)*1.2 from st_mat
	inner join material on mat_id=material.id
	where st_id=id;
end//
delimiter ; 

call predict(2);

select * from stock;

