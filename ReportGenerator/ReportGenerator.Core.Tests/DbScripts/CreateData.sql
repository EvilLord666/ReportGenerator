-- CREATE Regions
INSERT INTO [dbo].[Region] ([Id], [Name]) VALUES(N'608E0669-A440-4862-B6C7-63949B5D9FC0', N'Свердловская область');
INSERT INTO [dbo].[Region] ([Id], [Name]) VALUES(N'42B8818F-DF52-4363-88D9-D34125324F97', N'Челябинская область');
INSERT INTO [dbo].[Region] ([Id], [Name]) VALUES(N'665B7091-7099-440C-93F0-E159DE24EBE8', N'Тюменская область');
INSERT INTO [dbo].[Region] ([Id], [Name]) VALUES(N'0289F242-3544-4F5E-821B-7EEC36833953', N'Пермская область');
INSERT INTO [dbo].[Region] ([Id], [Name]) VALUES(N'A17B50B9-FCFE-4ED5-AD22-E34BD28A3C92', N'Респ. Татарстан');

-- CREATE Cities
-- Region - Свердловская область
INSERT INTO [dbo].[City] ([Id], [Name], [RegionId]) VALUES(N'0A15EC6D-BA21-4CDC-B166-5301B49D0273', N'г. Нижний Тагил', N'608E0669-A440-4862-B6C7-63949B5D9FC0');
INSERT INTO [dbo].[City] ([Id], [Name], [RegionId]) VALUES(N'B9B08522-D89B-486A-BFE5-7694C6AE2D06', N'г. Екатеринбург', N'608E0669-A440-4862-B6C7-63949B5D9FC0');
INSERT INTO [dbo].[City] ([Id], [Name], [RegionId]) VALUES(N'BA2B81C2-AE3B-4AD3-8502-73050CAB6D03', N'г. Первоуральск', N'608E0669-A440-4862-B6C7-63949B5D9FC0');
-- Region - Челябинская область
INSERT INTO [dbo].[City] ([Id], [Name], [RegionId]) VALUES(N'F4CB1315-E721-46F4-AF86-42C1E85917D3', N'г. Челябинск', N'42B8818F-DF52-4363-88D9-D34125324F97');
INSERT INTO [dbo].[City] ([Id], [Name], [RegionId]) VALUES(N'6A74B9EA-1F59-4D64-BA19-8ADD819E08DB', N'г. Магнитогорск', N'42B8818F-DF52-4363-88D9-D34125324F97');
-- Region - Тюменская область
INSERT INTO [dbo].[City] ([Id], [Name], [RegionId]) VALUES(N'53F821FB-FFD8-4FCD-9A80-02BA352BEC29', N'г. Тюмень', N'665B7091-7099-440C-93F0-E159DE24EBE8');
INSERT INTO [dbo].[City] ([Id], [Name], [RegionId]) VALUES(N'72BE1696-6EA1-4B25-8A9E-94E002E9D7AF', N'г. Сургут', N'665B7091-7099-440C-93F0-E159DE24EBE8');
INSERT INTO [dbo].[City] ([Id], [Name], [RegionId]) VALUES(N'06DAD783-BA35-420D-8BAB-86087D4E310C', N'г. Тобольск', N'665B7091-7099-440C-93F0-E159DE24EBE8');
-- Region - Пермская область
INSERT INTO [dbo].[City] ([Id], [Name], [RegionId]) VALUES(N'23CCE5E9-38FF-4B67-8E9A-7E7A73508F82', N'г. Пермь', N'0289F242-3544-4F5E-821B-7EEC36833953');
-- Region - Респ. Татарстан
INSERT INTO [dbo].[City] ([Id], [Name], [RegionId]) VALUES(N'7DB2764C-5696-4D80-8E97-928EB30134C5', N'г. Казань', N'A17B50B9-FCFE-4ED5-AD22-E34BD28A3C92');

-- Populate Cities with Citizens

