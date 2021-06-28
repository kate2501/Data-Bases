create database lr3;

use lr3;

drop table students;

create table students
(
	stCard int primary key,
	firstName varchar(10),
    secondName varchar(20),
    age int,
    avgScore double,
    rating int,
    scolarship double,
    fee double,
    sex enum('male', 'female'),
    favSubject set('DB', 'KM', 'Algebra', 'Calculus', 'Geometry', 'MPaI', 'DE')
);

insert into students values
(1922136, 'Ilya', 'Borovsky', 18, 8.7, 5, 155.64, null, 'male', 'DB,MPaI,Algebra'),
(1922113, 'Katya', 'Belskaya', 18, 9.6, 2, 171.64, null, 'female', 'Calculus'),
(1922117, 'Nastya', 'Savchenko', 18, 8.7, 4, 171.64, null, 'female', 'KM,Geometry'),
(1922118, 'Levi', 'Taborov', 18, 8.3, null, 155.64, null, 'male', 'MPaI'),
(1922119, 'Arina', 'Logvinenko', 18, 7.8, null, 155.64, null, 'female', 'DB,Algebra,Geometry'),
(1922120, 'Edgar', 'Vasilevski', 18, 8.55, 6, 155.64, null, 'male', 'DB,KM,MPaI'),
(1922121, 'Alexey', 'Karlovich', 18, 7.8, null, 155.64, null, 'male', 'DB,KM'),
(1922122, 'Kirill', 'Stankevich', 18, 7, null, 155.64, null, 'male', 'DB,KM'),
(1922123, 'Elena', 'Zrazikova', 18, 7.6, null, 155.64, null, 'female', 'Algebra,Geometry'),
(1922087, 'Katya', 'Labun', 18, 7.2, null, 155.64, null, 'female', 'DE'),
(1922124, 'Matvey', 'Draevich', 18, 7.6, null, null, 3293.0, 'male', 'KM'),
(1922287, 'Egor', 'Buksa', 18, 6.4, null, null, 3293.0, 'male', 'DB');

insert into students values
(1922288, 'Egor', 'Aad', 18, 6.4, null, 100, null, 'male', 'DB');
select * from students;

alter table students change avgScore avgMark float after rating;
alter table students change rating rate tinyint after stCard;
alter table students change fee stFee float after avgMark;

set sql_safe_updates = 0;

update students
set scolarship = 1.1*scolarship;

update students
set stFee = 1.15*stFee;

update students 
set scolarship=1.2*scolarship
where
length(regexp_replace(lower(secondName), '[euoai]',''))<
length(secondName)-length(regexp_replace(lower(secondName), '[euoai]',''));

update students 
set scolarship=1.2*scolarship
where
length(regexp_replace(cast(stCard as char), '[369]',''))<
length(regexp_replace(cast(stCard as char), '7',''));

select stCard, firstName, secondName from students
where sex="female";

create table girls
(
	stCard int primary key,
	firstName varchar(10),
    secondName varchar(20),
    constraint cn1 foreign key(stCard) references students(stCard)
);

insert into girls
select stCard, firstName, secondName from students
where sex="female";

select * from girls;

create table boys
(
	stCard int primary key,
	firstName varchar(10),
    secondName varchar(20),
    constraint cn2 foreign key(stCard) references students(stCard)
);

insert into boys
select stCard, firstName, secondName from students
where sex="male";

select * from boys;


select * from students;

#dop----------------------------------------------------
update students 
set scolarship=(1+(length(secondName)-length(regexp_replace(lower(secondName), '[euoai]','')))/100)*scolarship;
-----------------------------------------------
set @avgm = (select avg(avgMark) from students);

update students
set scolarship=1.2*scolarship
where avgMark>@avgm;

select * from students;
------------------------------------------------
select date_add(curdate(), interval (((6-dayofweek(curdate())) % 7)+7) % 7 day);
select date_add('2020-11-27', interval (((6-dayofweek('2020-11-27')) % 7)+7) % 7 day);
drop procedure finddate;

