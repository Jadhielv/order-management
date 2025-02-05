USE OrderManagementDB;
GO

-- Initialize OrderStatistics with default values
INSERT INTO OrderStatistics (TotalOrders, DailyOrders, TotalAmount, DailyAmount)
VALUES (0, 0, 0.00, 0.00);

-- Insert some sample orders
INSERT INTO Orders (CustomerName, OrderDate, TotalAmount)
VALUES 
    ('John Doe', DATEADD(day, -5, GETDATE()), 150.00),
    ('Jane Smith', DATEADD(day, -3, GETDATE()), 299.99),
    ('Bob Johnson', DATEADD(day, -1, GETDATE()), 75.50);
GO 