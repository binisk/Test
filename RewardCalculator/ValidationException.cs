using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RewardCalculator
{
	public class ValidationException:ApplicationException
	{
		public ValidationException(string message) : base(message)
		{
		}
	}
}
