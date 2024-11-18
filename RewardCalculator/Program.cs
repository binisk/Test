using RewardCalculator.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RewardCalculator
{
	class Program
	{
		static void Main(string[] args)
		{
			OptionScreen();

			RewardTracker tracker = new RewardTracker(new Data.Transaction(), new Data.Customer());

			string customerName, amount, transactionDate, customerID, startDate,endDate;
			string option;
			do
			{
				option = Console.ReadLine();
				try
				{
					
					switch (option)
					{
						case "1":
							Console.WriteLine("Enter a new customer name :");
							customerName = Console.ReadLine();
							tracker.AddCustomer(customerName);
							Console.WriteLine("Customer Added");
							break;

						case "2":
							Console.WriteLine("Enter a valid customer ID:");
							customerID = Console.ReadLine();

							Console.WriteLine("Enter an amount:");
							amount = Console.ReadLine();

							Console.WriteLine("Enter the transaction date(mm/dd/yyyy):");
							transactionDate = Console.ReadLine();

							tracker.AddTransaction(customerID, amount, transactionDate);

							Console.WriteLine("Transaction Added");

							break;
						case "3":
							Console.WriteLine("Get all customers");
							tracker.GetAllCustomers();
							break;
						case "4":
							Console.WriteLine("Get all transactions");
							tracker.GetAllTransactions();
							break;

						case "5":
							Console.WriteLine("Calculate Rewards for a Customer");

							Console.WriteLine("Enter a valid customer ID:");
							customerID = Console.ReadLine();
							tracker.GetRewards(customerID);

							break;
						case "6":
							Console.WriteLine("Calculate Rewards for a Customer between given dates");
							Console.WriteLine("Enter a valid customer ID:");
							customerID = Console.ReadLine();

							Console.WriteLine("Start Date (mm/dd/yyyy):");
							startDate = Console.ReadLine();

							Console.WriteLine("End Date(mm/dd/yyyy):");
							endDate = Console.ReadLine();

							tracker.GetRewards(customerID, startDate, endDate);

							break;
						case "7":
							Console.WriteLine("Calculate Rewards for all Customers");
							tracker.GetAllRewards();
							break;
						case "8":
							Console.WriteLine("Calculate Rewards for all Customers between given dates");
							Console.WriteLine("Start Date (mm/dd/yyyy):");
							startDate = Console.ReadLine();

							Console.WriteLine("End Date(mm/dd/yyyy):");
							endDate = Console.ReadLine();

							tracker.GetAllRewards(startDate, endDate);

							break;
						case "9":
							break;
						default:
							Console.WriteLine($"Invalid Option {option} selected");
							break;
					}
				}
				catch (ValidationException ex)
				{
					Console.WriteLine($"Validation exception {ex.Message}");
				}
				catch(Exception ex)
				{
					Console.WriteLine($"Unhandled exception {ex}");
					break;
				}

				Console.Write("Press any key to clear screen...");
				Console.ReadKey();
				OptionScreen(true);

			} while (option != "9") ;
				// Wait for the user to respond before closing.
			Console.Write("Press any key to close the app...");
			Console.ReadKey();

		}

		private static void OptionScreen(bool clearScreen = false)
		{
			if (clearScreen)
			{
				Console.Clear();
			}

			Console.WriteLine("Choose an option from the following list:");
			Console.WriteLine("\t1 - Add Customer");
			Console.WriteLine("\t2 - Add Transaction");
			Console.WriteLine("\t3 - Get All Customers");
			Console.WriteLine("\t4 - Get All Transactions");

			Console.WriteLine("\t5 - Calculate Rewards for a Customer");
			Console.WriteLine("\t6 - Calculate Rewards for a Customer between given dates");
			Console.WriteLine("\t7 - Calculate Rewards for all Customers");
			Console.WriteLine("\t8 - Calculate Rewards for all Customers between given dates");

			Console.WriteLine("\t9 - Close Application");

			Console.Write("Your option? ");
		}
	}
}
