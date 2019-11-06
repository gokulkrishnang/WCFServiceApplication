USE PODb;
GO

--EXEC dbo.sp_GetPurchaseRecord 'P001'
--EXEC dbo.sp_GetPurchaseRecord 'P003'
--EXEC dbo.sp_GetPurchaseRecord 'P004'
--EXEC dbo.sp_GetPurchaseRecord 'P100'

ALTER PROCEDURE dbo.sp_GetPurchaseRecord (
@poNO CHAR(4) )
AS
BEGIN

       SELECT PM.PONO
            , PM.SUPLNO
            , SL.SUPLNAME
            , PD.ITCODE
            , IT.ITDESC
            , PD.QTY
            , PM.PODATE
        FROM POMASTER PM
			INNER JOIN PODETAIL PD
        ON PM.PONO = PD.PONO
            INNER JOIN SUPPLIER SL
        ON PM.SUPLNO = SL.SUPLNO
            INNER JOIN ITEM IT
        ON PD.ITCODE = IT.ITCODE
        WHERE PM.PONO = @poNO;

END;
GO