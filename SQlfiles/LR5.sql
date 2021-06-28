 create database lb5;

use lb5;

create table person
(
person_id int primary key auto_increment,
person_name varchar(30),
person_surname varchar(30),
person_address varchar(50),
person_status enum('solvent', 'insolvent'),
person_date date
);

create table account
(
acc_number varchar(50) primary key,
acc_type enum('credit','deposit','current'),
acc_balance double,
acc_start_date datetime,
acc_owner int,
constraint cn1 foreign key(acc_owner) references person(person_id)
);

create table operations
(
op_id int primary key auto_increment,
op_type enum('transfer', 'payment')
);

create table appointment
(
app_id int primary key auto_increment,
app_op int,
app_sender varchar(50),
app_recipient varchar(50),
app_time datetime,
app_value double,
app_contr_number double,
constraint cn2 foreign key(app_op) references operations(op_id),
constraint cn3 foreign key(app_sender) references account(acc_number)
);
drop procedure transfer1;
delimiter ;;
create procedure transfer1(in sender varchar(50), in rec varchar(50), in sum double)
begin
declare i int;
start transaction;
update account
set acc_balance=aes_encrypt(cast(aes_decrypt(acc_balance, 'bank') as double)-cast(sum as double), 'bank')
where acc_number=aes_encrypt(sender, 'banker');
if row_count()>0 then
	update account
    set acc_balance=aes_encrypt(cast(aes_decrypt(acc_balance, 'bank') as double)+cast(sum as double), 'bank')
    where acc_number=aes_encrypt(rec, 'banker');
    if row_count()>0 then
		insert into operations (op_type) values ('transfer');
        select last_insert_id() into i; 
		insert into appointment
        (app_op, app_sender, app_recipient,app_time, app_value, app_contr_number)
        values
        (i, aes_encrypt(sender, 'banker'), aes_encrypt(rec, 'banker'), now(), cast(sum as double), cast(left(UUID(),8) as double));
        commit;
	else rollback;
    end if;

else rollback;
end if;
end;;
delimiter ;
drop procedure transfer1;
insert into person (person_name, person_surname, person_address, person_status, person_date) values
('Ivan', 'Ivanov', 'Minsk', 'solvent', curdate());

insert into person (person_name, person_surname, person_address, person_status, person_date) values
('Petr', 'Petrov', 'Minsk', 'solvent', curdate());

insert into account ()
values ('9111 0001 4578 1134', 'current', 4000,curdate(), 1);
insert into account ()
values ('9332 0002 4689 3212', 'current', 3000,curdate(), 2);


select * from person;
select * from account;
select * from operations;
select * from appointment;

select cast(aes_decrypt(acc_number, 'banker') as char), cast(aes_decrypt(acc_balance, 'bank') as double) from account;
call transfer1('9111 0001 4578 1134', '9332 0002 4689 3212', '100');
alter table operations
modify op_type enum('transfer', 'payment', 'charge', 'replenishment');

drop trigger checker;
drop procedure charge;

delimiter ;;
create trigger checker before update on account
for each row 
begin
	if cast(aes_decrypt(new.acc_balance, 'bank') as double)<0 then 
		set new.acc_balance=old.acc_balance;
	end if;
end ;;
delimiter;

delimiter ;;
create procedure charge(in sender varchar(50), in sum double)
begin
declare i int;
start transaction;
update account
set acc_balance=aes_encrypt(cast(aes_decrypt(acc_balance, 'bank') as double)-sum, 'bank')
where acc_number=aes_encrypt(sender, 'banker');
if row_count()>0 then
	insert into operations (op_type) values ('charge');
	select last_insert_id() into i; 
	insert into appointment
	(app_op, app_sender,app_time, app_value, app_contr_number)
	values
	(i, aes_encrypt(sender, 'banker'), now(), sum, cast(left(UUID(),8) as double));
	commit;
else rollback;
end if;
end;;
delimiter ;


select * from account;
call charge('9111 0001 4578 1134', 700);
drop procedure replenishment;
delimiter ;;
create procedure replenishment(in sender varchar(50), in sum double)
begin
declare i int;
start transaction;
update account
set acc_balance=aes_encrypt(cast(aes_decrypt(acc_balance, 'bank') as double)+sum, 'bank')
where acc_number=aes_encrypt(sender, 'banker');
if row_count()>0 then
	insert into operations (op_type) values ('replenishment');
	select last_insert_id() into i; 
	insert into appointment
	(app_op, app_sender,app_time, app_value, app_contr_number)
	values
	(i, aes_encrypt(sender, 'banker'), now(), sum, cast(left(UUID(),8) as double));
	commit;

