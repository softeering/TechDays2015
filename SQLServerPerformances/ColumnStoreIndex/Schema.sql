/**********************

drop table CSIndex;

***************/

if not exists (select * from sys.tables where name = 'CSIndex')
begin
	create table CSIndex
	(
		SubMarket nvarchar(50),
		Dec1 decimal(18, 6)
	)

	-- create index IX_CSIndex_SubMarket on CSIndex(SubMarket);
end

if exists (select * from sys.indexes where name = 'csindex')
	drop index csindex on CSIndex;

create NONCLUSTERED columnstore index csindex on CSIndex (SubMarket, Dec1);