delimiter $$
create procedure finddate()
begin
	declare x date;
	set x=date_add(curdate(), interval (((6-dayofweek(curdate())) % 7)+7) % 7 day);
	my_loop: loop
		if day(x)=13 then leave my_loop;
		end if;
		set x=date_add(date_add(x, interval 1 day), 
        interval (((6-dayofweek(date_add(x, interval 1 day))) % 7)+7) % 7 day);
	end loop;
select x;
end$$
delimiter ;

call finddate();
#2.1----------------------------------------------------
create table bookstore
(
	author1 varchar(50) not null,
    author2 varchar(50),
    title varchar(20),
    isbn char(10) not null,
    price double not null,
    customer_name varchar(20) not null,
    customer_address varchar(50),
    purchase_date date
);

drop table bookstore;

insert into bookstore values
('David Sklar', 'Adam Trachtenberg', 'PHP Cookbook', '0596101015', 44.99, 'Emme Brown', '1565 Rainbow Road, Los Angeles, CA 90014', '2009-03-03'),
('Danny Goodman', null, 'Dynamic HTML', '0596527403', 59.99, 'Darren Ryder', '4758 Emily Drive, Richmond, VA 23219', '2008-12-19'),
('Hudh E. Williams', 'David Lane', 'PHP and MySQL', '0596005436', 44.95, 'Earl B. Thurston', '862 Gregory Lane, Frankfort, KY 40601', '2009-06-22'),
('David Sklar', 'Adam Trachtenberg', 'PHP Cookbook', '0596101015', 44.99, 'Darren Ryder', '4758 Emily Drive, Richmond, VA 23219', '2008-12-19'),
('Rasmus Lerdorf', 'Kevin Tatroe & Peter MacIntyre', 'Programming PHP', '0596006815', 39.99, 'David Miller', '3647 Cedar Lane, Waltham, MA 02154', '2009-01-16');

select * from bookstore;

create table books
(
id int primary key auto_increment,
title varchar(30) not null,
isbn int
);

insert into books 
(title, isbn)
select distinct title, isbn
from bookstore;

select * from books;

create table authors
(
author varchar(30)
);

drop table authors;

insert into authors
(author)
select distinct author2
from bookstore
where author2 is not null and author2 not like "Kevin%";

insert into authors
(author)
select trim(substring_index(author2, '&', 1))
from bookstore
where author2 like "Kevin%";

insert into authors
(author)
select 
trim(right(author2, length(author2)-length(trim(substring_index(author2, '&', 1)))-3))
from bookstore
where author2 like "Kevin%";

insert into authors
(author)
select distinct author1
from bookstore;

select * from authors;
alter table authors add column id int primary KEY AUTO_INCREMENT;

create table books_authors
(
book_id int not null,
author_id int not null,
primary key(book_id, author_id),
constraint cnb foreign key(book_id) references books(id),
constraint cna foreign key(author_id) references authors(id)
);

insert into books_authors ()
values
(1,1), (1,5), (2,6), (3,2),(3,7),(4,3),(4,4),(4,8);

select * from books_authors;

create table purchases
(
id int primary key auto_increment,
price int,
purchase_date date,
book_id int,
customer_id int,
constraint bp foreign key(book_id) references books(id),
constraint cc foreign key(customer_id) references customers(id)
);

select * from purchases;

insert into purchases (price, purchase_date, customer_id, book_id)
(select price,purchase_date, customers.id, books.id from bookstore
inner join customers
on customer_address=cust_address
left join books
on bookstore.title=books.title);

create table customers
(
cust_name varchar(30),
cust_address varchar(50)
);

insert into customers 
(cust_name, cust_address)
select distinct customer_name, customer_address from bookstore;

alter table customers add column id int primary KEY AUTO_INCREMENT first;

