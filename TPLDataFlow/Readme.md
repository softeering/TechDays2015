#[TPL Data Flow](https://msdn.microsoft.com/en-us/library/hh228603(v=vs.110).aspx)

The TPL Dataflow library was introduced in the .NET framework 4.5 to help increase the robustness of concurrency-enabled applications (distributed as a NuGet package - Microsoft.Tpl.Dataflow).The dataflow model promotes actor-based programming by providing in-process message passing for coarse-grained dataflow and pipelining tasks. It is based on the Map/Reduce model to handle large volume of data. 

##Concept
The goal is to link different processing blocks to create a pipeline. Once defined, you can "start" processing the pipeline by posting item(s) to the first block.

##Pros & Cons
###Pros
- Based on the well-known built-in TPL library
- Code inside blocks is basic .NET code - no need to completely rewrite your existing code
- Several built-in blocks are available: [Blocks](https://msdn.microsoft.com/en-us/library/hh228603(v=vs.110).aspx#predefined_types)
 
###Cons / Limitations
- Learning curve: being able to build advanced / custom blocks and linking them can take some time
- Runs on a single box only (no distributed computing support)
	- some frameworks are available to workaround this limitation: akka.net
	- could add a Service bus / messaging layer to workaround this

##Benchmark
We made some tests based on a simple use case. You may have heard about the DataProcessingTool we implemented to host some of our data processing services. They're all based on the same pattern:
- Load X items from a source
- Process items one by one (transform, deep-load, ...)
- Complete the job by writing the result back to one or more repository repository

You're right; that's an ETL and you may be wondering why we're not creating an SSIS package instead of building a compiled assembly. Here are some reasons (good or not):
- Some tasks are easier to achieve using C# like built-in support for the HttpClient and JSON parsing
- Advanced multi-threading support
- Generalization: adding a new service requires writing only one class within the DataProcessingTool VS solution - easy and standardized process

###Process
Our service will:
- Read N (Items to process) items from dimhotelexpand
- Process the items one by one (parallelism set to "Workers")
	- Mimic a call to a web service (wait for 200 ms)
	- Set the value of a variable of the model
- Write all processed items to a CSV file

###Results
Here is a table summarizing the time taken by each implementation to process N (Items to process) items using X (Workers) threads.

|Workers|Items to process|TPL DataFlow|ParallelForEach|ForEach|
|---|---|---|---|---|
|10	|200	|6112 (1x)	|6337 (1.04x)	|42068 (6.88x)	|
|10	|500	|12350 (1x)	|15908 (1.29x)	|107751 (8.72x)	|
|50	|200	|2865 (1x)	|3396 (1.19x)	|42111 (14.70x)	|
|50	|500	|4302 (1x)	|15661 (3.64x)	|107443 (24.98x)|
|100|200	|2372 (1x)	|3291 (1.39x)	|42725 (18.01x)	|
|100|500	|8949 (1x)	|8949 (2.74x)	|107852 (33.07x)|

We can see that the TPL Dataflow implementation is always the fastest one even if the ParallelForEach one is close to it when the number of items is limited (200) - probably caused by a better locking mechanims within the TPL Dataflow librabry

##Conclusion
