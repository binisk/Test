using CsvHelper;
using CsvHelper.Configuration;
using RewardCalculator.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RewardCalculator.Data
{
	class Transaction : iTransaction
	{
		static List<Model.Transaction> transactions = new List<Model.Transaction>();
		static string template_path = $"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Transaction.csv")}";

		static Transaction()
		{
			LoadTransactions();
		}

		private static void LoadTransactions()
		{
			using (var reader = new StreamReader(template_path))
			{
				using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
				{
					transactions = csv.GetRecords<Model.Transaction>().ToList();
				}
			}
		}
		public bool AddTransaction(Model.Transaction transaction)
		{
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				// Don't write the header again.
				HasHeaderRecord = false,
			};

			try
			{
				using (var writer = new StreamWriter(template_path,true))
				{
					using (var csv = new CsvWriter(writer, config))
					{
						csv.WriteRecord<Model.Transaction>(transaction);
						csv.NextRecord();
					}
				}
			}
			catch
			{
				throw;
			}
			LoadTransactions();

			return true;
		}

		public List<Model.Transaction> GetAllTransactions()
		{
			return transactions;
		}

		public List<Model.Transaction> GetAllTransactions(DateTime startDate, DateTime endDate)
		{
			return transactions.Where(t => t.Date >= startDate && t.Date<= endDate).ToList();
		}

		public long GetNextID()
		{
			return transactions.Max(t=>t.ID) + 1;
		}

		public List<Model.Transaction> GetTransactionByCustomerID(int customerID)
		{
			return transactions.Where(t => t.CustomerID == customerID).ToList();
		}

		public List<Model.Transaction> GetTransactionByCustomerID(int customerID, DateTime startDate, DateTime endDate)
		{
			return transactions.Where(t => t.CustomerID == customerID && (t.Date >= startDate && t.Date <= endDate)).ToList();
		}
	}
}