select * from customers;
#2.2----------------------------------------------------
create table kids
(
	emp_id int primary key,
    first_name varchar(4),
    second_name varchar(5),
    child_name varchar(20),
    child_birth varchar(50)
);
drop table kids;
insert into kids values
(1001, 'Jane', 'Doe', 'Mary, Sam', '1992-01-01, 1994-05-15'),
(1002, 'John', 'Doe', 'Mary, Sam', '1992-01-01, 1994-05-15'),
(1003, 'Jane', 'Smith', 'John, Pat, Lee, Mary', '1994-10-05, 1990-10-12, 1996-06-06, 1994-08-21'),
(1004, 'John', 'Smith', 'Michael', '1996-07-04'),
(1005, 'Jane', 'Jones', 'Edward, Martha', '1995-10-21, 1989-10-15');

alter table kids
add column c1 char(10);

update kids
set c1 = substring_index(child_name, ', ', 1);

alter table kids
add column c2 char(10);

update kids
set c2 = right(substring_index(child_name, ', ', 2), length(substring_index(child_name, ', ', 2))-length(c1)-2);

alter table kids
add column b1 char(10);

update kids
set b1 = substring_index(child_birth, ', ', 1);

alter table kids
add column b2 char(10);

update kids
set b2 = right(substring_index(child_birth, ', ', 2), length(substring_index(child_birth, ', ', 2))-length(b1)-2);

create table children
(
cname varchar(10),
birth varchar(10)
);
select * from kids;
drop table children;
insert into children
select distinct c1, b1 from kids;

insert into children
select distinct c2, b2 from kids where c2!='';

insert into children
select 
right(substring_index(child_name, ', ', 3),3),
right(substring_index(child_birth, ', ', 3),10) from kids where emp_id=1003;

insert into children
select
right(substring_index(child_name, ', ', 4),4),
right(substring_index(child_birth, ', ', 4),10) from kids where emp_id=1003;

select * from children;
alter table children add column id int primary KEY AUTO_INCREMENT first;

select * from kids;

select * from children;

create table parents
(
id int primary key,
name varchar(10),
surname varchar(10)
);

insert into parents ()
select emp_id,first_name, second_name from kids;

select * from parents;

create table kids_par
(
child_id int, 
par_id int,
constraint ch foreign key(child_id) references children(id),
constraint par foreign key(par_id) references parents(id)
);
alter table kids_par add primary key(child_id, par_id);
insert into kids_par
select children.id, emp_id from children,kids
where kids.child_name like CONCAT('%',children.cname, '%') and kids.child_birth like CONCAT('%',children.birth, '%');
#3.3----------------------------------------------------

create table court
(
	court_num enum('1', '2'),
    start_time time,
    finish_time time,
    tariff varchar(10)
);

insert into court values
('1', '09:30:00', '10:30:00', 'Бережливый'),
('1', '11:00:00', '12:00:00', 'Бережливый'),
('1', '14:00:00', '15:30:00', 'Стандарт'),
('2', '10:00:00', '11:30:00', 'Премиум-B'),
('2', '11:30:00', '13:30:00', 'Премиум-B'),
('2', '15:00:00', '16:30:00', 'Премиум-A');

select * from court;

create table tariffs
(
tariff varchar(10),
court_num enum('1', '2')
);

insert into tariffs
select distinct tariff, court_num from court;

select * from tariffs;
alter table tariffs add primary key(tariff);

create table booking
(
tariff varchar(10),
start_time time,
finish_time time
);

insert into booking
select tariff, start_time, finish_time from court;
alter table booking add constraint boo foreign key(tariff) references tariffs(tariff);
select * from booking; 

drop table court;
#3.1----------------------------------------------------
create table movie
(
	title varchar(15),
    star varchar(20),
    producer varchar(20)
);