-- Yekaterinburg citizens
INSERT INTO [dbo].[Citizen] ([Id], [FirstName], [LastName], [Age], [Sex], [CityId])
VALUES (N'25FB9FF7-7259-48CD-86E3-F6F298F2FE91', N'Иван', N'Иванов', 21, 1, N'B9B08522-D89B-486A-BFE5-7694C6AE2D06');
INSERT INTO [dbo].[Citizen] ([Id], [FirstName], [LastName], [Age], [Sex], [CityId])
VALUES (N'29FDF443-A60E-49F2-94E3-071BAF02313A', N'Алексей', N'Козлов', 34, 1, N'B9B08522-D89B-486A-BFE5-7694C6AE2D06');
INSERT INTO [dbo].[Citizen] ([Id], [FirstName], [LastName], [Age], [Sex], [CityId])
VALUES (N'2D284B32-C3C2-4DEF-B868-748859AC7F39', N'Петр', N'Петров', 32, 1, N'B9B08522-D89B-486A-BFE5-7694C6AE2D06');
INSERT INTO [dbo].[Citizen] ([Id], [FirstName], [LastName], [Age], [Sex], [CityId])
VALUES (N'FA8F82F0-7B8F-459A-A5BD-57F2BFBB3FAD', N'Елена', N'Головач', 19, 0, N'B9B08522-D89B-486A-BFE5-7694C6AE2D06');
INSERT INTO [dbo].[Citizen] ([Id], [FirstName], [LastName], [Age], [Sex], [CityId])
VALUES (N'A660D90A-BD41-49B5-B42D-9020532501D3', N'Ольга', N'Вялова', 44, 0, N'B9B08522-D89B-486A-BFE5-7694C6AE2D06');

-- Nijniy Tagil citizens
INSERT INTO [dbo].[Citizen] ([Id], [FirstName], [LastName], [Age], [Sex], [CityId])
VALUES (N'1861B600-658F-4F18-914E-939DB1317CCF', N'Роман', N'Барашков', 49, 1, N'B9B08522-D89B-486A-BFE5-7694C6AE2D06');
INSERT INTO [dbo].[Citizen] ([Id], [FirstName], [LastName], [Age], [Sex], [CityId])
VALUES (N'1DA29BFC-62D5-4247-AD35-70F7A3467969', N'Татьяна', N'Трололошина', 30, 0, N'B9B08522-D89B-486A-BFE5-7694C6AE2D06');
INSERT INTO [dbo].[Citizen] ([Id], [FirstName], [LastName], [Age], [Sex], [CityId])
VALUES (N'D43B3021-D52F-43F5-AED6-9661F6C1C35E', N'Екатерина', N'Кракозябрина', 22, 0, N'B9B08522-D89B-486A-BFE5-7694C6AE2D06');

-- Pervoyralsk citizen
INSERT INTO [dbo].[Citizen] ([Id], [FirstName], [LastName], [Age], [Sex], [CityId])
VALUES (N'BA87DCB9-8A5D-40B5-93A5-13C4E65F008F', N'Юра', N'Наркоманов', 33, 1, N'BA2B81C2-AE3B-4AD3-8502-73050CAB6D03');
INSERT INTO [dbo].[Citizen] ([Id], [FirstName], [LastName], [Age], [Sex], [CityId])
VALUES (N'FC40831D-C5AF-46D0-89D3-EFBB004D4804', N'Татьяна', N'Трололошина', 30, 0, N'BA2B81C2-AE3B-4AD3-8502-73050CAB6D03');
INSERT INTO [dbo].[Citizen] ([Id], [FirstName], [LastName], [Age], [Sex], [CityId])
VALUES (N'826A6FA7-34D6-46C9-8B47-3DA50207E168', N'Екатерина', N'Кракозябрина', 22, 0, N'BA2B81C2-AE3B-4AD3-8502-73050CAB6D03');

-- Chelyabinsk citizen
INSERT INTO [dbo].[Citizen] ([Id], [FirstName], [LastName], [Age], [Sex], [CityId])
VALUES (N'CF9C5B97-7D36-432B-B427-14862FF4E9CE', N'Денис', N'Казаков', 14, 1, N'F4CB1315-E721-46F4-AF86-42C1E85917D3');
INSERT INTO [dbo].[Citizen] ([Id], [FirstName], [LastName], [Age], [Sex], [CityId])
VALUES (N'F689341E-5D1D-4AE9-997C-ED5A19B05FDA', N'Вячеслав', N'Семенов', 27, 1, N'F4CB1315-E721-46F4-AF86-42C1E85917D3');
INSERT INTO [dbo].[Citizen] ([Id], [FirstName], [LastName], [Age], [Sex], [CityId])
VALUES (N'123D576A-0016-4353-9CDC-A19DE8F9E966', N'Марина', N'Олошина', 30, 0, N'F4CB1315-E721-46F4-AF86-42C1E85917D3');
INSERT INTO [dbo].[Citizen] ([Id], [FirstName], [LastName], [Age], [Sex], [CityId])
VALUES (N'C2761BFE-6969-4C5E-A482-77EADA3FD36B', N'Маша', N'Козлова', 25, 0, N'F4CB1315-E721-46F4-AF86-42C1E85917D3');