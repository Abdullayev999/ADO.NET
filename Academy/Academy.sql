CREATE DATABASE Academy
USE Academy
 
 SELECT GETDATE() 

 DROP TABLE Student

 SELECT * FROM Student

CREATE TABLE Student(
Id INT PRIMARY KEY IDENTITY,
FirstName NVARCHAR(30) NOT NULL,
LastName NVARCHAR(30) NOT NULL,
BirthDay DATE NOT NULL,
Coins INT NOT NULL DEFAULT(0),
Username NVARCHAR(30) NOT NULL,
[PasswordHash] Text NULL,
Salt NVARCHAR(255) NULL,
LastModify DATETIME NOT  NULL DEFAULT(GETDATE()),
CONSTRAINT CK_Student_Coins CHECK (Coins >= 0))

CREATE TABLE Product(
Id INT PRIMARY KEY IDENTITY,
Name NVARCHAR(30) NOT NULL,
Price INT NOT NULL,
Quantity INT NOT NULL,
CONSTRAINT CK_Product_Price CHECK (Price >= 0),
CONSTRAINT CK_Product_Quantity CHECK (Quantity >= 0))

SELECT Id,Name,Price,Quantity FROM Product

UPDATE Product
SET Name = @name , Price = @price , Quantity = @quantity
WHERE Id = @id

INSERT INTO Product(Name,Price,Quantity) VALUES ()
select * from Product
SELECT Id,FirstName,LastName,BirthDay,Coins,Username,PasswordHash,Salt FROM Student

INSERT INTO Student(FirstName, LastName, BirthDay, UserName)
VALUES ('Ivan', 'Ivanov', '1997-01-01', 'ivan_iv123')
SELECT SCOPE_IDENTITY()

DROP TABLE Student

 delete AuthenticationUsers

 CREATE TABLE AuthenticationUsers
 (	
	Token uniqueidentifier PRIMARY KEY,
	StudentId INT NOT NULL,

	CONSTRAINT FK_AuthenticationUsers_StudentId FOREIGN KEY (StudentId) REFERENCES Student(Id),
	CONSTRAINT UQ_AuthenticationUsers_StudentId UNIQUE(StudentId)
 )