create database lb4;

use lb4;

create table studs
(
st_id int primary key auto_increment,
st_name varchar(30) not null,
st_surname varchar(30) not null,
st_speciality enum('web', 'km', 'mobilki'),
st_form enum('paid','notpaid'),
st_value float not null
);

create table subjects
(
sub_id int primary key auto_increment,
sub_name varchar(50) not null,
sub_teacher varchar(20),
sub_hours int not null
);

create table exams
(
exam_id int primary key auto_increment,
exam_date datetime not null,
exam_mark int,
ref_sub_id int not null,
ref_st_id int not null,
constraint cn1 foreign key(ref_st_id) references studs(st_id),
constraint cn2 foreign key(ref_sub_id) references subjects(sub_id)
);

alter table exams drop foreign key cn1;
alter table exams drop foreign key cn2;

alter table exams add constraint cn1 foreign key(ref_st_id) references studs(st_id)
on delete cascade on update cascade;

alter table exams add constraint cn2 foreign key(ref_sub_id) references subjects(sub_id)
on delete cascade on update cascade;

insert into studs (st_name, st_surname, st_speciality, st_form, st_value)
values 
('Alexey', 'Alexeev','web','paid', 8),
('Ivan', 'Ivanov', 'km', 'notpaid', 9),
('Pavel', 'Pavlov', 'mobilki', 'paid', 8);

insert into studs 
(st_name, st_surname, st_speciality, st_form, st_value, price, hgroup, activ, activ_lev, scholarship)
values 
('Boris', 'Bely','web','notpaid', 8.7, null, 3, null, null, 140),
('Dmitry', 'Bubnov','web','paid', 6, 100, 1, 'brsm', 7, null),
('Daria', 'Makarova','km','notpaid', 9.3, null, 2, 'studsouz', 7, 160),
('Vladislava', 'Morozova','km','notpaid', 9.2, null, 2, null, null, 160),
('Elena', 'Pushkina','km','notpaid', 9.4, null, 2, 'zamstarosta', 5, 160),
('Nikita', 'Shabrin','mobilki','paid', 5.6, 100, 1, 'zamstarosta', 5, null),
('Maria', 'Suchareva','mobilki','paid', 6.3, 100, 1, 'obshkom', 8, null),
('Egor', 'Laban','km','notpaid', 8.4, null, 2, null, null, 120);
 
insert into subjects (sub_name, sub_teacher, sub_hours)
values
('Geoma', 'Bazylev', 20),
('Algerbra', 'Kaskevich', 20),
('Matan', 'Brovka',40); 

insert into exams
(exam_date, exam_mark, ref_sub_id, ref_st_id)
values
('2020-10-09', 9, 2, 1),
('2020-10-10', 10, 3, 2),
('2020-10-11',8, 1, 1),
('2020-10-07', 8, 3, 3);

create table st_sub
(
st_id int not null,
l_id int not null, 
pr bool, 
mark int,
primary key(st_id, l_id),
constraint cn3 foreign key(st_id) references studs(st_id),
constraint cn4 foreign key(l_id) references lessons(id)
);

create table lessons
(
id int primary key auto_increment, 
sub_id int, 
l_date date, 
l_time time,
constraint cn5 foreign key(sub_id) references subjects(sub_id)
);
select * from lessons;
insert into lessons (sub_id, l_date, l_time) values
(1, '2020-12-12', '09:00:00'), (1, '2020-12-20', '09:00:00'), (2, '2020-12-20', '09:00:00'), 
(3, '2020-12-20', '09:00:00');


select * from st_sub;

alter table st_sub drop foreign key cn3;
alter table st_sub drop foreign key cn4;

alter table st_sub add constraint cn3 foreign key(st_id) references studs(st_id)
on delete cascade on update cascade;

alter table st_sub add constraint cn4 foreign key(sub_id) references subjects(sub_id)
on delete cascade on update cascade;

insert into st_sub ()
values
(1, 1, 0, null),
(1, 2,  1, 5),
(1, 3,  1, 6),
(1, 4, 0, null),
(2, 1,  1, 10),
(2, 2, 1, 9),
(2, 3,  1, 6),
(2, 4,  0, null),
(3, 1, 0, null),
(3, 2, 1, 8),
(3, 3, 1, 7),
(3, 4,  0, null);

select * from st_sub;

select * from studs;
select * from subjects;

alter table studs add column price int;
alter table studs add column hgroup enum('1','2','3');

set sql_safe_updates = 0;

update studs set price=100 where st_form='paid';
update studs set hgroup='1' where st_id in (1, 3);
update studs set hgroup='2' where st_id=2;

