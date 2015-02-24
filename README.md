#Microsoft APS
>
- Microsoft Analytics Platform System appliance
- Deployed in large industries (health, financial...)
- Query Hadoop with existing T-SQL skills
- Query relational system and Hadoop in parallel with a single query
- Hybrid: host Hadoop on premises or in the cloud
- [APS whitepaper](http://www.microsoft.com/en-US/server-cloud/products/analytics-platform-system/)

#Back office for mobile applications (Azure mobile services)
>
- The SDK is available for all mobile platforms (xamrin, iOS, windows phone, javascript...)
- Can now host web jobs with scheduling in a Azure web site
- Free for limited ressources - good for testing / POC purposes
- DocumentDB and Azure Search were used during the demo (actually in preview)

#Entity Framework 7
Entity Framework 7 
>
- Multiple providers supported: azure document db, sqlite, sqlserver, azure table storage...
	- To use a provider: add a nugget package then override the 'OnConfiguring' method : options.useSqlServer
- No EDMX file anymore -> only code first
- Supported on device apps like Windows Phone and Windows store apps
- The performance has been improved

#Power BI designer is coming
>
- [Power BI designer](https://msdn.microsoft.com/en-us/powerbi/)
	- Custom connectors are coming
	- OData connector will probably become one of our best friends

#NoSQL in azure
There are several NoSQL products available in Microsoft Azure. Here is a non-exhaustive list and their main differences:
>
- Cassandra (master-less distributed NoSQL database)
	- Key - value store
	- Can simply add / remove nodes from the cluster
	- Data are automatically replicated over all nodes
	- Geographic distribution - Linear scaling
	- Titan: graph database on top of Cassandra
- MongoDB
	- Schema-less JSON documents (BSON)
	- Available in Azure too (operated by MongoDB)
	- Free on-premise
- DocumentDB
	- Microsoft NoSQL solution
	- Schema-less JSON documents
	- Supports a T-SQL like queries (which MongoDB doesn't) somewhat like Salesforce SOQL

##Concepts
- CAP: cannot guarantee the 3 followings at the same time. Some databases are good to answer 2 requirements but not all of them:
	- consistency
	- availability
	- partitioning
- Hive OCR (file format)
	- load data as in a table in hadoop and query it with SQL like language
	- [http://www.semantikoz.com/blog/orc-intelligent-big-data-file-format-hadoop-hive](http://www.semantikoz.com/blog/orc-intelligent-big-data-file-format-hadoop-hive)
	- [https://cwiki.apache.org/confluence/display/Hive/LanguageManual+ORC](https://cwiki.apache.org/confluence/display/Hive/LanguageManual+ORC)

##Limitations
- Sharding makes the followings impossible:
	- No JOIN possible
	- No cross-document transaction support
	- Transactions are possible inside a same document though
	- The sharding key should be chosen wisely (how the data will be partionned)
- Hard to have SQL query support in NoSQL:
	- 1.0: needed to install additional drivers
	- 1.5: started supporting SQL native language
	- 2.0: middleware (Apache Drill - have a look at it, Presto)
