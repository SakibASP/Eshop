

/*
Author : Md. Sakibur Rahman

EXEC GetProducts 

*/

GO

CREATE OR ALTER PROC dbo.GetProducts
(
@ProductId AS INT = NULL,
@CatId AS INT = NULL,
@Price AS INT = NULL,
@SearchString VARCHAR(50) = NULL
)
AS
BEGIN
SET NOCOUNT ON;
BEGIN TRY

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	SELECT 
		A.*,CASE WHEN A.CurrentStock > 0 THEN TRY_CAST(1 AS BIT) ELSE TRY_CAST(0 AS BIT) END AS IsAvailable
	FROM (
	SELECT TOP 1000 
		P.AutoId ProductID,PIM.AutoId AS ProductImageID,P.Name ,P.Description,P.Price
		,P.CreatedDate,dbo.fnCurrentStockByProduct(P.AutoId) CurrentStock,P.CategoryId,C.CategoryName AS Category
		,ISNULL(PIM.IsCover, 0) IsCover,PIM.ImageName,PIM.ImagePath,P.CreatedBy
	FROM 
		Products P
	LEFT JOIN 
		ProductImages PIM ON P.AutoId = PIM.ProductID 
	LEFT JOIN 
		Categories C ON P.CategoryId = C.AutoId
	WHERE 
		(P.CategoryId = @CatId OR @CatId IS NULL) AND (P.Price <= @Price OR @Price IS NULL) AND (P.AutoId = @ProductId OR @ProductId IS NULL)
		AND (UPPER(c.CategoryName) LIKE '%'+ UPPER(@searchString) +'%' OR UPPER(P.Name) LIKE '%'+ UPPER(@searchString) +'%'  OR @searchString IS NULL )

	)A

	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
END TRY
    BEGIN CATCH

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_State();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState) WITH SETERROR;

        RETURN -1;

    END CATCH;
END;

