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



EXEC SP_PostForecast
    @Title = 'Título del pronóstico citi2',
    @DateClima = '2023-07-20',
    @MinTemperature = 30,
    @MaxTemperature = 40,
    @RainProbability = 0.2,
    @Observation = 'Observaciones del clima',
    @IdCity = 1,
    @IdUserChange = 123



SELECT * FROM forecasts

///////* procedimiento editar clima */

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


EXEC SP_PutForecast
    @id=1,
    @Title = 'Título del pronóstico edit',
    @DateClima = '2023-07-16',
    @MinTemperature = 20,
    @MaxTemperature = 30,
    @RainProbability = 0.3,
    @Observation = 'Observaciones del clima',
    @IdCity = 2,
    @IdUserChange = 122

SELECT * FROM forecasts

/*CONSULTAR POR ID */
CREATE OR ALTER PROCEDURE SP_GetForecast
    @Id int
AS
BEGIN
    SELECT Id, Title, DateClima, MinTemperature, MaxTemperature, RainProbability, Observation, IdCity
    FROM  forecasts WHERE Id = @Id
END

EXEC SP_GetForecast
    @Id=1

/****************************CONSULTAS**********************/

/*CONSULTAR POR FECHA */
CREATE OR ALTER PROCEDURE SP_ListDateForecast
    @DateClima DATE
AS
BEGIN
    SELECT f.Id, f.Title, f.DateClima, f.MinTemperature, f.MaxTemperature, f.RainProbability, f.Observation, f.IdCity, c.Name AS 'CityName' 
    FROM  forecasts AS f, cities As c WHERE f.IdCity=c.Id AND DateClima = @DateClima
END

EXEC SP_ListDateForecast
    @DateClima = '2023-07-16'


	
/*CONSULTAR POR Ciudad */
CREATE OR ALTER PROCEDURE SP_ListCityForecast
    @IdCity int
AS
BEGIN
    SELECT TOP 5 Id, Title, DateClima, MinTemperature, MaxTemperature, RainProbability, Observation, IdCity 
    FROM  forecasts WHERE IdCity = @IdCity AND DateClima>= CAST(GETDATE() AS DATE) ORDER BY DateClima 
END

EXEC SP_ListCityForecast
    @IdCity = 1



/*CONSULTAR POR Ciudad y fecha */
CREATE OR ALTER PROCEDURE SP_ListCityDateForecast
    @IdCity int,
	@DateClima DATE
AS
BEGIN
    SELECT f.Id, f.Title, f.DateClima, f.MinTemperature, f.MaxTemperature, f.RainProbability, f.Observation, f.IdCity, c.Name AS 'CityName'
    FROM  forecasts AS f, cities As c WHERE f.IdCity=c.Id AND IdCity = @IdCity AND DateClima = @DateClima
END

EXEC SP_ListCityDateForecast
    @IdCity = 1,
	@DateClima = '2023-07-17'


/*************************PROCEDIMIENTOS PARA LA CIUDAD***************************/

CREATE OR ALTER PROCEDURE SP_PostCity
    @Name VARCHAR(50),
    @Departament VARCHAR(50)
AS
BEGIN
    INSERT INTO cities(Name, Departament)
    VALUES (@Name , @Departament)
END

EXEC SP_PostCity
    @Name = 'Ibague',
	@Departament = 'Tolima'

select * from cities