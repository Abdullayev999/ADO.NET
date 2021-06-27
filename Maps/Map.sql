--Создайте базу данных «Страны». 
--Нужно хранить такую информацию: 
--- Название страны; 
--- Название столицы; 
--- Количество жителей страны; 
--- Площадь страны; 
--- Часть света: Европа, Азия, Африка и т.д.
--- Количество жителей в столице; 
--- Названия крупных городов страны с количеством жителей в каждом городе.
 
CREATE DATABASE Map

GO
USE Map



CREATE TABLE Cities
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,
	CountPeople INT NOT NULL,

	CONSTRAINT UQ_Cities_Name UNIQUE([Name]),
	CONSTRAINT CK_Citiess_Name CHECK([Name] != ' '),
    CONSTRAINT CK_Cities_CountPeople CHECK(CountPeople >= 100)--Допустим мин для статуса города 100 должно быть
)



CREATE TABLE Continents
(
	Id INT PRIMARY KEY IDENTITY,
    [Name] NVARCHAR(50) NOT NULL,

	CONSTRAINT UQ_Continents_Name UNIQUE([Name]),
	CONSTRAINT CK_Continents_Name CHECK([Name] IN ('Africa','Antarctica','Asia','Europe','North America','South America','Australia')),
)



CREATE TABLE Countries
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,
	ContinentId INT NOT NULL,
	CapitalId INT NOT NULL,
	CountPeople INT NOT NULL DEFAULT(0), 
	Area FLOAT NOT NULL,
	
	
	CONSTRAINT CK_Countries_Area CHECK(Area >=50),--Допустим мин площадь 50km должно быть
	CONSTRAINT CK_Countries_Name CHECK([Name] != ' '), 
	CONSTRAINT UQ_Countries_Name UNIQUE([Name]),
	CONSTRAINT FK_Countries_CapitalId FOREIGN KEY (CapitalId) REFERENCES Cities(Id),
	CONSTRAINT UQ_Countries_CapitalId UNIQUE(CapitalId), --Столицы не должны повторяться
	CONSTRAINT FK_Countries_ContinentId FOREIGN KEY (ContinentId) REFERENCES Continents(Id)
)



CREATE TABLE CountryByCities
(
	Id INT PRIMARY KEY IDENTITY,
    CountryId INT NOT NULL, 
	CityId INT NOT NULL, 
	
	CONSTRAINT FK_CountryByCities_CountryId FOREIGN KEY (CountryId) REFERENCES Countries(Id),
	CONSTRAINT FK_CountryByCities_CityId FOREIGN KEY (CityId) REFERENCES Cities(Id)
)



CREATE TRIGGER CountsPeopleIncrease ON CountryByCities
AFTER INSERT
AS
BEGIN
	DECLARE @countPeople INT
	DECLARE @IdCountry INT

    SELECT @countPeople = Cities.CountPeople,@IdCountry =Countries.Id FROM inserted ,Countries, Cities
	WHERE inserted.CityId = Cities.Id AND inserted.CountryId = Countries.Id

	UPDATE Countries
	SET CountPeople = CountPeople + @countPeople
	WHERE Id = @IdCountry 
END



CREATE TRIGGER CountsPeopleDecrease ON CountryByCities
AFTER DELETE
AS
BEGIN
	DECLARE @countPeople INT
	DECLARE @IdCountry INT

    SELECT @countPeople = Cities.CountPeople,@IdCountry =Countries.Id FROM inserted ,Countries, Cities
	WHERE inserted.CityId = Cities.Id AND inserted.CountryId = Countries.Id

	UPDATE Countries
	SET CountPeople = CountPeople - @countPeople
	WHERE Id = @IdCountry 
END



