## SQL Server performances (may test some stuff from this session)
>
- Indexes with included columns
- Filtered indexes for columns having small number of distinct values
- Sparse columns
- Can enable "vardecimal" at the databse or table level for NUMERIC and DECIMAL compression
- Column store
	- only load the specified column when asking for aggs on a number for example instead of loading the whole pages
	- is mostly used in data warehouse apps
	- clustered column store index: can be updated whereas the simple column store cannot
	- clustered column store index performances are close to the simple one
- in-memory
	- main goal is to avoid contentions and locking
	- if you just want to load a table into memory -> DBCC CHECK TABLE XXX (all the pages will be loaded into he memory)
	- a in-memory table supports native compiled store procedures (compiled to C++ and executed by SQL Server)
	- completely integrated into SQL Server 2014 Enterprise
	- only sequential access is done
	- never goes back to old versions
	- delta file are kept for history (deleted items for example)
	- the best performances are achieved with SSD