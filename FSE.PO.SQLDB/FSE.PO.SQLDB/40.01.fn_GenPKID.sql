USE PODb;
GO

CREATE FUNCTION fn_GenPKID(@pfx CHAR(1), @id INT)
RETURNS CHAR(4)
AS
BEGIN
        RETURN @pfx + right('000' + CONVERT(VARCHAR(10), @id), 3);
END

/* Testing:
----------
	SELECT dbo.fn_GenPKID ('S', 0);		--S000
	SELECT dbo.fn_GenPKID ('S', 1);		--S001
	SELECT dbo.fn_GenPKID ('I', 2);		--I002
	SELECT dbo.fn_GenPKID ('I', 10);	--I010
	SELECT dbo.fn_GenPKID ('P', 100);	--P100
	SELECT dbo.fn_GenPKID ('P', 999);	--P999
	SELECT dbo.fn_GenPKID ('P', 1000);  --P000
	SELECT dbo.fn_GenPKID ('P', 1001);  --P001

	DROP FUNCTION fnGenPKID;
	GO

*/
 