Create database PruebaBig_WillianFerneyCuartasMesa
GO
use PruebaBig_WillianFerneyCuartasMesa
GO


create table users(
	Id int IDENTITY(1,1) PRIMARY KEY,
	Email varchar(50) not null unique,
	Password VARBINARY(128) not null,
	FullName varchar(50) not null,
	UserRol varchar(10) not null
)
GO

create table cities(
	Id int IDENTITY(1,1) PRIMARY KEY,
	Name varchar(50),
	Departament varchar(50)
)
GO
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

GO

ALTER TABLE forecasts
ADD FOREIGN KEY (IdCity) REFERENCES cities(Id);
GO

ALTER TABLE forecasts
ADD CONSTRAINT UniqueCitiDate UNIQUE (DateClima,IdCity);
GO

insert into cities values('Bogotá','Cundinamarca'),('Medellin','Antioquia'),('Cali','Valle del cauca'),('Pasto','Nariño');
GO
insert into users (Email,Password,FullName,UserRol) values ('admin@mail.com',HASHBYTES('SHA2_512', '12345'),'Administrador','Admin'),('user1@mail.com',HASHBYTES('SHA2_512', '6789'),'Administrador','User');
GO

insert into forecasts (Title,DateClima,MinTemperature,MaxTemperature,RainProbability,Observation,IdCity,IdUserChage,DateChange)
values ('lluvia','2023-07-17',10,15,20,'lluvias leves',1,1,GETDATE()),
('soleado','2023-07-17',20,30,15,'dia de pisscina',2,1,GETDATE()),
('nublado','2023-07-17',15,20,50,'',3,1,GETDATE()),
('lluvia fuerta','2023-07-17',10,25,60,'usa paraguas',4,1,GETDATE());

GO

/*
procedimientos
*/

go
CREATE OR ALTER PROCEDURE SP_PostForecast
    @Title VARCHAR(50),
    @DateClima DATE,
    @MinTemperature INT,
    @MaxTemperature INT,
    @RainProbability FLOAT,
    @Observation VARCHAR(200),
    @IdCity INT,
    @IdUserChange INT
AS
BEGIN
    INSERT INTO forecasts (Title, DateClima, MinTemperature, MaxTemperature, RainProbability, Observation, IdCity, IdUserChage, DateChange)
    VALUES (@Title, @DateClima, @MinTemperature, @MaxTemperature, @RainProbability, @Observation, @IdCity, @IdUserChange, GETDATE())
END
GO


/* procedimiento editar clima */

CREATE OR ALTER PROCEDURE SP_PutForecast
	@Id int,
    @Title VARCHAR(50),
    @DateClima DATE,
    @MinTemperature INT,
    @MaxTemperature INT,
    @RainProbability FLOAT,
    @Observation VARCHAR(200),
    @IdCity INT,
    @IdUserChange INT
AS
BEGIN
    UPDATE forecasts
    SET Title = @Title,
        DateClima = @DateClima,
        MinTemperature = @MinTemperature,
        MaxTemperature = @MaxTemperature,
        RainProbability = @RainProbability,
        Observation = @Observation,
        IdCity = @IdCity,
        IdUserChage = @IdUserChange,
        DateChange = GETDATE()
    WHERE Id = @Id
END
GO


/*CONSULTAR POR ID */
CREATE OR ALTER PROCEDURE SP_GetForecast
    @Id int
AS
BEGIN
    SELECT f.Id, f.Title, f.DateClima, f.MinTemperature, f.MaxTemperature, f.RainProbability, f.Observation, f.IdCity, c.Name AS 'CityName'
    FROM  forecasts AS f, cities As c WHERE f.Id = @Id AND f.IdCity=c.Id
END
GO


-------CONSULTAS-----

/*CONSULTAR POR FECHA */
CREATE OR ALTER PROCEDURE SP_ListByDateForecast
    @DateClima DATE
AS
BEGIN
    SELECT f.Id, f.Title, f.DateClima, f.MinTemperature, f.MaxTemperature, f.RainProbability, f.Observation, f.IdCity, c.Name AS 'CityName' 
    FROM  forecasts AS f, cities As c WHERE f.IdCity=c.Id AND DateClima = @DateClima
END
GO

	
/*CONSULTAR POR Ciudad */
CREATE OR ALTER PROCEDURE SP_ListCityForecast
    @IdCity int
AS
BEGIN
    SELECT TOP 5 Id, Title, DateClima, MinTemperature, MaxTemperature, RainProbability, Observation, IdCity 
    FROM  forecasts WHERE IdCity = @IdCity AND DateClima>= CAST(GETDATE() AS DATE) ORDER BY DateClima 
END
GO


/*CONSULTAR POR Ciudad y fecha */
CREATE OR ALTER PROCEDURE SP_ListCityDateForecast
    @IdCity int,
	@DateClima DATE
AS
BEGIN
    SELECT f.Id, f.Title, f.DateClima, f.MinTemperature, f.MaxTemperature, f.RainProbability, f.Observation, f.IdCity, c.Name AS 'CityName'
    FROM  forecasts AS f, cities As c WHERE f.IdCity=c.Id AND IdCity = @IdCity AND DateClima = @DateClima
END
GO


/*************************PROCEDIMIENTOS PARA LA CIUDAD***************************/

CREATE OR ALTER PROCEDURE SP_PostCity
    @Name VARCHAR(50),
    @Departament VARCHAR(50)
AS
BEGIN
    INSERT INTO cities(Name, Departament)
    VALUES (@Name , @Departament)
END
GO


CREATE OR ALTER PROCEDURE SP_GetCity
    @Id int
AS
BEGIN
    select * from cities where Id=@Id
END
GO



/*************************PROCEDIMIENTOS PARA USUARIOS***************************/
/*Metodo get**/
CREATE OR ALTER PROCEDURE SP_GetUser
    @Email VARCHAR(50)
AS
BEGIN
    SELECT Id, Email, FullName, UserRol
        FROM users
        WHERE Email = @Email;
END
GO


/*Metodo post*/
CREATE OR ALTER PROCEDURE SP_PostUser
    @Email VARCHAR(50),
    @Password VARCHAR(50),
	@FullName VARCHAR(50),
	@UserRol VARCHAR(10)
AS
BEGIN
    INSERT INTO users(Email, Password, FullName, UserRol)
    VALUES (@Email, HASHBYTES('SHA2_512', @Password), @FullName, @UserRol)
END
GO


/*Procedimiento para validar usuario*/
CREATE OR ALTER PROCEDURE SP_ValidateUser
    @Email VARCHAR(50),
    @Password VARCHAR(50)
AS
BEGIN
    
	DECLARE @PasswordHash VARBINARY(128);
	SET @PasswordHash = HASHBYTES('SHA2_512', @Password);

	IF EXISTS (
        SELECT *
        FROM users
        WHERE Email = @Email AND Password = @PasswordHash
    )
    BEGIN
        -- Usuario y contraseña válidos, retornar el registro del usuario
        SELECT Id, Email, FullName, UserRol
        FROM users
        WHERE Email = @Email;
    END
    ELSE
    BEGIN
        -- Usuario o contraseña incorrectos, retornar mensaje de error
        RAISERROR('Usuario o contraseña incorrectos.', 16, 1);
    END
END
GO


	 