alter table studs add column activ varchar(30);
alter table studs add column activ_lev int;
update studs set activ='starosta', activ_lev=5  where st_id=2;


alter table studs add column scholarship int;
update studs set scholarship=150 where st_form='notpaid';

#3.1-----------------------------
delimiter //
create procedure incrschol(in id int, in pr int)
begin
	declare idd int;
    declare f varchar(10);
    select st_id into idd from studs where st_id=id;
    select st_form into f from studs where st_id=id;
    if (idd>0 and pr>0 and f='notpaid') then
		update studs
		set scholarship=(1+pr/100)*scholarship where st_id=idd;
    end if;
end//
delimiter ; 

drop procedure incrschol;

select * from studs;
call incrschol(2, 20);

#3.2-------------------------------------------------------
delimiter //
create function avmark(p_name varchar(30))
returns float
begin
	declare avgmark float;
    select avg(exam_mark) into avgmark from exams
	inner join subjects
	where exams.ref_sub_id=subjects.sub_id and sub_teacher=p_name;
    return avgmark;
end//
delimiter ; 

select avmark('Brovka');

#3.3--------------------------------------------------
delimiter //
create procedure surcharge()
begin
	update studs
    set scholarship=scholarship*(1+st_value/100)
    where activ is not null and st_form is not null;
end//
delimiter ; 

select * from studs;

call surcharge();

#3.4-------------------------------------------------
create table most_suc
(
st_name varchar(30),
st_surname varchar(30)
);

create table most_unsuc
(
st_name varchar(30),
st_surname varchar(30)
);

create table most_act
(
st_name varchar(30),
st_surname varchar(30)
);

delimiter //
create procedure 5suc()
begin
	declare n, sn varchar (30);
    declare is_end int default 0;
    
    declare studscur cursor for select st_name, st_surname from studs
    order by st_value desc
    limit 5;
    
    declare continue handler for not found set is_end=1;
    
    open studscur;
    curs:loop
		fetch studscur into n, sn;
        if is_end then
			leave curs;
		end if;
        insert ignore into most_suc () values (n, sn);
	end loop curs;
    
    close studscur;
end//
delimiter ;
drop procedure 5suc;
delimiter //
create procedure 5unsuc()
begin
	declare n, sn varchar (30);
    declare is_end int default 0;
    
    declare studscur cursor for select st_name, st_surname from studs
    order by st_value asc
    limit 5;
    
    declare continue handler for not found set is_end=1;
    
    open studscur;
    curs:loop
		fetch studscur into n, sn;
        if is_end then
			leave curs;
		end if;
        insert ignore into most_unsuc () values (n, sn);
	end loop curs;
    
    close studscur;
end//
delimiter ;

delimiter //
create procedure 5act()
begin
	declare n, sn varchar (30);
    declare is_end int default 0;
    
    declare studscur cursor for select st_name, st_surname from studs
    order by activ_lev desc
    limit 5;
    
    declare continue handler for not found set is_end=1;
    
    open studscur;
    curs:loop
		fetch studscur into n, sn;
        if is_end then
			leave curs;
		end if;
        insert ignore  into most_act () values (n, sn);
	end loop curs;
    
    close studscur;
end//
delimiter ;

select * from studs;

call 5suc();
call 5unsuc();
call 5act();

select * from most_suc;
select * from most_unsuc;
select * from most_act;


#3.5-----------------------------------------------------

 
delimiter //
create procedure expulsion()
begin
	declare idd int;
    declare is_end int default 0;
    
    declare studscur cursor for 
    (select tb.s from (select st_id as s, sub_id, avg(mark) as k from st_sub
	inner join lessons on l_id=id
	group by st_id, sub_id) as tb
	group by tb.s
	having min(k)<6) union
	(select st_id from st_sub group by st_id
	having sum(pr=0)>1);
    
    declare continue handler for not found set is_end=1;
    
    open studscur;
    curs:loop
		fetch studscur into idd;
        if is_end then
			leave curs;
		end if;
        delete from studs where st_id=idd;
	end loop curs;
    
    close studscur;
	delete from studs where st_value<=4;
end//
delimiter ; 


insert into studs 
(st_name, st_surname, st_speciality, st_form, st_value, price, hgroup, activ, activ_lev, scholarship)
values 
('Alexandr', 'Cherny','web','notpaid', 3.9, 100, 1, null, null, 100),
('Alexandr', 'Alexandrov','web','paid', 8, null, 3, null, null, 100);

insert into st_sub
() values (13, 2, '2020-10-10', 0, null);
insert into st_sub
() values (11, 2, '2020-10-10', 0, null);
insert into st_sub
() values (2, 2, '2020-10-11', 1, 9);

call expulsion();

