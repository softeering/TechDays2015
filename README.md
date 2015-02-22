#Microsoft APS
>
- Microsoft Analytics Platform System appliance
- deployed in large industries (health, financial...)
- [APS whitepaper](http://www.microsoft.com/en-US/server-cloud/products/analytics-platform-system/)

#Back office for mobile applications (Azure mobile services)
>
- The SDK available for all mobile platforms (xamrin, iOS, windows phone, javascript...)
- Can now host web jobs with scheduling in a Azure web site
- Free for limited ressources - good for testing / POC purposes
- DocumentDB and Azure Search were used during the demo (actually in preview)

#Power BI designer is coming
>
- [Power BI designer](https://msdn.microsoft.com/en-us/powerbi/)
	- custom connectors are coming
	- OData connector will probably become one of our best friends

#NoSQL in azure
There are a lot of NoSQL products available in Microsoft Azure. Here is a non-exhaustive list and their main 
>
- Cassandra (master-less distributed NoSQL database)
	- key - value store
	- can simply add / remove nodes from the cluster
	- data are automatically replicated over all nodes
	- Titan: graph database on top of Cassandra
- MongoDB
	- schema-less JSON documents (BSON)
	- available in Azure too (operated by MongoDB)
	- free on-premise
- DocumentDB
	- Microsoft NoSQL solution
	- schema-less JSON documents
	- supports a T-SQL like queries (which MongoDB doesn't) somewhat like Salesforce SOQL

##Concepts
- CAP: cannot guarantee the 3 followings at the same time. Some databases are good to answer 2 requirements but not all of them:
	- consistency
	- availability
	- partitioning
- Hive OCR (file format)
	- [http://www.semantikoz.com/blog/orc-intelligent-big-data-file-format-hadoop-hive](http://www.semantikoz.com/blog/orc-intelligent-big-data-file-format-hadoop-hive)
	- [https://cwiki.apache.org/confluence/display/Hive/LanguageManual+ORC](https://cwiki.apache.org/confluence/display/Hive/LanguageManual+ORC)

##Limitations
- sharding makes the followings impossible:
	- no JOIN possible
	- no cross-document transaction support
- SQL query in NoSQL starts being available:
	- 1.0: driver
	- 1.5: SQL native language
	- 2.0: middleware (Apache Drill - have a look at it, Presto)
- Hive + ORC
	- SQL without any database
	- load data as in a table in hadoop and query it with SQL like language
