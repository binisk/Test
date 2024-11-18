using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RewardCalculator.Model
{
	public class Customer
	{
		public int ID { get; private set; }
		public string Name { get; private set; }

		public Customer(int ID, string Name)
		{
			this.ID = ID;
			this.Name = Name;
		}
	}
}
