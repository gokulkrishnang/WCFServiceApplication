USE PODb;
GO

--EXEC dbo.sp_InsertPurchaseRecord 'S003', '20190624', 'I005', 4

ALTER PROCEDURE dbo.sp_InsertPurchaseRecord (
 @suplNo CHAR(4)
 , @poDate DATETIME
 , @itCode CHAR(4)
 , @qty INT)
AS
BEGIN TRAN
    -- SUPPLIER(x) - ITEM - QTY - DATE(x)
    DECLARE @nextPONO INT;
    SET @nextPONO = (SELECT TOP 1 CAST(SUBSTRING(PONO, 2, 3) AS INT) + 1
    FROM POMASTER
    ORDER BY PONO DESC);
               
    DECLARE @poNO CHAR(4);
    SET @poNO = (SELECT dbo.fn_GenPKID ('P', @nextPONO));

        BEGIN TRY
			--Insert record to POMASTER table
			INSERT INTO POMASTER ([PONO], [PODATE], [SUPLNO]) VALUES (@poNO, @poDate, @suplNo);

			-- SUPPLIER(1) - ITEM(3) - QTY(4) - DATE(2)
			--Insert record to PODETAIL table
			INSERT INTO PODETAIL ([PONO], [ITCODE], [QTY]) VALUES (@poNO, @itCode, @qty);
        END TRY
        BEGIN CATCH
                PRINT 'ERROR OCCURED'   
				ROLLBACK TRAN
		END CATCH
COMMIT TRAN

GO

/*
SELECT TOP 1 * FROM SUPPLIER (NoLock);
SELECT TOP 1 * FROM ITEM (NoLock);
SELECT TOP 1 * FROM POMASTER (NoLock);
SELECT TOP 1 * FROM PODETAIL (NoLock);

SELECT * FROM SUPPLIER (NoLock);
SELECT * FROM ITEM (NoLock);
SELECT * FROM POMASTER (NoLock);
SELECT * FROM PODETAIL (NoLock);

P006 - 2019-06-24 00:00:00.000 - S003
P006 - I005 - 4

-- SUPPLIER - ITEM - QTY - DATE
--CREATE
--Entry to PO Master
INSERT INTO POMASTER ([PONO], [PODATE], [SUPLNO]) SELECT dbo.fnGenPKID ('P', 1), '20190615','S001';

--Entry to PO Detail
INSERT INTO PODETAIL ([PONO], [ITCODE], [QTY]) VALUES ('P001', 'I001', 2); --P001 (PM) | I001 PEN | 2 - SUP1

SELECT SUBSTRING(input_string, start, length);
SELECT SUBSTRING('P005', 2, 3);

Cast(Substring(LOTNUMBER,10,2) as int)
SELECT Cast(SUBSTRING('P005', 2, 3) AS INT);
SELECT Cast(SUBSTRING('P005', 2, 3) AS INT) + 1;

BEGIN TRAN
    BEGIN TRY
        INSERT INTO PARENT
        (PARENT_DT)

        SELECT    GETDATE()-1
        SELECT    @PARENT_ID = @@IDENTITY

        INSERT INTO CHILD (PARENT_ID,CHILD_DT)
        SELECT    @PARENT_ID, GETDATE() + 1

    END TRY
    BEGIN CATCH
        PRINT 'ERROR OCCURED'   
        ROLLBACK TRAN
    END CATCH

COMMIT TRAN

*/