USE PODb;
GO

--EXEC PODb.dbo.SP_GetAllPurchaseRecord
--EXEC PODb.dbo.sp_UpdatePurchaseRecord 'P005', 'S001', '20190623', 'I004', 2
--Before Update
--PONO	SUPLNO	SUPLNAME	ITCODE	ITDESC	QTY	PODATE
--P005	S003	SUPPLIER-C	I005	SHOES	4	2019-06-24 00:00:00.000

--PONO(x)    SUPLNAME      ITDESC  QTY        PODATE
--P001       SUPPLIER-A    PEN     2          2019-06-15 00:00:00.000

ALTER PROCEDURE dbo.sp_UpdatePurchaseRecord (
   @poNo CHAR(4)            
   , @suplNo CHAR(4)
   , @poDate DATETIME
   , @itCode CHAR(4)
   , @qty INT)
AS
BEGIN TRAN
    -- SUPPLIER(x) - ITEM - QTY - DATE(x)
    BEGIN TRY
        --UPDATE Student SET NAME = 'PRATIK' WHERE Age = 20;
        --SUPPLIER(1) - ITEM(3) - QTY(4) - DATE(2)
        --Insert record to PODETAIL table
        UPDATE  PODETAIL SET
            [ITCODE] = @itCode
            , [QTY] = @qty
        WHERE [PONO] = @poNO;

        --Insert record to POMASTER table
        UPDATE POMASTER SET 
			[PODATE] = @poDate
			, [SUPLNO] = @suplNo
		WHERE [PONO] = @poNO;

	END TRY
	BEGIN CATCH
		PRINT 'ERROR OCCURED'   
        ROLLBACK TRAN
    END CATCH

COMMIT TRAN
GO


/*
SELECT TOP 1 * FROM PODb.dbo.SUPPLIER (NoLock);
SELECT TOP 1 * FROM PODb.dbo.ITEM (NoLock);
SELECT TOP 1 * FROM PODb.dbo.POMASTER (NoLock);
SELECT TOP 1 * FROM PODb.dbo.PODETAIL (NoLock);

SELECT * FROM PODb.dbo.SUPPLIER (NoLock);
SELECT * FROM PODb.dbo.ITEM (NoLock);
SELECT * FROM PODb.dbo.POMASTER (NoLock);
SELECT * FROM PODb.dbo.PODETAIL (NoLock);

PONO	PODATE					SUPLNO
P001	2019-06-15 00:00:00.000	S001
P002	2019-06-15 00:00:00.000	S001
P003	2019-06-15 00:00:00.000	S002
P004	2019-06-22 00:00:00.000	S001
P005	2019-06-23 00:00:00.000	S001

PONO	ITCODE	QTY
P001	I001	2
P002	I002	4
P003	I001	8
P004	I004	8
P005	I004	4

P005	I004	2 -- after Update
*/
 