#3.6--------------------------------------------
drop function popmark;

delimiter //
create function popmark(g_name varchar(30))
returns int
begin
	declare mark int;
	select tb.exam_mark into mark from 
	(select exams.exam_mark as exam_mark from exams
	inner join studs on ref_st_id=st_id
	where studs.st_speciality=g_name) as tb
	group by exam_mark 
	order by count(exam_mark) desc
	limit 1;
    return mark;
end//
delimiter ; 

select * from exams;

select popmark('mobilki');

#3.7---------------------------------------------
drop procedure exist;
delimiter //
create procedure exist(in spec varchar(15), out res float)
begin
	select sum(pr=0)/count(pr)*100 into res from st_sub
	inner join studs on st_sub.st_id=studs.st_id
	where studs.st_speciality=spec;
end//
delimiter ; 

call exist('mobilki', @res);
select @res;

#3.8--------------------------------------------
select * from exams;

delimiter //
create procedure loyal(out res1 varchar(30), out res2 varchar(30))
begin
	create temporary table temp 
	select avg(exams.exam_mark) as m, subjects.sub_teacher as t from exams
	inner join subjects on ref_sub_id=sub_id
	group by sub_teacher;
    select t into res1 from temp
    order by m asc
	limit 1;
	select t into res2 from temp
    order by m desc
	limit 1;
    drop temporary table temp;
end//
delimiter ; 

call loyal(@res1, @res2);
select @res1, @res2; 

#3.9--------------------------------------------
select * from studs;
alter table studs 
add column date_b date;

update studs
set date_b=date_add('2002-01-25', interval st_id month);

delimiter //
create procedure prem(in date1 date, in date2 date)
begin
	update studs, (select st_id as id, datediff(date2,date_b) as d from studs
	where datediff(date_b, date1)*datediff(date2, date_b)>0 
	and st_form='notpaid'
	order by date_b) as tb
	set scholarship=scholarship*(1+tb.d/300)
	where st_id=tb.id;
end//
delimiter ; 


#3.10--------------------------------------------
delimiter //
create procedure prem1(in date1 date, in date2 date, in dd int)
begin
	update studs, (select st_id as id, datediff(date2,date_b) as d from studs
	where datediff(date_b, date1)*datediff(date2, date_b)>0 
	and st_form='notpaid'
	order by date_b) as tb
	set scholarship=if(dayofweek(date_b)=dd, scholarship*(1+tb.d/100), scholarship*(1+tb.d/300))
	where st_id=tb.id;
end//
delimiter ; 

#3.11-------------------------------------------

delimiter //
create procedure predict(in stid int, in stsub int)
begin
	select round((tb1.mm+tb2.m+tb4.m1)/3-tb3.mmm) as prmark from 
    (select avg(mark) as mm from st_sub inner join lessons
	on l_id=id where st_id=stid and sub_id=stsub) as tb1,
    (select avg(exams.exam_mark) as m, subjects.sub_id as t from exams
	inner join subjects on ref_sub_id=sub_id
	group by sub_teacher
    having sub_id=stsub) as tb2, 
    (select sum(pr=0)/10 as mmm from st_sub where st_id=stid) as tb3, 
    (select avg(mark) as m1 from semesters where st_id=stid and sub_id=stsub) as tb4;
end//
delimiter ; 
drop procedure predict;
call predict(2,1);

create table semesters
(
id int primary key auto_increment, 
st_id int, 
sub_id int,
sem int, 
mark int,
constraint cn6 foreign key(st_id) references studs(st_id), 
constraint cn7 foreign key(sub_id) references subjects(sub_id)
);

insert into semesters (st_id, sub_id, sem, mark) values 
(2, 1, 1, 10), (2, 2, 1, 9), (2, 1, 3, 10);


#4.1---------------------------------------------
drop trigger sch_enlarge;
delimiter //
create trigger sch_enlarge after insert on exams
for each row 
begin
	update studs, (select avg(exam_mark) as m, ref_st_id as id from exams
	group by ref_st_id) as temp
	set st_value=temp.m
	where st_id in (select idd from (select count(exam_id)=k as p,st_id as idd from exams
	inner join (select st_id, count(st_id) as k from list_ex group by st_id) as tb
	on ref_st_id=st_id
    group by st_id) as tb1
    where p=1) and st_id=temp.id;
end//
delimiter ;

update studs, (select avg(exam_mark) as m, ref_st_id as id from exams
	group by ref_st_id) as temp
	set st_value=0
	where st_id in (select idd from (select count(exam_id)=k as p,st_id as idd from exams
	inner join (select st_id, count(st_id) as k from list_ex group by st_id) as tb
	on ref_st_id=st_id
    group by st_id) as tb1
    where p=1) and st_id=temp.id;
    
