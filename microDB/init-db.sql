IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'microDB')
BEGIN
        CREATE DATABASE microDB;
END
GO

USE microDB;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Orders' AND type = 'U')
BEGIN
CREATE TABLE Orders
(
    OrderId uniqueidentifier NOT NULL CONSTRAINT Orders_pk PRIMARY KEY,
    OrderDate datetime NOT NULL,
    OrderOrigin varchar(255)
);
END
GO
