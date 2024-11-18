using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RewardCalculator.Model
{
	public class RewardTracker
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public DateTime? RewardMonth { get; set; }
		public double Amount { get; set; }
		
		public int RewardPoint
		{
			get
			{
				if (Amount <= 50 ) return 0;
				//ignore the decimal part of the amount
				if (Amount > 50 && Amount <= 100) return (int)Amount - 50;
				else
				{
					//Over 100. Hence 50 reward guaranteed for 51 -100
					return (((int)Amount - 100) * 2) + 50;
				}
			}

		}
	}
}
