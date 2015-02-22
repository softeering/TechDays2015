
CREATE NONCLUSTERED COLUMNSTORE INDEX csindx_simple
ON WithCSIndex (Int1);
GO

CREATE NONCLUSTERED COLUMNSTORE INDEX csindx_simple_dec
ON WithCSIndex (Dec1);
GO

select SubMarket, sum(CAST(Int1 AS BIGINT)), avg(CAST(Dec1 AS BIGINT))
from WithCSIndex
group by SubMarket

go

select SubMarket, sum(CAST(Int1 AS BIGINT)), avg(CAST(Dec1 AS BIGINT))
from WithoutCSIndex
group by SubMarket
