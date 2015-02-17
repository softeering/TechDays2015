using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Newtonsoft.Json.Linq;

namespace TPL.DataFlow.Processor.Core
{
	public abstract class ProcessorBase<T> where T : class
	{
		public ProcessorBase(JObject config = null)
		{
			this._additionalBlocks = new List<IDataflowBlock>();
			if (config != null)
				this.LoadConfig(config);
		}

		protected virtual void LoadConfig(JObject config)
		{

		}

		protected abstract Task<BufferBlock<T>> GetItemsToProcess();
		protected BufferBlock<T> StartBlockInstance { get; private set; }

		private readonly List<IDataflowBlock> _additionalBlocks;
		protected void RegisterAdditionalBlock(IDataflowBlock block)
		{
			if (block == null)
				throw new ArgumentNullException("block");

			var lastItem = this._additionalBlocks.FirstOrDefault() ?? this.StartBlockInstance;

			this._additionalBlocks.Add(block);

			lastItem.Completion.ContinueWith(t =>
			{
				if (t.IsFaulted)
					((IDataflowBlock)block).Fault(t.Exception);
				else
					block.Complete();
			});
		}

		protected abstract void AddAdditionalBlock();

		private IDataflowBlock LastBlock
		{
			get
			{
				if (this._additionalBlocks != null && this._additionalBlocks.Any())
					return this._additionalBlocks.Last();

				return this.StartBlockInstance;
			}
		}

		public async Task ProcessAsync()
		{
			var buffer = await this.GetItemsToProcess();
			this.StartBlockInstance = buffer;
			this.AddAdditionalBlock();
			buffer.Complete();
			await this.LastBlock.Completion;
		}
	}
}
