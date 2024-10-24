
/*
Author : Md. Sakibur Rahman

SELECT dbo.fnCurrentStock(1) 

*/

CREATE OR ALTER  FUNCTION dbo.fnCurrentStock (@ProductId INT)
RETURNS INT
AS
BEGIN

DECLARE @CurrentStock INT, @TotalStock INT, @TotalSale INT, @TotalDamage INT;

--Purchase
SELECT @TotalStock = ISNULL(SUM(Quantity),0) FROM dbo.PurchaseDetail
WHERE ProductId = @ProductId;

-- Sales
SELECT @TotalSale = ISNULL(SUM(Quantity),0) FROM dbo.SaleDetail
WHERE ProductId = @ProductId;

--Damages
SELECT @TotalDamage = ISNULL(SUM(Quantity),0) FROM dbo.DamageDetail
WHERE ProductId = @ProductId;

SET @CurrentStock = @TotalStock - (@TotalSale+@TotalDamage);

RETURN @CurrentStock;

END;

