﻿// This file would be loaded in HDInsight
// bin\runspec TopologyExample.spec temp StreamingDataProcessing

{
  :name "StreamingDataProcessing"
  :topology
    (nontx-topolopy
      "StreamingDataProcessing"

      { "spout" (spout-spec (scp-spout  {
              "plugin.name" "StreamingDataProcessing.exe"
              "plugin.args" ["dataloadspout"]
              "output.schema" {"default" ["json"]}
            })
		:p 1)
      }

      { 
		"deserializerbolt" (bolt-spec { "spout" :shuffle }
			(scp-bolt
			{
				"plugin.name" "StreamingDataProcessing.exe"
				"plugin.args" ["deserializerbolt"]
				"output.schema" {"default" ["entity"]}
			})
		:p 1)

        "sumbolt"  (bolt-spec { "deserializerbolt" :global  }
			(scp-bolt
			{
				"plugin.name" "StreamingDataProcessing.exe"
				"plugin.args" ["sumbolt"]
				"output.schema" {"default" ["count"]}
			})
		:p 1)

		 "averagebolt"  (bolt-spec { "deserializerbolt" :global  }
			(scp-bolt
			{
				"plugin.name" "StreamingDataProcessing.exe"
				"plugin.args" ["averagebolt"]
				"output.schema" {"default" ["avg"]}
			})
		:p 1)
      })

  :config
    {
      "topology.kryo.register" ["[B"]
    }
}