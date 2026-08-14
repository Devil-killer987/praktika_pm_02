-- ============================================
-- БД: SchoolEquipmentAccounting
-- Описание: Учёт компьютерного оборудования в школе
-- ============================================

CREATE DATABASE SchoolEquipmentAccounting;
GO

USE SchoolEquipmentAccounting;
GO

-- 1. Здания
CREATE TABLE Buildings (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Address NVARCHAR(200) NULL,
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- 2. Кабинеты
CREATE TABLE Classrooms (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    BuildingId INT NOT NULL,
    Number NVARCHAR(20) NOT NULL,
    Floor NVARCHAR(10) NULL,
    Description NVARCHAR(500) NULL,
    CONSTRAINT FK_Classrooms_Buildings FOREIGN KEY (BuildingId) REFERENCES Buildings(Id) ON DELETE CASCADE
);

-- 3. Типы оборудования
CREATE TABLE EquipmentTypes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Category NVARCHAR(50) NOT NULL -- PC, Printer, Projector, Network, Other
);

-- 4. Оборудование
CREATE TABLE Equipment (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClassroomId INT NOT NULL,
    EquipmentTypeId INT NOT NULL,
    InventoryNumber NVARCHAR(50) NOT NULL UNIQUE,
    Manufacturer NVARCHAR(100) NULL,
    Model NVARCHAR(100) NULL,
    SerialNumber NVARCHAR(100) NULL,
    PurchaseDate DATE NULL,
    WarrantyEnd DATE NULL,
    Status NVARCHAR(20) DEFAULT 'Working', -- Working, Repair, Decommissioned
    Notes NVARCHAR(500) NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Equipment_Classrooms FOREIGN KEY (ClassroomId) REFERENCES Classrooms(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Equipment_EquipmentTypes FOREIGN KEY (EquipmentTypeId) REFERENCES EquipmentTypes(Id)
);

-- 5. Категории характеристик
CREATE TABLE SpecificationCategories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL, -- CPU, RAM_Size, HDD, SSD, PrintType, etc.
    DisplayName NVARCHAR(100) NOT NULL, -- Процессор, Оперативная память, etc.
    Unit NVARCHAR(20) NULL, -- ГБ, МГц, шт, etc.
    EquipmentTypeId INT NOT NULL,
    CONSTRAINT FK_SpecCat_EquipmentTypes FOREIGN KEY (EquipmentTypeId) REFERENCES EquipmentTypes(Id) ON DELETE CASCADE
);

-- 6. Характеристики оборудования
CREATE TABLE EquipmentSpecifications (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EquipmentId INT NOT NULL,
    CategoryId INT NULL,
    Value NVARCHAR(500) NOT NULL,
    CustomName NVARCHAR(100) NULL, -- для произвольных характеристик
    CONSTRAINT FK_EqSpec_Equipment FOREIGN KEY (EquipmentId) REFERENCES Equipment(Id) ON DELETE CASCADE,
    CONSTRAINT FK_EqSpec_Categories FOREIGN KEY (CategoryId) REFERENCES SpecificationCategories(Id)
);

-- 7. Пользователи (для ролевого доступа)
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Login NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) NOT NULL DEFAULT 'Viewer', -- Admin, Operator, Viewer
    FullName NVARCHAR(100) NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    LastLogin DATETIME NULL
);

-- 8. Журнал действий (аудит)
CREATE TABLE AuditLog (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NULL,
    Action NVARCHAR(50) NOT NULL,
    EntityType NVARCHAR(50) NOT NULL,
    EntityId INT NULL,
    Details NVARCHAR(MAX) NULL,
    Timestamp DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Audit_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Индексы для производительности
CREATE INDEX IX_Equipment_ClassroomId ON Equipment(ClassroomId);
CREATE INDEX IX_Equipment_Status ON Equipment(Status);
CREATE INDEX IX_EquipmentSpecifications_EquipmentId ON EquipmentSpecifications(EquipmentId);
CREATE INDEX IX_Users_Login ON Users(Login);