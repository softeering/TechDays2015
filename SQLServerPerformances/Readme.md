#SQL Server performances
##[COLUMNSTORE indexes](https://msdn.microsoft.com/en-US/library/gg492088(v=sql.110).aspx)
COLUMNSTORE indexes have been introduced in SQL Server 2012.
###Pros
- only load the specified column when asking for aggs on a number for example instead of loading the whole pages
- for Expedient: COLUMNSTORE index are mostly used in data warehouse apps
###Cons
- a table with a COLUMNSTORE index cannot be updated (this will change in SQL Server 2014)
	- thanks to the CLUSTERED COLUMNSTORED index
	- clustered column store index performances are close to the simple one
	- this is not a big deal when it comes to an aggregates table in a datawarehouse environment

###Benchmark
- A table with 5 000 000 rows and the following columns has been created
	- SubMarketID (int): 1 to 1000
	- MarketID (int): 1 to 200
	- Int1 (int): 1 to 1 000 000
	- Bigint1 (bigint): 1000 to 1 000 000 000
	- Dec1 (decimal): random values

- And the following query has been executed twice for each type of index (while taking care of cleaning the buffers before each run)

```tsql
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
```

####Results
|Index|Run|Scan count|Logical reads|CPU time (ms)|Elapsed time (ms)|
|---|---|---|---|---|---|
|NO INDEX	|1	|5	|23475	|6015	|1627	|
|NO INDEX	|2	|5	|23475	|5937	|1610	|
|CLUSTERED	|1	|5	|24021	|3452	|1030	|
|CLUSTERED	|2	|5	|24021	|3296	|988	|
|COLUMNSTORE|2	|4	|12894	|501	|471	|
|COLUMNSTORE|2	|4	|12894	|563	|405	|

We can see that, even if the clustered index brings better performances, the columnstore index is far the best one we can use for such a scenario (i.e. aggregates table with a lot of rows)

##In-memory tables
- main goal is to avoid contentions and locking
- if you just want to load a table into memory -> DBCC CHECK TABLE XXX (all the pages will be loaded into he memory)
- a in-memory table supports native compiled store procedures (compiled to C++ and executed by SQL Server)
- completely integrated into SQL Server 2014 Enterprise
- only sequential access is done
- never goes back to old versions
- delta file are kept for history (deleted items for example)
- the best performances are achieved with SSD
