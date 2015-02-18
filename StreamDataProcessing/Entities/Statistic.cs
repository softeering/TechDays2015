using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPSample.Entities
{
	[Serializable]
	public class Statistic
	{
		public string DataSet { get; set; }

		public DateTime? LastCallDate { get; set; }

		public int? DurationAverage { get; set; }

		public int? NumberOfCalls { get; set; }
	}
}
