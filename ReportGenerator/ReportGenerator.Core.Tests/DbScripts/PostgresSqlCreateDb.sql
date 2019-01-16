CREATE TABLE Region
(
   Id INTEGER PRIMARY KEY,
   Name VARCHAR(200) NOT NULL
);

CREATE TABLE City
(
   Id INTEGER PRIMARY KEY,
   Name VARCHAR(200) NOT NULL,
   RegionId INTEGER,
   FOREIGN KEY (RegionId) REFERENCES Region(Id)
);

CREATE TABLE Citizen
(
   Id INTEGER PRIMARY KEY,
   FirstName VARCHAR(100) NOT NULL,
   LastName VARCHAR(100) NOT NULL,
   Age INTEGER NOT NULL,
   Sex INTEGER NOT NULL,
   CityId INTEGER NOT NULL,
   FOREIGN KEY (CityId) REFERENCES City(Id)
);

CREATE VIEW CitizensWithRegion AS
SELECT Cz.FirstName, Cz.LastName, Cz.Age, Cz.Sex, Ci.Name AS City, R.Name AS Region 
FROM Citizen AS Cz INNER JOIN City AS Ci ON Cz.CityId = Ci.Id
INNER JOIN Region AS R ON Ci.RegionId = R.Id;

CREATE FUNCTION GetCitizensWithCitiesByCityAndAge(CityName VARCHAR(200), PersonAge INTEGER)
RETURNS TABLE (
    FirstName VARCHAR(100),
	LastName VARCHAR(100),
	Age INTEGER,
	City VARCHAR(200)
)
AS $$
  BEGIN
    RETURN QUERY SELECT Cz.FirstName, Cz.LastName, Cz.Age, Ci.Name AS City FROM Citizen AS Cz
	INNER JOIN City AS Ci ON  Cz.CityId = Ci.Id
	WHERE Ci.Name = CityName AND Cz.Age > PersonAge;
  END;
$$

LANGUAGE 'plpgsql';