else rollback;
end if;
end;;
delimiter ;
drop procedure replenishment;
call replenishment('9111 0001 4578 1134', 700);

alter table account modify acc_balance varbinary(50);
alter table account modify acc_number varbinary(50);
alter table appointment modify app_sender varbinary(50);
alter table appointment modify app_recipient varbinary(50);
select * from account;
ALTER TABLE appointment
drop foreign key cn3;
ALTER TABLE appointment
ADD constraint cn3 foreign KEY (app_sender) REFERENCES account(acc_number);
update account set acc_balance=aes_encrypt(acc_balance, 'bank');

update appointment set app_sender=aes_encrypt(app_sender, 'banker');
update account set acc_number=aes_encrypt(acc_number, 'banker');

select cast(aes_decrypt(acc_number, 'banker') as char),cast(aes_decrypt(acc_balance, 'bank') as double) from account;


select * from appointment;
drop table account;

#1.5 
create or replace view person1
(app_time, sender, recipient, app_value, app_type) as
select app_time, app_sender, app_recipient, app_value, op_type
from person
inner join account
on person_id=acc_owner
right join 
(select appointment.*, operations.op_type from appointment
inner join operations
on app_op=op_id) as tb on 
app_sender=acc_number or app_recipient=acc_number
where person_id=1 and app_time>'2020-12-20' and app_time<'2020-12-22';

select * from person1;

#1.6--------------------------------------------------
create table credits
(
cr_id int primary key auto_increment,
person int,
amount double, 
perc double, 
bank varchar(50), 
start_date date,
end_date date,
on_time bool, 
mon_pay double, 
rest double,
constraint cn4 foreign key(person) references person(person_id)
);


delimiter ;;
create procedure credit_info(in per int)
begin
	select * from credits where person=per;
end;;
delimiter ;

select * from credits;
insert into credits
(person, amount, perc, bank, start_date, end_date, on_time, mon_pay, rest)
values
(2, 3000, 10, 'sberbank', '2017-12-12', '2018-12-12', 1, 275, 0),
(2, 2000, 9, 'belarusbank', '2018-10-10', '2019-10-10', 0, 182, 0),
(2, 3000, 10, 'sberbank', '2019-12-12', '2021-12-12', 0, 150, 1800);

drop procedure credit_get;
delimiter ;;
create procedure credit_get(in per int, in bank varchar(20), in sum double, in perc double, in period int)
begin

declare i,k double;
start transaction;

set i=sum/period+sum/12*perc/100;
select sum(on_time=0)/count(person) into k from credits where person=per;
if (k<0.5 or (k is null and sum<1000)) then
       insert into credits (person, amount, perc, bank, start_date, end_date, mon_pay, rest)
	   values
	   (per, sum, perc, bank, curdate(), date_add(curdate(), interval period month),i, sum);
end if;

commit;
end;;
delimiter ;

call credit_get(1, 'sberbank', 2000, 11, 12);
select * from credits;
set sql_safe_updates=0;
create table history
(
id int,
pay double,
ost double
);
drop table history;
select * from history;

drop procedure credit_story;
delimiter ;;
create procedure credit_story(in id int)
begin
declare len, i int;
declare s, pp,p, os double;
delete from history where id>0;
set i=1;
select timestampdiff(month, start_date, end_date) into len from credits where cr_id=id;
select amount into s from credits where cr_id=id;
select perc into pp from credits where cr_id=id;
set p=s/len+s/1200*pp;
set os=s*(len-i)/len;
insert into history (id, pay, ost) values (i, p, os);
while i < len do
	set p=s/len+os/1200*pp;
    set i=i+1;
    set os=s*(len-i)/len;
    insert into history (id, pay, ost) values (i, p, os);
end while;
end;;
delimiter ;
select * from credits;
call credit_story(4);
select * from history;

delimiter ;;
create procedure credit_pay(in per int, in id int, in sum double)
begin
start transaction;
update credits set rest=if(rest-sum>0, rest-sum, 0)
where person=per and cr_id=id and curdate()<end_date;
commit;
end;;
delimiter ;
drop procedure credit_pay;

