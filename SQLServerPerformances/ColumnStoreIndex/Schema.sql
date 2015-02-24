use SQLServerPerf;

/**********************

drop table CSIndex;

***************/

if not exists (select * from sys.tables where name = 'CSIndex')
begin
	create table CSIndex
	(
		SubMarketID int,
		MarketID int,
		Int1 int,
		Bigint1 bigint,
		Dec1 decimal(18, 6)
	)
end

if exists (select * from sys.indexes where name = 'csindex')
	drop index csindex on CSIndex;

-- create NONCLUSTERED columnstore index csindex on CSIndex (SubMarketID, Int1, Bigint1, Dec1);

create clustered index cindex on CSIndex (SubMarketID, Int1, Bigint1, Dec1)