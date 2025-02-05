USE OrderManagementDB;
GO

-- Trigger to prevent future dates
CREATE TRIGGER TR_Orders_PreventFutureDates
ON Orders
AFTER INSERT, UPDATE AS
BEGIN
    IF EXISTS (SELECT 1 FROM inserted WHERE OrderDate > GETDATE())
    BEGIN
        RAISERROR ('Order date cannot be in the future.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;
GO

-- Trigger to update statistics when new orders are created
CREATE TRIGGER TR_Orders_UpdateStatistics
ON Orders
AFTER INSERT AS
BEGIN
    UPDATE OrderStatistics
    SET TotalOrders = TotalOrders + 1,
        DailyOrders = (
            SELECT COUNT(*) 
            FROM Orders 
            WHERE CAST(CreatedAt AS DATE) = CAST(GETDATE() AS DATE)
        ),
        TotalAmount = TotalAmount + (SELECT SUM(TotalAmount) FROM inserted),
        DailyAmount = (
            SELECT SUM(TotalAmount) 
            FROM Orders 
            WHERE CAST(CreatedAt AS DATE) = CAST(GETDATE() AS DATE)
        ),
        LastUpdated = GETDATE();
END;
GO

-- Trigger for audit trail
CREATE TRIGGER TR_Orders_Audit
ON Orders
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    INSERT INTO OrderAudit (OrderId, Action, OldValue, NewValue, UserName)
    SELECT
        COALESCE(i.Id, d.Id),
        CASE 
            WHEN i.Id IS NOT NULL AND d.Id IS NOT NULL THEN 'UPDATE'
            WHEN i.Id IS NOT NULL THEN 'INSERT'
            ELSE 'DELETE'
        END,
        CASE WHEN d.Id IS NOT NULL THEN (SELECT d.* FOR JSON PATH) ELSE NULL END,
        CASE WHEN i.Id IS NOT NULL THEN (SELECT i.* FOR JSON PATH) ELSE NULL END,
        SYSTEM_USER
    FROM deleted d
    FULL OUTER JOIN inserted i ON d.Id = i.Id;
END;
GO 