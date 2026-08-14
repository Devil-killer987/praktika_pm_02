-- ============================================
-- Наполнение демонстрационными данными
-- ============================================

USE SchoolEquipmentAccounting;
GO

-- === Здания (2 штук) ===
INSERT INTO Buildings (Name, Address) VALUES
('Большая школа', 'ул. Школьная, д. 1'),
('Начальная школа', 'ул. Мира, д. 5');

-- === Кабинеты (30-50 штук) ===
INSERT INTO Classrooms (BuildingId, Number, Floor, Description) VALUES
(1, '101', '1', 'Кабинет информатики'),
(1, '102', '1', 'Кабинет физики'),
(1, '201', '2', 'Кабинет математики'),
(1, '202', '2', 'Кабинет русского языка'),
(1, '203', '2', 'Кабинет английского языка'),
(1, '301', '3', 'Кабинет химии'),
(1, '302', '3', 'Кабинет биологии'),
(1, '303', '3', 'Кабинет географии'),
(1, '401', '4', 'Кабинет истории'),
(1, '402', '4', 'Кабинет обществознания'),
(1, '403', '4', 'Кабинет литературы'),
(2, '101', '1', 'Спортивный зал'),
(2, '102', '1', 'Тренажёрный зал'),
(2, '103', '1', 'Раздевалка №1'),
(2, '104', '1', 'Раздевалка №2'),
(3, '101', '1', '1-й класс "А"'),
(3, '102', '1', '1-й класс "Б"'),
(3, '103', '1', '2-й класс "А"'),
(3, '104', '1', '2-й класс "Б"'),
(3, '201', '2', '3-й класс "А"'),
(3, '202', '2', '3-й класс "Б"'),
(3, '203', '2', '4-й класс "А"'),
(3, '204', '2', '4-й класс "Б"'),
(1, '104', '1', 'Лаборатория'),
(1, '105', '1', 'Мастерская'),
(1, '204', '2', 'Библиотека'),
(1, '205', '2', 'Медиатека'),
(1, '304', '3', 'Кабинет информатики №2'),
(1, '305', '3', 'Кабинет робототехники'),
(1, '404', '4', 'Актовый зал');

-- === Типы оборудования (20+ штук) ===
INSERT INTO EquipmentTypes (Name, Category) VALUES
('Системный блок', 'PC'),
('Монитор', 'PC'),
('Ноутбук', 'PC'),
('Планшет', 'PC'),
('Принтер лазерный', 'Printer'),
('Принтер струйный', 'Printer'),
('МФУ', 'Printer'),
('Проектор', 'Projector'),
('Интерактивная доска', 'Projector'),
('Коммутатор', 'Network'),
('Маршрутизатор', 'Network'),
('Точка доступа Wi-Fi', 'Network'),
('Сервер', 'PC'),
('Рабочая станция', 'PC'),
('Моноблок', 'PC'),
('3D-принтер', 'Other'),
('Сканер', 'Other'),
('Копировальный аппарат', 'Other'),
('Телевизор', 'Other'),
('Музыкальный центр', 'Other'),
('Микроскоп', 'Other');