select * from credits;
call credit_pay(2, 4, 2000);

#2---------------------------------------------------------
create database shops;

use shops;

create table storage
(
id int primary key auto_increment,
pr_type varchar(50),
pr_q int
);

create table st_points
(
pr_id int, 
p_id int,
q int,
primary key(pr_id, p_id), 
constraint cn4 foreign key(pr_id) references storage(id),
constraint cn5 foreign key(p_id) references points(id)
);
insert into st_points ()
values (1, 1, 5), (2, 1, 3), (3, 1, 4), (4, 2, 1), (3, 2, 7);

create table points
(
id int primary key auto_increment,
address varchar(50)
);

create table staff
(
id int primary key auto_increment,
name varchar(50),
surname varchar(50),
post varchar(50),
place int,
constraint cn1 foreign key(place) references points(id)
);
drop table sells;

create table sells
(
id int primary key auto_increment,
p_id int, 
constraint cn3 foreign key(p_id) references staff(id)
);

create table pr_sells
(
s_id int, 
pr_id int, 
primary key(s_id, pr_id), 
constraint cn2 foreign key(s_id) references sells(id),
constraint cn6 foreign key(pr_id) references storage(id)
);

insert into storage(pr_type, pr_q)
values
('bananas', 10),
('water', 30), 
('cheese', 10), 
('apples', 10);

insert into points(address)
values ('ul. belaya'), ('ul. chernaya');

insert into staff(name, surname, post, place) values
('Ivan', 'Ivanov', 'loader', 1),
('Anna', 'Petrova', 'seller', 1),
('Anna', 'Ivanova', 'seller',2);

insert into sells () values (1, 2, curdate()),(2, 3, curdate());
select * from staff where post='seller';

create table passwords
(
pass varchar(100), 
st_id int, 
constraint cn12 foreign key(st_id) references staff(id)
);

insert into passwords
()
values
(MD5('pass1'), 2), (MD5('pass2'), 3);

select pass=MD5('pass1') from passwords where st_id=2;
select * from sells;
drop procedure purchase;
delimiter ;;
create procedure purchase(in pr int, in ad int, in per int, in pas varchar(100), in qua int)
begin
declare i int;
start transaction;
update st_points set q=q-qua
where pr_id=pr and p_id=ad;
if row_count()>0 then
	if (select pass=MD5(pas) from passwords where st_id=per)=1 then
	insert into sells (p_id) values (per);
    select last_insert_id() into i;
    insert into pr_sells () values (i, pr);
    else rollback;
    end if;
    commit;
else rollback;
end if;
end;;
delimiter ;

delimiter ;;
create trigger checker1 before update on st_points
for each row 
begin
	if new.q<0 then 
		set new.q=old.q;
	end if;
end ;;
delimiter ;

drop procedure purchase;
drop trigger checker2;

delimiter ;;
create trigger checker2 before insert on sells
for each row 
begin
	if (new.p_id not in (select id from staff))  then 
		signal sqlstate '45000';
	end if;
end ;;
delimiter ;

select * from sells;
select * from st_points;
select * from pr_sells;

call purchase(3, 2,3,'pass2',1);
drop table clients;

create table clients
(
id int primary key auto_increment,
name varchar(50), 
surname varchar(50), 
phone varchar(30), 
q int, 
sale double, 
card bool
);

insert into clients(name, surname, phone, q, sale, card) values
('Ivan', 'Ivanov', '+375 29 6785498', 1, 0, 1),
('Petr', 'petrov', '+375 25 6685798', 5, null, 0), 
('Anna', 'Ivanova', '+375 44 5885798', 10, 1, 1);
SET SQL_SAFE_UPDATES = 0;

select * from clients;
alter table clients modify phone varbinary(50);
update clients set phone=aes_encrypt(phone, 'shop');

select cast(aes_decrypt(phone, 'shop') as char) from clients;


create table orders
(
id int primary key auto_increment,
cl_id int, 
pr_id int,
q int, 
ord_date date,
constraint cn7 foreign key(cl_id) references clients(id), 
constraint cn8 foreign key(pr_id) references storage(id)
);