select * from studs;

select * from exams
where ref_st_id=3;

create table list_ex
(
st_id int, 
sub_id int,
constraint cn9 foreign key(st_id) references studs(st_id),
constraint cn10 foreign key(sub_id) references subjects(sub_id)
);

insert into list_ex () values (2, 3), (2, 1);
insert into list_ex () values (3, 3), (3, 2);
select count(st_id) from list_ex where st_id=2;


select * from studs
where st_id=6;

update studs
set st_value=9
where st_id=6;
#4.2-----------------------------------------------
select * from studs;

delimiter //
create trigger price_low before update on studs
for each row 
begin
if old.st_form='paid' then
	if new.st_value>old.st_value then
		set new.price=old.price*(1-(new.st_value-old.st_value)/10);
	end if;
end if;
end//
delimiter ;

select * from studs
where st_id=1;

update studs
set st_value=9.1
where st_id=1;

#4.3---------------------------------------------------------
select * from subjects;
select * from st_sub;
select * from exams;
select * from studs;

alter table studs
add column course int default 1;

delimiter //
create trigger pr_course before update on studs
for each row 
begin
	if new.hgroup=3 then
		set new.course=old.course;
	end if;
end//
delimiter ;

update studs
set course=2
where st_id=3;

#4.4-------------------------------------------------

delimiter //
create trigger ex_course after insert on exams
for each row 
begin
	declare i, q int;
    declare mark float;
    declare is_end int default 0;
    
    declare studscur cursor for 
    select 
    ref_st_id, avg(exam_mark), count(exam_mark) from exams
	group by ref_st_id;
    
    declare continue handler for not found set is_end=1;
    
    open studscur;
    curs:loop
		fetch studscur into i, mark, q;
        if is_end then
			leave curs;
		end if;
        if q = 2 and mark>4 then
			update studs
			set course = course+1
			where st_id=i;
        end if;
	end loop curs;
    
    close studscur;
end//
delimiter ;

select * from exams;
insert into exams (exam_date, exam_mark, ref_sub_id, ref_st_id)
values ('2020-10-11',7,1,2);

select * from studs;

#4.5------------------------------------------------
select st_id, count(pr) from st_sub
where pr=0
group by st_id;


delimiter //
create trigger pr_studs after insert on st_sub
for each row 
begin
	declare i, q int;
    declare is_end int default 0;
    
    declare studscur cursor for 
	select st_id, count(pr) from st_sub
	where pr=0
	group by st_id;
    
    declare continue handler for not found set is_end=1;
    
    open studscur;
    curs:loop
		fetch studscur into i, q;
        if is_end then
			leave curs;
		end if;
        if q > 4 then
			update studs
			set st_name = concat(st_name, '.')
			where st_id=i;
        end if;
	end loop curs;
    
    close studscur;
end//
delimiter ;

select * from studs;
select * from st_sub;
drop table st_sub;

insert into st_sub ()
values
(3,1,'2020-12-12',0, null);
insert into st_sub ()
values
(3,2,'2020-12-12',0, null);
insert into st_sub ()
values
(3,3,'2020-12-12',0, null);

#4.3-------------------------------------------------------------
delimiter //
create trigger km_cool before insert on studs
for each row 
begin
	if new.st_speciality='km' then
		case new.st_form
        when 'paid' then
			set new.price=50;
		when 'notpaid' then
			set new.scholarship=200;
        end case;
    end if;
end//
delimiter ;

#5-------------------------------------------------------------------
create table credits
(
st_id int,
sub_id int,
cr_date date,
pass bool,
constraint cr_st foreign key(st_id) references studs(st_id),
constraint cr_sub foreign key(sub_id) references subjects(sub_id)
);
insert into credits ()
values
(3, 2, '2020-12-12', 1);

insert into credits ()
values
(1, 3, '2020-12-12', 0);

select * from credits;
insert into exams
(exam_date, ref_sub_id, ref_st_id, cr_req) values
('2020-12-20', 2, 3, 1);

insert into exams
(exam_date, ref_sub_id, ref_st_id, cr_req) values
('2020-12-20', 3, 1, 1);

alter table exams
add column cr_req bool default 0;

select * from exams;

delimiter //
create trigger cr_check before update on exams
for each row 
begin
	if old.exam_id not in (select exams.exam_id from exams
	inner join credits 
	on exams.ref_st_id=credits.st_id and exams.ref_sub_id=credits.sub_id
	where pass=cr_req=1) then
    set new.exam_mark=null;
    end if;
end//
delimiter ;
drop trigger cr_check;
update exams
set exam_mark=7
where exam_id=8;