-- === Категории характеристик ===
INSERT INTO SpecificationCategories (Name, DisplayName, Unit, EquipmentTypeId) VALUES
('CPU', 'Процессор', NULL, 1),
('RAM_Size', 'Оперативная память', 'ГБ', 1),
('RAM_Type', 'Тип памяти', NULL, 1),
('HDD', 'Жёсткий диск', 'ГБ', 1),
('SSD', 'SSD-накопитель', 'ГБ', 1),
('GPU', 'Видеокарта', NULL, 1),
('OS', 'Операционная система', NULL, 1),
('ScreenSize', 'Диагональ экрана', '"', 2),
('ScreenResolution', 'Разрешение экрана', NULL, 2),
('CPU', 'Процессор', NULL, 3),
('RAM_Size', 'Оперативная память', 'ГБ', 3),
('SSD', 'SSD-накопитель', 'ГБ', 3),
('ScreenSize', 'Диагональ экрана', '"', 3),
('CPU', 'Процессор', NULL, 4),
('RAM_Size', 'Оперативная память', 'ГБ', 4),
('ScreenSize', 'Диагональ экрана', '"', 4),
('PrintType', 'Тип печати', NULL, 5),
('PrintSpeed', 'Скорость печати', 'стр/мин', 5),
('PrintFormat', 'Формат печати', NULL, 5),
('ColorPrint', 'Цветная печать', NULL, 5),
('PrintType', 'Тип печати', NULL, 6),
('PrintSpeed', 'Скорость печати', 'стр/мин', 6),
('ColorPrint', 'Цветная печать', NULL, 6),
('PrintType', 'Тип печати', NULL, 7),
('PrintSpeed', 'Скорость печати', 'стр/мин', 7),
('ColorPrint', 'Цветная печать', NULL, 7),
('Scanner', 'Сканер', NULL, 7),
('ProjectionType', 'Тип проекции', NULL, 8),
('Lumens', 'Яркость', 'лм', 8),
('Resolution', 'Разрешение', NULL, 8),
('ScreenSize', 'Диагональ', '"', 9),
('Ports', 'Количество портов', 'шт', 10),
('Speed', 'Скорость', 'Мбит/с', 10),
('Standard', 'Стандарт', NULL, 11),
('Standard', 'Стандарт', NULL, 12),
('CPU', 'Процессор', NULL, 13),
('RAM_Size', 'Оперативная память', 'ГБ', 13),
('HDD', 'Жёсткий диск', 'ГБ', 13),
('CPU', 'Процессор', NULL, 14),
('RAM_Size', 'Оперативная память', 'ГБ', 14),
('HDD', 'Жёсткий диск', 'ГБ', 14),
('CPU', 'Процессор', NULL, 15),
('RAM_Size', 'Оперативная память', 'ГБ', 15),
('SSD', 'SSD-накопитель', 'ГБ', 15),
('ScreenSize', 'Диагональ экрана', '"', 15),
('PrintType', 'Тип печати', NULL, 16),
('PrintFormat', 'Формат печати', NULL, 16);

-- === ВСТАВКА 1000+ записей оборудования ===
-- Генерируем 1000+ записей через цикл
DECLARE @i INT = 1;
DECLARE @classroomId INT;
DECLARE @typeId INT;
DECLARE @status NVARCHAR(20);
DECLARE @purchaseDate DATE;
DECLARE @warrantyEnd DATE;
DECLARE @manufacturers NVARCHAR(100) = 'HP,Dell,Lenovo,Apple,ASUS,Acer,Samsung,Xerox,Canon,Epson,Brother,Panasonic,Sony,LG,Intel,AMD,Kingston,Seagate,Western Digital,Microsoft,Google';
DECLARE @models NVARCHAR(100) = 'ProBook,Latitude,ThinkPad,MacBook,VivoBook,Aspire,Galaxy,WorkCentre,MF735Cdw,ET-2750,HL-L2350DW,SmartBoard,PT-VZ580,EX,OptiPlex,IdeaCentre,Chromebook,Surface Pro,iMac,Studio';