delimiter ;;
create procedure orders(in cl int, in pr int, in q int)
begin
start transaction;
insert into orders (cl_id, pr_id, q, ord_date) values (cl, pr, q, curdate());
commit;
end;;
delimiter ;

call orders(1, 1, 1);


delimiter ;;
create procedure autoorders(in cl int, in pr int)
begin
declare qua int;
start transaction;
if (select count(id) from orders where cl_id=cl)>=2 then
    select ceiling(avg(q)) into qua from orders where cl_id=cl;
	insert into orders (cl_id, pr_id, q, ord_date) values (cl, pr, qua, curdate());
	commit;
end if;
end;;
delimiter ;

call autoorders(3, 1);
select * from orders;

select count(id), cl_id from orders group by cl_id;


#3---------------------------------------------------
use coffee_shops;

create user director identified by 'dirpass123';
grant all privileges on coffee_shops to director;

create user manager identified by 'manpass123';
grant select on products.* to manager;
grant select on sells_products.* to manager;
grant select on sells.* to manager;

create user barista identified by 'barpass123';
grant insert,update, delete on sells.* to barista;


select * from sells;
select * from sells_products;

delimiter ;;
create procedure pr1(in pr int, in per int, in q int)
begin
declare i int;
start transaction;
select max(sell_id) into i from sells;
insert into sells (sell_id, sell_date, sell_amount, employee_id) values (i+1, curdate(), q, per);
insert into sells_products (sell_id, product_id) values (i+1, pr);
commit;
end;;
delimiter ;

call pr1(3, 1, 2);

delimiter ;;
create procedure pr2(in per int, in perc double)
begin
start transaction;
update contracts
set contract_salary=aes_encrypt(cast(aes_decrypt(contract_salary, 'shop') as double)+perc/100*cast(aes_decrypt(contract_salary, 'shop') as double), 'shop')
where contract_id=per;
commit;
end;;
delimiter ;
drop procedure pr2;
call pr2(1, 10);

select * from contracts;
select * from staff;
alter table contracts add column nick varchar(16);
update contracts, staff set nick=(concat(left(employee_name,3),'_',left(employee_surname,3), '_',contract_id))
where employee_id=contract_id;
use coffee_shops;
create index myind on contracts(nick(3));
explain select * from contracts
where nick='Ale%';

alter table contracts modify contract_start_date varbinary(50);
alter table contracts modify contract_end_date varbinary(50);
update contracts set contract_start_date=aes_encrypt(contract_start_date, 'shop');
update contracts set contract_end_date=aes_decrypt(contract_end_date, 'shop');

select cast(aes_decrypt(contract_start_date, 'shop') as date) from contracts;

alter table contracts modify contract_salary varbinary(40);
update contracts set contract_salary=aes_encrypt(contract_salary, 'shop');
select cast(aes_decrypt(contract_salary, 'shop') as double) from contracts;

#4-----------------------------------------------------------------------------
create database litdb;

use litdb;

create table quotes
(
id integer primary key auto_increment,
author varchar(30),
quote text
);

