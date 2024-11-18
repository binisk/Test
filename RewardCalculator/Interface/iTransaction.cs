using RewardCalculator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RewardCalculator.Interface
{
	public interface iTransaction
	{
		bool AddTransaction(Transaction transaction);
		List<Transaction> GetTransactionByCustomerID(int customerID);
		List<Transaction> GetTransactionByCustomerID(int customerID,DateTime startDate, DateTime endDate);
		List<Transaction> GetAllTransactions();
		List<Transaction> GetAllTransactions(DateTime startDate,DateTime endDate);
		long GetNextID();

	}
}
