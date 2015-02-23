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

##In-memory tables
- main goal is to avoid contentions and locking
- if you just want to load a table into memory -> DBCC CHECK TABLE XXX (all the pages will be loaded into he memory)
- a in-memory table supports native compiled store procedures (compiled to C++ and executed by SQL Server)
- completely integrated into SQL Server 2014 Enterprise
- only sequential access is done
- never goes back to old versions
- delta file are kept for history (deleted items for example)
- the best performances are achieved with SSD
