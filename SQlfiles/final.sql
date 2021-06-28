create database OryoliReshka;

use OryoliReshka;

create table host
(
h_id int primary key auto_increment,
h_name varchar(20) not null,
h_surname varchar(20) not null,
h_bday date not null,
h_sex char(1) not null
);

create table sponsor
(
sp_id int primary key auto_increment,
sp_comp varchar(45) not null,
sp_product varchar(45) not null
);

create table city
(
c_id int primary key auto_increment,
c_country varchar(20) not null,
c_city varchar(20) not null
);
alter table city modify c_city varchar(50) not null;
alter table city modify c_country varchar(50) not null;
create table project
(
p_id int primary key auto_increment,
p_name varchar(30) not null,
p_start date not null, 
p_end date not null
);

create table program
(
pr_id int primary key auto_increment,
p_id int,
city_id int not null,
pr_date date not null,
pr_len time not null, 
constraint cn1 foreign key(p_id) references project(p_id) on delete cascade,
constraint cn2 foreign key(city_id) references city(c_id) on delete cascade
);

create table host_pr
(
h_id int, 
pr_id int, 
primary key(h_id, pr_id),
constraint cn3 foreign key(h_id) references host(h_id) on delete cascade, 
constraint cn4 foreign key(pr_id) references program(pr_id) on delete cascade
);

create table sp_program
(
sp_id int, 
pr_id int, 
primary key(sp_id, pr_id),
constraint cn5 foreign key(sp_id) references sponsor(sp_id) on delete cascade,
constraint cn6 foreign key(pr_id) references program(pr_id) on delete cascade
);

create table trip
(
tr_id int primary key auto_increment,
tr_start date not null,
tr_end date not null,
constraint cn7 foreign key(tr_id) references program(pr_id)
);
alter table trip add constraint cn7 foreign key(tr_id) references program(pr_id);
alter table trip modify tr_id int primary key;

create table costs
(
c_id int primary key auto_increment,
tr_id int not null,
c_name varchar(45) not null, 
c_type enum('flight', 'accommodation', 'food','entertainment', 'transpost') not null,
c_price double not null,
constraint cn8 foreign key(tr_id) references trip(tr_id) on delete cascade
);

create table crew
(
cr_id int primary key auto_increment,
cr_name varchar(20) not null,
cr_surname varchar(20) not null,
cr_post varchar(20) not null
);

create table tr_crew
(
tr_id int, 
cr_id int, 
primary key(tr_id, cr_id),
constraint cn9 foreign key(tr_id) references trip(tr_id) on delete cascade,
constraint cn10 foreign key(cr_id) references crew(cr_id) on delete cascade
);

select * from host;
insert into host (h_name, h_surname, h_bday, h_sex) values
('Anastasia','Ivleeva', '1991-03-08','f'),
('Regina', 'Todorenko', '1990-06-14', 'f'),
('Anton', 'Ptushkin', '1984-05-22', 'm'), 
('Lesya', 'Nikityuk', '1984-10-19', 'f'),
('Nikolai', 'Serga', '1984-11-21', 'm'), 
('Andrei', 'Bednyakov', '1987-03-21', 'm'), 
('Evgeni', 'Sinelnikov', '1981-11-03', 'm'),
('Alina', 'Astrovskaya', '1989-10-18', 'f');

select * from sponsor;
insert into sponsor (sp_comp, sp_product) values
('honor', 'phone'), 
('pharmstandard', 'medicine');

insert into OryoliReshka.city(c_city, c_country) select * from world.temp;
select * from city;

SET SQL_SAFE_UPDATES = 0;

select * from project;
insert into project(p_name, p_start, p_end) values
('Russia', '2018-10-14', curdate()), 
('Rai i ad', '2017-02-01', '2017-10-01'), 
('Perezagruzka', '2017-09-01', '2018-01-01');

select * from crew;
insert into crew (cr_name, cr_surname, cr_post) values
('Sergei', 'Simanchuk', 'screenwriter'), 
('Taisia', 'Yusupova', 'screenwriter'), 
('Maksim', 'Storozhuk', 'screenwriter'), 
('Konstantin', 'Loboda', 'operator'), 
('Oleg', 'Avilov', 'operator'), 
('Evgeniy', 'Sinelnikov', 'director'), 
('Elena', 'Sinelnikova', 'director'), 
('Nikita', 'Chizhov', 'director');

