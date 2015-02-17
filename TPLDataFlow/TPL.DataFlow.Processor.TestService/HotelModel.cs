using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPL.DataFlow.Processor.TestService
{
	public class HotelModel
	{
		public long HotelKey { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public string Country { get; set; }

		public override string ToString()
		{
			return string.Format("{0},{1},{2},{3}", this.HotelKey, this.Latitude, this.Longitude, this.Country);
		}
	}
}
