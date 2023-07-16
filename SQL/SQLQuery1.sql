use PruebaBig_WillianFerneyCuartasMesa


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


insert into cities values('Bogotá','Cundinamarca'),('Medellin','Antioquia'),('Cali','Valle del cauca'),('Pasto','Nariño')

select * from cities