select * from city where c_id=1253;

select * from program;
insert into program (p_id, city_id, pr_date, pr_len) values
(null, 98, '2019-10-10', '45:10:00'),
(2, 1253, '2018-10-10', '39:14:00'), 
(1, 3151, '2020-09-11', '50:00:00'), 
(null, 998, '2020-05-05', '46:15:00'), 
(3, 533, '2017-07-07', '47:54:00'), 
(3, 2027, '2017-06-24', '43:12:00');

select * from host;
insert into host_pr () values
(1, 1), (3, 1), (8, 2), (7, 2), (5, 3), (2, 3), (4, 4), (2, 4), (1, 5), (6, 5), (1, 6), (2, 6);

insert into sp_program () values 
(1, 2), (2, 2), (1, 5), (2, 5), (1, 3);

select * from trip;
insert into trip(tr_start, tr_end) values 
('2019-09-13', '2019-09-30'), 
('2018-08-21', '2018-09-01'), 
('2020-08-07', '2020-08-19'), 
('2020-04-17', '2020-05-01'), 
('2017-06-06', '2017-06-28'), 
('2017-05-29', '2017-06-03');

select * from crew;
insert into tr_crew () values
(1, 2), (1, 4), (1, 7), 
(2, 1), (2, 5), (2, 8),
(3, 3), (3, 4), (3, 7),
(4, 2), (4, 4), (4, 8),
(5, 1), (5, 5), (5, 6),
(6, 3), (6, 4), (6, 6);

select * from costs;
insert into costs (tr_id, c_name, c_type, c_price) values
(1, 'flight moscow-wien', 'flight', 5000), 
(1, 'hotel', 'accommodation', 3000), 
(2, 'flight moscow-paris', 'flight', 6000), 
(3, 'hotel', 'accommodation', 4000);

#indexes
create index cities on city(c_city(3));
create index cost on costs(c_name(4));

#views
create or replace view product_manager
(date, length, city, project) as
select pr_date, pr_len, c_city, p_name from program
inner join city on
city_id=c_id
left join project on
project.p_id=program.p_id;

select * from product_manager;	


create or replace view sponsors
(program, pr_date, company, product) as
select program.pr_id,pr_date, sp_comp, sp_product from program
inner join sp_program
on program.pr_id=sp_program.pr_id
inner join sponsor
on sponsor.sp_id=sp_program.sp_id;

select * from sponsors;

select * from trip;

create user 'ceo'@'localhost' identified by 'ceopass123';
grant all privileges on OryoliReshka.* to 'ceo'@'localhost';
grant select on program.* to 'dir'@'localhost';

create user 'me1'@'localhost' identified by 'mepass123';
grant all privileges on OryoliReshka.* to 'me1'@'localhost';
show grants for 'me1'@'localhost';

create user 'manager1'@'localhost' identified by 'manpass123';
grant select on program.* to 'manager1'@'localhost';
grant select on host_pr.* to 'manager1'@'localhost';
grant select on host.* to 'manager1'@'localhost';

create user 'adminstrator'@'localhost' identified by 'adminpass123';
grant insert,update, delete on trip.* to 'adminstrator'@'localhost';
grant insert,update, delete on cost.* to 'adminstrator'@'localhost';

create user 'crew'@'localhost' identified by 'crewpass123';
grant insert,update, delete on trip.* to 'crew'@'localhost' ;
grant insert,update, delete on tr_crew.* to 'crew'@'localhost' ;
grant insert,update, delete on crew.* to 'crew'@'localhost' ;

delimiter ;;
create trigger checker1 before insert on trip
for each row 
begin
declare d date;
	if new.tr_start>new.tr_end then
		set d=new.tr_start;
        set new.tr_start=new.tr_end;
        set new.tr_end=d;
    end if;
end ;;
delimiter ;

delimiter ;;
create trigger checker2 before insert on costs
for each row 
begin
	if new.c_price<0 then
		set new.c_price=(-1)*new.c_price;
    end if;
end ;;
delimiter ;
select * from costs;

delimiter ;;
create trigger checker3 before insert on host_pr
for each row 
begin
	if (select count(pr_id) from host_pr where pr_id=new.pr_id)=2 then
    signal sqlstate '45000';
    end if;
end ;;
delimiter ;

select * from host_pr;
insert into host_pr () values (2, 1);

delete from host_pr where h_id=2 and pr_id=1;

