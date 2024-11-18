using RewardCalculator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RewardCalculator.Interface
{
	public interface iCustomer
	{
		bool AddCustomer(Customer customer);
		int GetNextID();
		Customer GetCustomerByID(int customerID);
		List<Customer> GetAllCustomers();

	}
}
