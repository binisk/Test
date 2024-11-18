using RewardCalculator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RewardCalculator.Model;
using System.Text.RegularExpressions;
using System.Globalization;

namespace RewardCalculator.Business
{
	public class RewardTracker
	{
		iTransaction _transaction = null;
		iCustomer _customer = null;

		public RewardTracker(iTransaction transaction, iCustomer customer)
		{
			_transaction = transaction;
			_customer = customer;
		}

		public void AddTransaction(string scustomerID, string samount, string stransactionDate)
		{
			if (!Int32.TryParse(scustomerID, out int customerID))
				throw new ValidationException("Invalid Customer ID");
			if(_customer.GetCustomerByID(customerID) == null)
				throw new ValidationException($" Customer with ID {customerID} does not exist");
			if (!Double.TryParse(samount, out double amount))
				throw new ValidationException("Invalid Amount");
			if (!DateTime.TryParseExact(stransactionDate,"MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime transactionDate))
				throw new ValidationException("Invalid Date");


			long  transactionID = _transaction.GetNextID();
			Transaction transaction = new Transaction(transactionID,customerID, amount, transactionDate);
			_transaction.AddTransaction(transaction);
		}

		public void AddCustomer(string customerName)
		{
			if (!Regex.IsMatch(customerName, @"^[a-zA-Z\s]+$"))
				throw new ValidationException("Invalid Customer Name");

			int customerID = _customer.GetNextID();

			Customer customer = new Customer(customerID,customerName);
			_customer.AddCustomer(customer);
		}

		public void GetAllCustomers()
		{
			var customers = _customer.GetAllCustomers();
			foreach (var customer in customers)
			{
				Console.WriteLine($"Customer ID {customer.ID} , Customer Name : {customer.Name}");
			}
		}

		public void GetAllTransactions()
		{
			var transactions = _transaction.GetAllTransactions();
			foreach (var transaction in transactions.OrderBy(t=>t.CustomerID).ThenBy(t=>t.Date))
			{
				Console.WriteLine($"Customer ID {transaction.CustomerID} , TransactionId {transaction.ID}, Amount : {transaction.Amount} , Date :{transaction.Date}");
			}
		}

		public void GetRewards(string  scustomerID)
		{
			if (!Int32.TryParse(scustomerID, out int customerID))
				throw new ValidationException("Invalid Customer ID");
			if (_customer.GetCustomerByID(customerID) == null)
				throw new ValidationException($" Customer with ID {customerID} does not exist");

			var customer = _customer.GetCustomerByID(customerID);
			List<Customer> customers = new List<Customer>();
			customers.Add(customer);

			var transactions = _transaction.GetTransactionByCustomerID(customerID);

			CalculateRewards(customers, transactions);
		}

		public void GetRewards(string  scustomerID, string sstartDate, string sendDate)
		{

			if (!Int32.TryParse(scustomerID, out int customerID))
				throw new ValidationException("Invalid Customer ID");
			if (_customer.GetCustomerByID(customerID) == null)
				throw new ValidationException($" Customer with ID {customerID} does not exist");

			if (!DateTime.TryParseExact(sstartDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
				throw new ValidationException("Invalid Start Date");

			if (!DateTime.TryParseExact(sendDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate))
				throw new ValidationException("Invalid End Date");

			if(startDate > endDate)
				throw new ValidationException("Start Date should be less than End Date");

			var customer = _customer.GetCustomerByID(customerID);
			List<Customer> customers = new List<Customer>();
			customers.Add(customer);

			var transactions = _transaction.GetTransactionByCustomerID(customerID, startDate, endDate);

			CalculateRewards(customers, transactions);
		}

		public void GetAllRewards()
		{
			var customers = _customer.GetAllCustomers();
			var transactions = _transaction.GetAllTransactions();

			CalculateRewards(customers, transactions);
		}

		public void GetAllRewards(string sstartDate, string sendDate)
		{

			if (!DateTime.TryParseExact(sstartDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
				throw new ValidationException("Invalid Start Date");

			if (!DateTime.TryParseExact(sendDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate))
				throw new ValidationException("Invalid End Date");

			if (startDate > endDate)
				throw new ValidationException("Start Date should be less than End Date");

			var customers = _customer.GetAllCustomers();
			var transactions = _transaction.GetAllTransactions(startDate, endDate);

			CalculateRewards(customers, transactions);
		}

		private static void CalculateRewards(List<Customer> customers, List<Transaction> transactions)
		{
			if (customers == null || customers.Count == 0)
				throw new ApplicationException("No Customer retrieved");


			//Left Join in case we have customers without transactions
			var result = from customer in customers
						 join transaction in transactions on customer.ID equals transaction.CustomerID into customerTransactions
						 from customerTransaction in customerTransactions.DefaultIfEmpty()
						 select new Model.RewardTracker() { ID = customer.ID, Name = customer.Name, Amount = customerTransaction == null ? default(double):customerTransaction.Amount, RewardMonth = customerTransaction == null ? default(DateTime) : new DateTime(customerTransaction.Date.Year, customerTransaction.Date.Month, 1) };

			var monthlyRewards = result
				.GroupBy(l => new { l.ID, l.RewardMonth })
				.Select(cl => new
				{
					ID = cl.First().ID,
					RewardMonth = cl.First().RewardMonth,
					MonthlyRewardPoints = cl.Sum(c => c.RewardPoint)

				}).ToList();

			foreach (var customer in customers)
			{
				Console.WriteLine($"Customer ID {customer.ID} , Customer Name : {customer.Name}");
				long totalRewards = 0;
				foreach (var customerMonthly in monthlyRewards.Where(t=>t.ID == customer.ID))
				{
					if (customerMonthly.RewardMonth.Value == default(DateTime))
						continue;
					Console.WriteLine($"Year: {customerMonthly.RewardMonth.Value.Year} Month :{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(customerMonthly.RewardMonth.Value.Month)} MonthlyReward :{customerMonthly.MonthlyRewardPoints} ");
					totalRewards += customerMonthly.MonthlyRewardPoints;
				}
				Console.WriteLine($"Total Rewards {totalRewards}");
				Console.WriteLine("");
			}

		}

	}
}
