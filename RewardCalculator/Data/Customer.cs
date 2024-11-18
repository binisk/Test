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
	public class Customer : iCustomer
	{
		static List<Model.Customer> customers = new List<Model.Customer>();
		static string template_path = $"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Customer.csv")}";

		static  Customer()
		{
			LoadCustomers();
		}

		private static void LoadCustomers()
		{
			using (var reader = new StreamReader(template_path))
			{
				using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
				{
					customers = csv.GetRecords<Model.Customer>().ToList();
				}
			}
		}

		public bool AddCustomer(Model.Customer customer)
		{
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				// Don't write the header again.
				HasHeaderRecord = false,
			};

			try
			{
				using (var writer = new StreamWriter(template_path, true))
				{
					using (var csv = new CsvWriter(writer, config))
					{
						csv.WriteRecord<Model.Customer>(customer);
						csv.NextRecord();
					}
				}
			}
			catch
			{
				throw;
			}
			LoadCustomers();
			
			return true;
		}

		public List<Model.Customer> GetAllCustomers()
		{
			return customers;
		}

		public Model.Customer GetCustomerByID(int customerID)
		{
			return customers.FirstOrDefault(t => t.ID == customerID);
		}

		public int GetNextID()
		{
			return customers.Max(t => t.ID) + 1;
		}
	}
}
