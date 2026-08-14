-- ============================================
-- Резервное копирование БД (MS SQL Server)
-- ============================================

-- Создание полной резервной копии
BACKUP DATABASE SchoolEquipmentAccounting
TO DISK = 'C:\Backups\SchoolEquipmentAccounting_Full.bak'
WITH FORMAT, 
     NAME = 'Full Backup of SchoolEquipmentAccounting',
     COMPRESSION,
     STATS = 10;

-- Создание резервной копии с датой в имени
DECLARE @backupPath NVARCHAR(200) = 'C:\Backups\SchoolEquipmentAccounting_' + 
    FORMAT(GETDATE(), 'yyyyMMdd_HHmmss') + '.bak';

BACKUP DATABASE SchoolEquipmentAccounting
TO DISK = @backupPath
WITH COMPRESSION, STATS = 10;

-- Восстановление БД
RESTORE DATABASE SchoolEquipmentAccounting
FROM DISK = 'C:\Backups\SchoolEquipmentAccounting_20260520_120000.bak'
WITH REPLACE, STATS = 10;