-- Create database
CREATE DATABASE OrderManagementDB;
GO

USE OrderManagementDB;
GO

-- Create Orders table
CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerName NVARCHAR(100) NOT NULL,
    OrderDate DATE NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    IsDeleted BIT DEFAULT 0,
    CONSTRAINT CHK_CustomerName CHECK (LEN(CustomerName) >= 3),
    CONSTRAINT CHK_TotalAmount CHECK (TotalAmount > 0),
    CONSTRAINT CHK_OrderDate CHECK (OrderDate <= GETDATE())
);

-- Create OrderStatistics table
CREATE TABLE OrderStatistics (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TotalOrders INT DEFAULT 0,
    DailyOrders INT DEFAULT 0,
    TotalAmount DECIMAL(18,2) DEFAULT 0,
    DailyAmount DECIMAL(18,2) DEFAULT 0,
    LastUpdated DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT CHK_TotalOrders CHECK (TotalOrders >= 0),
    CONSTRAINT CHK_TotalAmount_Stats CHECK (TotalAmount >= 0)
);

-- Create OrderAudit table
CREATE TABLE OrderAudit (
    AuditId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT,
    Action NVARCHAR(10),
    ChangeDate DATETIME2 DEFAULT GETDATE(),
    OldValue NVARCHAR(MAX),
    NewValue NVARCHAR(MAX),
    UserName NVARCHAR(100)
);

-- Create indexes for better performance
CREATE NONCLUSTERED INDEX IX_Orders_CustomerName ON Orders(CustomerName);
CREATE NONCLUSTERED INDEX IX_Orders_OrderDate ON Orders(OrderDate);
CREATE NONCLUSTERED INDEX IX_OrderAudit_OrderId ON OrderAudit(OrderId); 