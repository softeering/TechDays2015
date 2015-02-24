/**********************

drop table CSIndex;

***************/

if not exists (select * from sys.tables where name = 'CSIndex')
begin
	create table CSIndex
	(
		SubMarketID int,
		Dec1 decimal(18, 6)
	)

	-- create index IX_CSIndex_SubMarketID on CSIndex(SubMarketID);
end

if exists (select * from sys.indexes where name = 'csindex')
	drop index csindex on CSIndex;

create NONCLUSTERED columnstore index csindex on CSIndex (SubMarketID, Dec1);