insert into costs (tr_id, c_name, c_type, c_price)
values ('3', 'flights', 'flight', -10000);

delimiter ;;
create procedure cost(in tr int)
begin
select sum(c_price) from costs
where tr_id=tr;
end;;
delimiter ;

call cost(1);

select * from program;

insert into program (p_id, city_id, pr_date, pr_len) values
(null, 99, '2020-12-12', '49:34:00');

delimiter ;;
create procedure autohost(in pr int)
begin
declare i int;
start transaction;
	if (select count(pr_id) from host_pr where pr_id=pr)=0 then 
		insert into host_pr () 
        select h_id, pr from host
        order by rand()
        limit 2;
	elseif (select count(pr_id) from host_pr where pr_id=pr)=1 then
		select pr_id into i from host_pr where pr_id=pr;
        insert into host_pr () 
        select h_id, pr from host
        where h_id<>i
        order by rand()
        limit 1;
	end if;
    commit;
end;;
delimiter ;

call autohost(6);
delete from host_pr where pr_id=6 and h_id=3;

select * from host_pr;
delimiter ;;
create procedure mostpopular()
begin
	select c_country from program
	inner join city
	on city_id=c_id
	group by c_country 
	order by count(c_country) desc
	limit 1;
end;;
delimiter ;

call mostpopular();

delimiter ;;
create procedure sale(in c int, in per double)
begin
	start transaction;
    update costs
    set c_price=(100-per)/100*c_price
    where c_id=c;
    commit;
end;;
delimiter ;

select * from host_pr;
delimiter ;;
create procedure q_pr(in h int, in pr_name varchar(20))
begin
select count(h_id)from host_pr
inner join program
on program.pr_id=host_pr.pr_id
inner join project
on project.p_id=program.p_id
where h_id=h and p_name=pr_name;
end;;
delimiter ;

call q_pr(1, 'Perezagruzka');
select * from sp_program;

delimiter ;;
create procedure sp_proj()
begin
select count(program.pr_id) as k, p_name from sp_program
inner join sponsor
on sponsor.sp_id=sponsor.sp_id
inner join program
on program.pr_id=sp_program.pr_id
inner join project
on project.p_id=program.p_id
group by p_name
order by k desc;
end;;
delimiter ;

call sp_proj();

delimiter ;;
create procedure autopr(in d date)
begin
declare city, country, proj, host1, host2 varchar(40);
select p_name into proj from project
where d>p_start and d<p_end;
if proj='Russia' then	
	select c_city, c_country into city, country from city 
	where c_country='Russian Federation'
	order by rand()
	limit 1;
else 
	select c_city, c_country into city, country from city 
	order by rand()
	limit 1;
end if;
select h_surname into host1 from host
order by rand() limit 1;
select h_surname into host2 from host
where h_surname<>host1 order by rand() limit 1;
select proj, city, country, d, host1, host2;
end;;
delimiter ;

call autopr('2020-12-14');

select * from program
inner join city on
city_id=c_id;

create table description
(
d_id int primary key, 
d_text varchar(300),
constraint cn13 foreign key(d_id) references program(pr_id)
);

insert into description () values
(1, "Орёл и Решка в Париже! Париж... он прекрасен, здесь и говорить не о чем. Даже жизнь на 100 долларов не сможет испортить столь романтическое и возвышенное настроение. Кому же повезет с золотой картой и сколько потратят ведущие?"), 
(2, "Съемки в Вене начались с мокрых ног, зато закончились исцелением! Наши ведущие больше высоты не боятся. Наверное... Австрия, мы не прощаемся!"), 
(3, "Ведущие Орла и Решки уже много раз были в Москве, но в очередной раз найдут что посмотреть в столице."), 
(4, "Орёл и Решка в Берлине! В Берлине найдется место для каждого, а свобода — синоним этой столицы. Не забываем и о том, что интересно путешествовать можно и на сто долларов, и наши ведущие докажут это."), 
(5, "Мы выбрали “лучшее” время, чтобы приехать в Шанхай. Изнывающие от жары ведущие, толпы людей, которые не дают нашим операторам снимать… Но мы справились! И повеселились! Смотрим видео!"), 
(6, "Орёл и Решка. НА КРАЮ СВЕТА в Токио, Япония! Это другая планета, параллельная реальность, во многом шокирующая и непонятная, но прекрасная страна!");

select * from description;
create fulltext index text_ind on description(d_text);