WHILE @i <= 1000
BEGIN
    -- Случайный кабинет (1-30)
    SET @classroomId = 1 + ABS(CHECKSUM(NEWID()) % 30);
    
    -- Случайный тип оборудования (1-21)
    SET @typeId = 1 + ABS(CHECKSUM(NEWID()) % 21);
    
    -- Случайный статус (80% Working, 15% Repair, 5% Decommissioned)
    SET @status = CASE 
        WHEN ABS(CHECKSUM(NEWID())) % 100 < 80 THEN 'Working'
        WHEN ABS(CHECKSUM(NEWID())) % 100 < 95 THEN 'Repair'
        ELSE 'Decommissioned'
    END;
    
    -- Случайная дата покупки (от 2018 до 2026)
    SET @purchaseDate = DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 2920, '2026-01-01');
    
    -- Гарантия: 1-3 года от даты покупки
    SET @warrantyEnd = DATEADD(MONTH, 12 + ABS(CHECKSUM(NEWID())) % 24, @purchaseDate);

    INSERT INTO Equipment (
        ClassroomId,
        EquipmentTypeId,
        InventoryNumber,
        Manufacturer,
        Model,
        SerialNumber,
        PurchaseDate,
        WarrantyEnd,
        Status,
        Notes
    ) VALUES (
        @classroomId,
        @typeId,
        'INV-' + RIGHT('000000' + CAST(@i AS NVARCHAR(6)), 6),
        -- Случайный производитель
        (SELECT TOP 1 value FROM STRING_SPLIT(@manufacturers, ',') ORDER BY NEWID()),
        -- Случайная модель
        (SELECT TOP 1 value FROM STRING_SPLIT(@models, ',') ORDER BY NEWID()) + '-' + CAST(ABS(CHECKSUM(NEWID())) % 1000 AS NVARCHAR(4)),
        'SN' + CAST(ABS(CHECKSUM(NEWID())) AS NVARCHAR(15)),
        @purchaseDate,
        @warrantyEnd,
        @status,
        CASE WHEN ABS(CHECKSUM(NEWID())) % 5 = 0 THEN 'Требуется замена расходных материалов' ELSE NULL END
    );

    SET @i = @i + 1;
END

-- === Характеристики для оборудования (выборочно) ===
-- Добавим характеристики для первых 500 записей (чтобы не перегружать)
INSERT INTO EquipmentSpecifications (EquipmentId, CategoryId, Value)
SELECT 
    e.Id,
    sc.Id,
    CASE 
        WHEN sc.Name = 'CPU' THEN 
            (SELECT TOP 1 value FROM STRING_SPLIT('Intel Core i3-10100,Intel Core i5-10400,Intel Core i7-10700,AMD Ryzen 3,AMD Ryzen 5,Intel Celeron,Intel Pentium', ',') ORDER BY NEWID())
        WHEN sc.Name = 'RAM_Size' THEN 
            CAST((4 + ABS(CHECKSUM(NEWID())) % 28) AS NVARCHAR(10)) -- 4-32 ГБ
        WHEN sc.Name = 'RAM_Type' THEN 
            (SELECT TOP 1 value FROM STRING_SPLIT('DDR3,DDR4,DDR5', ',') ORDER BY NEWID())
        WHEN sc.Name = 'HDD' THEN 
            CAST((500 + ABS(CHECKSUM(NEWID())) % 3500) AS NVARCHAR(10))
        WHEN sc.Name = 'SSD' THEN 
            CAST((128 + ABS(CHECKSUM(NEWID())) % 1900) AS NVARCHAR(10))
        WHEN sc.Name = 'GPU' THEN 
            (SELECT TOP 1 value FROM STRING_SPLIT('Intel UHD,Intel Iris,NVIDIA GTX 1650,NVIDIA RTX 3060,AMD Radeon', ',') ORDER BY NEWID())
        WHEN sc.Name = 'OS' THEN 
            (SELECT TOP 1 value FROM STRING_SPLIT('Windows 10 Pro,Windows 11 Pro,Linux Ubuntu,Windows 10 Home,macOS', ',') ORDER BY NEWID())
        WHEN sc.Name = 'ScreenSize' THEN 
            CAST((15 + ABS(CHECKSUM(NEWID())) % 16) AS NVARCHAR(10))
        WHEN sc.Name = 'ScreenResolution' THEN 
            (SELECT TOP 1 value FROM STRING_SPLIT('1920x1080,2560x1440,3840x2160,1366x768', ',') ORDER BY NEWID())
        WHEN sc.Name = 'PrintType' THEN 
            (SELECT TOP 1 value FROM STRING_SPLIT('Лазерный,Струйный,Матричный,Сублимационный', ',') ORDER BY NEWID())
        WHEN sc.Name = 'PrintSpeed' THEN 
            CAST((10 + ABS(CHECKSUM(NEWID())) % 40) AS NVARCHAR(10))
        WHEN sc.Name = 'PrintFormat' THEN 
            (SELECT TOP 1 value FROM STRING_SPLIT('A4,A3,10x15,13x18', ',') ORDER BY NEWID())
        WHEN sc.Name = 'ColorPrint' THEN 
            CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN 'Да' ELSE 'Нет' END
        WHEN sc.Name = 'Scanner' THEN 
            CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN 'Да' ELSE 'Нет' END
        WHEN sc.Name = 'Lumens' THEN 
            CAST((2000 + ABS(CHECKSUM(NEWID())) % 3000) AS NVARCHAR(10))
        WHEN sc.Name = 'Resolution' THEN 
            (SELECT TOP 1 value FROM STRING_SPLIT('1920x1080,1024x768,1280x800', ',') ORDER BY NEWID())
        WHEN sc.Name = 'Ports' THEN 
            CAST((8 + ABS(CHECKSUM(NEWID())) % 40) AS NVARCHAR(10))
        WHEN sc.Name = 'Speed' THEN 
            CAST((100 + ABS(CHECKSUM(NEWID())) % 900) AS NVARCHAR(10))
        WHEN sc.Name = 'Standard' THEN 
            (SELECT TOP 1 value FROM STRING_SPLIT('IEEE 802.11n,IEEE 802.11ac,IEEE 802.11ax,Gigabit Ethernet', ',') ORDER BY NEWID())
        WHEN sc.Name = 'ProjectionType' THEN 
            (SELECT TOP 1 value FROM STRING_SPLIT('DLP,3LCD,LCoS', ',') ORDER BY NEWID())
        ELSE 'Unknown'
    END
