USE PODb;
GO

--EXEC dbo.sp_DeletePurchaseRecord 'P005'

ALTER PROCEDURE dbo.sp_DeletePurchaseRecord (
@poNO CHAR(4) )
AS
BEGIN TRAN
    -- SUPPLIER(x) - ITEM - QTY - DATE(x)
    BEGIN TRY
        
		--Remove the PO record from PODETAIL table
        DELETE FROM PODETAIL 
        WHERE [PONO] = @poNO;

        --Remove the PO record from POMASTER table
        DELETE FROM POMASTER 
		WHERE [PONO] = @poNO;

	END TRY
	BEGIN CATCH
		PRINT 'ERROR OCCURED'   
        ROLLBACK TRAN
    END CATCH

COMMIT TRAN
GO