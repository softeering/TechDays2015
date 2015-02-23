-- http://blog.sqlauthority.com/2011/10/29/sql-server-fundamentals-of-columnstore-index/

set statistics IO on;
set statistics time on;
dbcc dropcleanbuffers;

select 
	SubMarket, 
	sum(Dec1)
from CSIndex
group by SubMarket
option (IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX);

go

set statistics IO on;
set statistics time on;
dbcc dropcleanbuffers;

select 
	SubMarket, 
	sum(Dec1)
from CSIndex
group by SubMarket