insert into quotes
()
values
(1,'Einstein',"You can't blame gravity for falling in love."),
(2,'Einstein',"Look deep into nature, and then you will understand everything better."),
(3,'Einstein',"Insanity: doing the same thing over and over again and expecting different results."),
(4,'Einstein',"Learn from yesterday, live for today, hope for tomorrow. The important thing is not to stop questioning."),
(5,'Einstein',"Try not to become a man of success, but rather try to become a man of value."),
(6,'Einstein',"Once we accept our limits, we go beyond them."),
(7,'Einstein',"When you are courting a nice girl an hour seems like a second. When you sit on a red-hot cinder a second seems like an hour. That's relativity."),
(8,'Einstein',"The difference between stupidity and genius is that genius has its limits."),
(9,'Einstein',"We cannot solve our problems with the same thinking we used when we created them."),
(10,'Einstein',"The true sign of intelligence is not knowledge but imagination."),
(11,'Franklin',"Tell me and I forget. Teach me and I remember. Involve me and I learn."),
(12,'Franklin',"Without continual growth and progress, such words as improvement, achievement, and success have no meaning."),
(13,'Franklin',"We are all born ignorant, but one must work hard to remain stupid."),
(14,'Franklin',"An investment in knowledge pays the best interest."),
(15,'Franklin',"Lost time is never found again."),
(16,'Franklin',"Early to bed and early to rise makes a man healthy, wealthy and wise."),
(17,'Franklin',"It takes many good deeds to build a good reputation, and only one bad one to lose it."),
(18,'Franklin',"They who can give up essential liberty to obtain a little temporary safety deserve neither liberty nor safety."),
(19,'Franklin',"Money has never made man happy, nor will it, there is nothing in its nature to produce happiness. The more of it one has the more one wants."),
(20,'Franklin',"Anger is never without a reason, but seldom with a good one."),
(21,'Carr',"Trees love to toss and sway; they make such happy noises."),
(22,'Carr',"You must be absolutely honest and true in the depicting of a totem for meaning is attached to every line. You must be most particular about detail and proportion."),
(23,'Carr',"Oh, Spring! I want to go out and feel you and get inspiration. My old things seem dead. I want fresh contacts, more vital searching."),
(24,'Carr',"There is something bigger than fact: the underlying spirit, all it stands for, the mood, the vastness, the wildness."),
(25,'Carr',"You come into the world alone and you go out of the world alone yet it seems to me you are more alone while living than even going and coming."),
(26,'Carr',"It is wonderful to feel the grandness of Canada in the raw."),
(27,'Carr',"I think that one's art is a growth inside one. I do not think one can explain growth. It is silent and subtle. One does not keep digging up a plant to see how it grows."),
(28,'Carr',"The artist himself may not think he is religious, but if he is sincere his sincerity in itself is religion."),
(29,'Carr',"I sat staring, staring, staring - half lost, learning a new language or rather the same language in a different dialect. So still were the big woods where I sat, sound might not yet have been born."),
(30,'Carr',"Be careful that you do not write or paint anything that is not your own, that you don't know in your own soul."),
(31,'Ford',"Coming together is a beginning; keeping together is progress; working together is success."),
(32,'Ford',"If everyone is moving forward together, then success takes care of itself."),
(33,'Ford',"Thinking is the hardest work there is, which is probably the reason why so few engage in it."),
(34,'Ford',"When everything seems to be going against you, remember that the airplane takes off against the wind, not with it."),
(35,'Ford',"My best friend is the one who brings out the best in me."),
(36,'Ford',"Don't find fault, find a remedy."),
(37,'Ford',"If you think you can do a thing or think you can't do a thing, you're right."),
(38,'Ford',"Life is a series of experiences, each one of which makes us bigger, even though sometimes it is hard to realize this. For the world was built to develop character, and we must learn that the setbacks and grieves which we endure help us in our marching onward."),
(39,'Ford',"Anyone who stops learning is old, whether at twenty or eighty. Anyone who keeps learning stays young. The greatest thing in life is to keep your mind young."),
(40,'Ford',"Quality means doing it right when no one is looking."),
(41,'Gandhi',"Happiness is when what you think, what you say, and what you do are in harmony."),
(42,'Gandhi',"Where there is love there is life."),
(43,'Gandhi',"The weak can never forgive. Forgiveness is the attribute of the strong."),
(44,'Gandhi',"Live as if you were to die tomorrow. Learn as if you were to live forever."),
(45,'Gandhi',"The best way to find yourself is to lose yourself in the service of others."),
(46,'Gandhi',"You must not lose faith in humanity. Humanity is an ocean; if a few drops of the ocean are dirty, the ocean does not become dirty."),
(47,'Gandhi',"When I admire the wonders of a sunset or the beauty of the moon, my soul expands in the worship of the creator."),
(48,'Gandhi',"My life is my message."),
(49,'Gandhi',"Strength does not come from physical capacity. It comes from an indomitable will."),
(50,'Gandhi',"It is health that is real wealth and not pieces of gold and silver.");

create fulltext index litind on quotes(quote);

select quote, match (quote) against ('tomorrow')
from quotes
where match (quote) against ('tomorrow');

select quote, match (quote) against ('die')
from quotes
where match (quote) against ('die');

select * from quotes 
where match (quote) against ('love + today' in boolean mode);

select * from quotes 
where match (quote) against ('love - today' in boolean mode);

select * from quotes 
where match (quote) against ('+today+(>live<die)' in boolean mode);

select * from quotes 
where match (quote) against ('to*' in boolean mode);

select * from quotes 
where match (quote) against ('+hope-love' in boolean mode);

select * from quotes 
where match (quote) against ('+together~today' in boolean mode);