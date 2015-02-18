#Streaming data processing (SCP)

Stream Computing Platform (SCP) is a platform to build real-time, reliable, distributed, consistent and high performance data processing applications using .NET. It is built on top of [Apache Storm](http://storm.apache.org/) -- an open source, real-time stream processing system that is available with HDInsight.

This simple example goes through 3 steps:
- The spout will connect to a dynamo datasource then emit the resulting json
- The first bolt (DeserializerBolt) deserialize the JSon result as list of entity and emit those entities
- The second bolt (SumBolt) counts the global sum of datasources' calls
- The third bolt (AverageBolt) counts the datasources' average duration

Run this project then go to bin\Debug folder. You can find different files:
- spout.txt => the output of the spout the "Data" field is in binary
- deserializer.txt => the output of the first bolt
- count.txt => the output of the second bolt
- avg.txt => the output of the third bolt
- In the log folder the complete trace of the execution

The TopologyExample.spec file shows how the topography looks like for this example.

Please note that the SCP Framework is currently in preview and thus should not be considered as stable.

[http://azure.microsoft.com/en-us/documentation/articles/hdinsight-storm-overview/](http://azure.microsoft.com/en-us/documentation/articles/hdinsight-storm-overview/)

[http://azure.microsoft.com/en-us/documentation/articles/hdinsight-hadoop-storm-scpdotnet-csharp-develop-streaming-data-processing-application/](http://azure.microsoft.com/en-us/documentation/articles/hdinsight-hadoop-storm-scpdotnet-csharp-develop-streaming-data-processing-application/)
