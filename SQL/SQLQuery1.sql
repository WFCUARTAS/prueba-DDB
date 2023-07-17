Create database prueba_Back
go
use prueba_Back
go
/*
use PruebaBig_WillianFerneyCuartasMesa;
*/

create table users(
	Id int IDENTITY(1,1) PRIMARY KEY,
	Email varchar(50) not null unique,
	Password VARBINARY(128) not null,
	FullName varchar(50) not null,
	UserRol varchar(10) not null
)


create table cities(
	Id int IDENTITY(1,1) PRIMARY KEY,
	Name varchar(50),
	Departament varchar(50)
)

create table forecasts(
	Id int IDENTITY(1,1) PRIMARY KEY,
	Title varchar(50),
	DateClima date not null,
	MinTemperature int not null,
	MaxTemperature int not null,
	RainProbability int not null,
	Observation varchar(200),
	IdCity int,
	IdUserChage int not null,
	DateChange datetime not null
)



ALTER TABLE forecasts
ADD FOREIGN KEY (IdCity) REFERENCES cities(Id);


ALTER TABLE forecasts
ADD CONSTRAINT UniqueCitiDate UNIQUE (DateClima,IdCity);


insert into cities values('Bogotá','Cundinamarca'),('Medellin','Antioquia'),('Cali','Valle del cauca'),('Pasto','Nariño');

insert into users (Email,Password,FullName,UserRol) values ('admin@mail.com',HASHBYTES('SHA2_512', '12345'),'Administrador','Admin');


insert into forecasts (Title,DateClima,MinTemperature,MaxTemperature,RainProbability,Observation,IdCity,IdUserChage,DateChange)
values ('lluvia','2023-07-17',10,15,20,'lluvias leves',1,1,GETDATE()),
('soleado','2023-07-17',20,30,15,'dia de pisscina',2,1,GETDATE()),
('nublado','2023-07-17',15,20,50,'',3,1,GETDATE()),
('lluvia fuerta','2023-07-17',10,25,60,'usa paraguas',4,1,GETDATE());