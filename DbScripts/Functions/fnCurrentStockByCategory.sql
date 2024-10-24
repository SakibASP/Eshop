

/*
Author : Md. Sakibur Rahman

SELECT dbo.fnCurrentStockByCategory(1) 

*/

CREATE OR ALTER  FUNCTION dbo.fnCurrentStockByCategory (@CatId INT)
RETURNS INT
AS
BEGIN

DECLARE @CurrentStock INT, @TotalStock INT, @TotalSale INT, @TotalDamage INT;

--Purchase
SELECT @TotalStock = ISNULL(SUM(Quantity),0) FROM dbo.PurchaseDetail WITH (NOLOCK)
WHERE CategoryId = @CatId;

--Sales
SELECT @TotalSale = ISNULL(SUM(Quantity),0) FROM dbo.SaleDetail WITH (NOLOCK)
WHERE CategoryId = @CatId;

--Damages
SELECT @TotalDamage = ISNULL(SUM(Quantity),0) FROM dbo.DamageDetail WITH (NOLOCK)
WHERE CategoryId = @CatId;

SET @CurrentStock = @TotalStock - (@TotalSale+@TotalDamage);

RETURN @CurrentStock;

END;

