using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RewardCalculator.Model
{
	public class Transaction
	{
		public long ID { get; private set; }
		public int CustomerID { get; private set; }
		public double Amount { get; private set; }
		public DateTime Date { get; private set; }

		public Transaction(long ID, int CustomerID, double Amount, DateTime Date)
		{
			this.ID = ID;
			this.CustomerID = CustomerID;
			this.Amount = Amount;
			this.Date = Date;
		}


	}
}
