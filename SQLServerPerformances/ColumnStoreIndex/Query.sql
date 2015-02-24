use SQLServerPerf;

set statistics IO on;
set statistics time on;
dbcc dropcleanbuffers;

select 
	SubMarketID, 
	sum(cast(Int1 as bigint)) as SumInt,
	sum(Bigint1) as SumBigint,
	sum(Dec1) as SumDec,
	sum(cast(Int1 as bigint)) / avg(Dec1) as SunAvg
from CSIndex
group by SubMarketID
-- option (IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX);
