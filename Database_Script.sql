USE [master];

DECLARE @kill varchar(8000) = '';  
SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'  
FROM sys.dm_exec_sessions
WHERE database_id  = db_id('NancyApi')

EXEC(@kill);

DROP DATABASE NancyApi
GO

CREATE DATABASE NancyApi
GO

USE NancyApi

CREATE TABLE Users
(UserId INT PRIMARY KEY IDENTITY(1,1),
"Password" VARCHAR(250),
UserName VARCHAR(80))
GO

DECLARE @UserCounter INT = 0
WHILE @UserCounter <10
BEGIN
	INSERT INTO Users ("Password", UserName) VALUES
	('1234', CONCAT('User', @UserCounter))
SET @UserCounter = @UserCounter+1
END
GO

SELECT * FROM Users
GO