insert into Cities ([Name], CountPeople) values ('Moscow', 34557511);
insert into Cities ([Name], CountPeople) values ('Baku', 24735506);
insert into Cities ([Name], CountPeople) values ('Kiev', 28778419);
insert into Cities ([Name], CountPeople) values ('Odessa', 12840438);
insert into Cities ([Name], CountPeople) values ('Paris', 47086103);
insert into Cities ([Name], CountPeople) values ('Ufa', 45718284);
insert into Cities ([Name], CountPeople) values ('New-York', 44594839);
insert into Cities ([Name], CountPeople) values ('Trabzon', 39387892);
insert into Cities ([Name], CountPeople) values ('London', 37272096);
insert into Cities ([Name], CountPeople) values ('Ankara', 17033928);
insert into Cities ([Name], CountPeople) values ('Guagua', 3054013);
insert into Cities ([Name], CountPeople) values ('Hlyboka', 1987222);
insert into Cities ([Name], CountPeople) values ('San Ricardo', 4023245);
insert into Cities ([Name], CountPeople) values ('Keles Timur', 4716554);
insert into Cities ([Name], CountPeople) values ('Hongshan', 2125886);
insert into Cities ([Name], CountPeople) values ('Faranah', 1215504);
insert into Cities ([Name], CountPeople) values ('Khānaqāh', 4076482);
insert into Cities ([Name], CountPeople) values ('Budrus', 2222805);
insert into Cities ([Name], CountPeople) values ('Znamenka', 3804564);
insert into Cities ([Name], CountPeople) values ('Nanxi', 2535271);
insert into Cities ([Name], CountPeople) values ('Heshan', 473451);
insert into Cities ([Name], CountPeople) values ('Kubangkondanglapangan', 1605053);
insert into Cities ([Name], CountPeople) values ('Xike', 2794671);
insert into Cities ([Name], CountPeople) values ('Saint-Leu-la-Forêt', 4300628);
insert into Cities ([Name], CountPeople) values ('Warungbuah', 3607990);
insert into Cities ([Name], CountPeople) values ('Jackson', 951436);
insert into Cities ([Name], CountPeople) values ('Mandal', 3862737);
insert into Cities ([Name], CountPeople) values ('Nakhon Sawan', 1524425);
insert into Cities ([Name], CountPeople) values ('Belford Roxo', 2679718);
insert into Cities ([Name], CountPeople) values ('Bayeux', 2931241);
insert into Cities ([Name], CountPeople) values ('Oebatu', 4184098);
insert into Cities ([Name], CountPeople) values ('Goulmima', 1365230);
insert into Cities ([Name], CountPeople) values ('Las Flores', 4253125);
insert into Cities ([Name], CountPeople) values ('Niš', 4587213);
insert into Cities ([Name], CountPeople) values ('Kolambugan', 311666);
insert into Cities ([Name], CountPeople) values ('Xinshi', 4686719);
insert into Cities ([Name], CountPeople) values ('Palmas De Gran Canaria, Las', 3238213);
insert into Cities ([Name], CountPeople) values ('Buenaventura', 4904960);
insert into Cities ([Name], CountPeople) values ('Tatsuno', 4029086);
insert into Cities ([Name], CountPeople) values ('Jiyizhuang', 1175592);
insert into Cities ([Name], CountPeople) values ('Mariano Moreno', 4046112);
insert into Cities ([Name], CountPeople) values ('Situ', 4764811);
insert into Cities ([Name], CountPeople) values ('Calvinia', 4375729);
insert into Cities ([Name], CountPeople) values ('El Obeid', 1367863);
insert into Cities ([Name], CountPeople) values ('Zalesnoye', 379909);
insert into Cities ([Name], CountPeople) values ('Pittsburgh', 3130111);
insert into Cities ([Name], CountPeople) values ('Heimahe', 2975812);
insert into Cities ([Name], CountPeople) values ('Kapyl’', 4553349);
insert into Cities ([Name], CountPeople) values ('Mpigi', 549832);
insert into Cities ([Name], CountPeople) values ('Miān Channūn', 4373218);


insert into Continents ([Name]) values ('Africa');
insert into Continents ([Name]) values ('Antarctica');
insert into Continents ([Name]) values ('Asia');
insert into Continents ([Name]) values ('Europe');
insert into Continents ([Name]) values ('North America');
insert into Continents ([Name]) values ('South America');
insert into Continents ([Name]) values ('Australia'); 