FROM Equipment e
CROSS JOIN SpecificationCategories sc
WHERE e.Id % 2 = 0 -- Только для чётных ID оборудования (500 записей)
  AND sc.EquipmentTypeId = e.EquipmentTypeId
  AND ABS(CHECKSUM(NEWID())) % 3 < 2; -- 66% вероятность добавления характеристики

-- === Добавление произвольных характеристик ===
INSERT INTO EquipmentSpecifications (EquipmentId, CustomName, Value)
SELECT 
    e.Id,
    (SELECT TOP 1 value FROM STRING_SPLIT('Доп. ПО,Кабель питания,Клавиатура,Мышь,USB-удлинитель,Адаптер питания', ',') ORDER BY NEWID()),
    (SELECT TOP 1 value FROM STRING_SPLIT('Установлено,В комплекте,Требуется замена,Не требуется', ',') ORDER BY NEWID())
FROM Equipment e
WHERE e.Id % 3 = 0
  AND ABS(CHECKSUM(NEWID())) % 2 = 0;

-- === Пользователи ===
INSERT INTO Users (Login, PasswordHash, Role, FullName) VALUES
('admin', 'AQAAAAEAACcQAAAAEJBXBqF+CU4KlzGxAbvNLdcLgq0fMlUO10f5lN0iM2n7Qw5yLx8Q4rV3w9A==', 'Admin', 'Администратор системы'),
('operator', 'AQAAAAEAACcQAAAAEJBXBqF+CU4KlzGxAbvNLdcLgq0fMlUO10f5lN0iM2n7Qw5yLx8Q4rV3w9A==', 'Operator', 'Оператор учёта'),
('viewer', 'AQAAAAEAACcQAAAAEJBXBqF+CU4KlzGxAbvNLdcLgq0fMlUO10f5lN0iM2n7Qw5yLx8Q4rV3w9A==', 'Viewer', 'Просмотрщик');

-- Проверка количества
SELECT COUNT(*) AS TotalEquipment FROM Equipment; -- Должно быть >= 1000
SELECT COUNT(*) AS TotalRecords FROM EquipmentSpecifications;
SELECT COUNT(*) AS TotalClassrooms FROM Classrooms;
SELECT COUNT(*) AS TotalBuildings FROM Buildings;
SELECT COUNT(*) AS TotalEquipmentTypes FROM EquipmentTypes;