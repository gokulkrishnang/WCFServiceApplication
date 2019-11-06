--https://stackoverflow.com/questions/12957635/sql-query-to-insert-datetime-in-sql-server

You will want to use the YYYYMMDD for unambiguous date determination in SQL Server.
insert into table1(approvaldate)values('20120618 10:34:09 AM');
If you are married to the dd-mm-yy hh:mm:ss xm format, you will need to use CONVERT with the specific style.

insert table1 (approvaldate)
       values (convert(datetime,'18-06-12 10:34:09 PM',5));

https://www.c-sharpcorner.com/article/asp-net-web-api-crud-logics-using-entity-framework-without-writing-single-code/