insert into movie values
('Great Film', 'Lovely Lady', 'Money Bags'),
('Great Film', 'Handsome Man', 'Money Bags'),
('Great Film', 'Lovely Lady', 'Helen Pursestrings'),
('Great Film', 'Handsome Man', 'Helen Pursestrings'),
('Boring Movie', 'Lovely Lady', 'Helen Pursestrings'),
('Boring Movie', 'Precocious Child', 'Helen Pursestrings');

select * from movie;

create table stars
(
    star varchar(20)
);

insert into stars
select distinct star from movie;
alter table stars add column id int primary KEY AUTO_INCREMENT first;
select * from stars;

create table producers
(
    producer varchar(20)
);

insert into producers
select distinct producer from movie;
alter table producers add column id int primary KEY AUTO_INCREMENT first;
select * from producers;

create table films
(
title varchar (20)
);

insert into films
select distinct title from movie;
alter table films add column id int primary KEY AUTO_INCREMENT first;
select * from films;
select * from stars;

create table stars_films
(
star_id int, 
film_id int,
constraint star foreign key(star_id) references stars(id),
constraint film foreign key(film_id) references films(id)
);
alter table stars_films add primary key(star_id, film_id);

insert into stars_films select distinct stars.id, films.id from movie
inner join stars
on stars.star=movie.star
inner join films
on films.title=movie.title;

create table producer_films
(
producer_id int, 
film_id int,
constraint prod foreign key(producer_id) references producers(id),
constraint film1 foreign key(film_id) references films(id)
);

alter table producer_films add primary key(producer_id, film_id);
insert into producer_films select distinct producers.id, films.id from movie
inner join producers
on producers.producer=movie.producer
inner join films
on films.title=movie.title;
select * from producer_films;
#3.2----------------------------------------------------

create table study
(
	surname enum('A', 'B'),
    course varchar(20),
    workbook varchar(20)
);

insert into study values
('A', 'Информатика', 'Информатика'),
('A', 'Сети ЭВМ', 'Информатика'),
('A', 'Информатика', 'Сети ЭВМ'),
('A', 'Сети ЭВМ', 'Сети ЭВМ'),
('B', 'Программирование', 'Программирование'),
('B', 'Программирование', 'Теория алгоритмов');

select * from study;

create table courses
(
    course varchar(20)
);

insert into courses (course)
select distinct course from study;

alter table courses add column id int primary KEY AUTO_INCREMENT first;
select * from courses;

create table st
(
surname varchar(1)
);

insert into st (surname)
select distinct surname from study;
alter table st add column id int primary KEY AUTO_INCREMENT first;
select * from st;

create table workbooks
(
    books varchar(20)
);

insert into workbooks (books)
select distinct workbook from study;

alter table workbooks add column id int primary KEY AUTO_INCREMENT first;
select * from workbooks;

create table st_courses
(
sur_id int,
course_id int,
constraint sur foreign key(sur_id) references st(id),
constraint cou foreign key(course_id) references courses(id)
);

alter table st_courses add primary key(sur_id, course_id);

create table st_books
(
sur_id int,
book_id int,
constraint ss foreign key(sur_id) references st(id),
constraint bb foreign key(book_id) references workbooks(id)
);

alter table st_books add primary key(sur_id, book_id);

insert into st_courses select distinct st.id, courses.id from study
inner join st
on st.surname=study.surname
inner join courses
on courses.course=study.course;

select * from st_courses;

insert into st_books select distinct st.id, workbooks.id from study
inner join st
on st.surname=study.surname
inner join workbooks
on workbooks.books=study.workbook;

select * from st_books;

#3.4----------------------------------------------------
create table products
(
	product_name varchar(30),
    supplier_name varchar(20),
    pack_size varchar(5)
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

create table suppliers
(
product_name varchar(30),
supplier_name varchar(20)
);

insert into suppliers
select distinct product_name, supplier_name from products;

select * from suppliers;

create table sizes
(
pack_size varchar(30),
product_name varchar(30)
);

insert into sizes
select pack_size, product_name from products;

select * from sizes;