insert into Countries ([Name], ContinentId, CapitalId, Area) values ('Azerbaijan', 4, 2, 4957);
insert into Countries ([Name], ContinentId, CapitalId, Area) values ('Ukraine', 4, 3, 3086);
insert into Countries ([Name], ContinentId, CapitalId, Area) values ('Russia', 2, 1, 3050);
insert into Countries ([Name], ContinentId, CapitalId, Area) values ('Georgia', 5, 13, 3857);
insert into Countries ([Name], ContinentId, CapitalId, Area) values ('Chile', 1, 22, 1650);
insert into Countries ([Name], ContinentId, CapitalId, Area) values ('Japan', 2, 32, 353);
insert into Countries ([Name], ContinentId, CapitalId, Area) values ('Portugal', 4, 38, 2801); 
insert into Countries ([Name], ContinentId, CapitalId, Area) values ('Philippines', 5, 13, 741);
insert into Countries ([Name], ContinentId, CapitalId, Area) values ('America', 2, 9, 3533);
insert into Countries ([Name], ContinentId, CapitalId, Area) values ('Turkey', 2, 10, 3533);


insert into CountryByCities (CountryId, CityId) values (1, 17);
insert into CountryByCities (CountryId, CityId) values (7, 6);
insert into CountryByCities (CountryId, CityId) values (2, 25);
insert into CountryByCities (CountryId, CityId) values (7, 8);
insert into CountryByCities (CountryId, CityId) values (8, 5);
insert into CountryByCities (CountryId, CityId) values (10, 22);
insert into CountryByCities (CountryId, CityId) values (1, 32);
insert into CountryByCities (CountryId, CityId) values (6, 27);
insert into CountryByCities (CountryId, CityId) values (6, 21);
insert into CountryByCities (CountryId, CityId) values (2, 28);
insert into CountryByCities (CountryId, CityId) values (7, 14);
insert into CountryByCities (CountryId, CityId) values (10, 40);
insert into CountryByCities (CountryId, CityId) values (10, 12);
insert into CountryByCities (CountryId, CityId) values (10, 40);
insert into CountryByCities (CountryId, CityId) values (2, 11);
insert into CountryByCities (CountryId, CityId) values (8, 5);
insert into CountryByCities (CountryId, CityId) values (6, 32);
insert into CountryByCities (CountryId, CityId) values (8, 1);
insert into CountryByCities (CountryId, CityId) values (1, 27);
insert into CountryByCities (CountryId, CityId) values (9, 16);
insert into CountryByCities (CountryId, CityId) values (9, 21);
insert into CountryByCities (CountryId, CityId) values (3, 15);
insert into CountryByCities (CountryId, CityId) values (4, 10);
insert into CountryByCities (CountryId, CityId) values (3, 38);
insert into CountryByCities (CountryId, CityId) values (10, 21);
insert into CountryByCities (CountryId, CityId) values (4, 3);
insert into CountryByCities (CountryId, CityId) values (3, 15);
insert into CountryByCities (CountryId, CityId) values (7, 2);
insert into CountryByCities (CountryId, CityId) values (6, 37);
insert into CountryByCities (CountryId, CityId) values (7, 7);
insert into CountryByCities (CountryId, CityId) values (4, 35);
insert into CountryByCities (CountryId, CityId) values (10, 8);
insert into CountryByCities (CountryId, CityId) values (8, 3);
insert into CountryByCities (CountryId, CityId) values (2, 10);
insert into CountryByCities (CountryId, CityId) values (5, 10);
insert into CountryByCities (CountryId, CityId) values (10, 20);
insert into CountryByCities (CountryId, CityId) values (6, 30);
insert into CountryByCities (CountryId, CityId) values (9, 37);
insert into CountryByCities (CountryId, CityId) values (4, 14);
insert into CountryByCities (CountryId, CityId) values (7, 30);




