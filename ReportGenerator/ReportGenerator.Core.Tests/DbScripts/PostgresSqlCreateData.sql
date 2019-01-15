-- CREATE Regions
INSERT INTO Region (Id, Name) VALUES(1, 'Свердловская область');
INSERT INTO Region (Id, Name) VALUES(2, 'Челябинская область');
INSERT INTO Region (Id, Name) VALUES(3, 'Тюменская область');
INSERT INTO Region (Id, Name) VALUES(4, 'Пермская область');
INSERT INTO Region (Id, Name) VALUES(5, 'Респ. Татарстан');

-- CREATE Cities
-- Region - Свердловская область
INSERT INTO City (Id, Name, RegionId) VALUES(1, 'г. Нижний Тагил', 1);
INSERT INTO City (Id, Name, RegionId) VALUES(2, 'г. Екатеринбург', 1);
INSERT INTO City (Id, Name, RegionId) VALUES(3, 'г. Первоуральск', 1);
-- Region - Челябинская область
INSERT INTO City (Id, Name, RegionId) VALUES(4, 'г. Челябинск', 2);
INSERT INTO City (Id, Name, RegionId) VALUES(5, 'г. Магнитогорск', 2);
-- Region - Тюменская область
INSERT INTO City (Id, Name, RegionId) VALUES(6, 'г. Тюмень', 3);
INSERT INTO City (Id, Name, RegionId) VALUES(7, 'г. Сургут', 3);
INSERT INTO City (Id, Name, RegionId) VALUES(8, 'г. Тобольск', 3);
-- Region - Пермская область
INSERT INTO City (Id, Name, RegionId) VALUES(9, 'г. Пермь', 4);
-- Region - Респ. Татарстан
INSERT INTO City (Id, Name, RegionId) VALUES(10, 'г. Казань', 5);

-- Populate Cities with Citizens

-- Yekaterinburg citizens
INSERT INTO Citizen (Id, FirstName, LastName, Age, Sex, CityId)
VALUES (1, 'Иван', 'Иванов', 21, 1, 2);
INSERT INTO Citizen (Id, FirstName, LastName, Age, Sex, CityId)
VALUES (2, 'Алексей', 'Козлов', 34, 1, 2);
INSERT INTO Citizen (Id, FirstName, LastName, Age, Sex, CityId)
VALUES (3, 'Петр', 'Петров', 32, 1, 2);
INSERT INTO Citizen (Id, FirstName, LastName, Age, Sex, CityId)
VALUES (4, 'Елена', 'Головач', 19, 0, 2);
INSERT INTO Citizen (Id, FirstName, LastName, Age, Sex, CityId)
VALUES (5, 'Ольга', 'Вялова', 44, 0, 2);

-- Nijniy Tagil citizens
INSERT INTO Citizen (Id, FirstName, LastName, Age, Sex, CityId)
VALUES (6, 'Роман', 'Барашков', 49, 1, 1);
INSERT INTO Citizen (Id, FirstName, LastName, Age, Sex, CityId)
VALUES (7, 'Татьяна', 'Трололошина', 30, 0, 1);
INSERT INTO Citizen (Id, FirstName, LastName, Age, Sex, CityId)
VALUES (8, 'Екатерина', 'Кракозябрина', 22, 0, 1);

-- Pervoyralsk citizen
INSERT INTO Citizen (Id, FirstName, LastName, Age, Sex, CityId)
VALUES (9, 'Юра', 'Наркоманов', 33, 1, 3);
INSERT INTO Citizen (Id, FirstName, LastName, Age, Sex, CityId)
VALUES (10, 'Татьяна', 'Трололошина', 30, 0, 3);
INSERT INTO Citizen (Id, FirstName, LastName, Age, Sex, CityId)
VALUES (11, 'Екатерина', 'Кракозябрина', 22, 0, 3);

-- Chelyabinsk citizen
INSERT INTO Citizen (Id, FirstName, LastName, Age, Sex, CityId)
VALUES (12, 'Денис', 'Казаков', 14, 1, 4);
INSERT INTO Citizen (Id, FirstName, LastName, Age, Sex, CityId)
VALUES (13, 'Вячеслав', 'Семенов', 27, 1, 4);
INSERT INTO Citizen (Id, FirstName, LastName, Age, Sex, CityId)
VALUES (14, 'Марина', 'Олошина', 30, 0, 4);
INSERT INTO Citizen (Id, FirstName, LastName, Age, Sex, CityId)
VALUES (15, 'Маша', 'Козлова', 25, 0, 4);