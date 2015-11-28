using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryTreeApp
{
	public class RandomValueGenerator
	{
		public RandomValueGenerator ()
		{
		}


		public IEnumerable<int> GetRandomNumbers(int count = 1, IEnumerable<int> excludeList = null)
		{
			Random random = new Random ();

			for (int i = 0; i < count; i++) {

					var randomNumber = random.Next (10, 100);

					if (excludeList != null && excludeList.Contains (randomNumber))
						yield return GetRandomNumbers (1, excludeList).First();
					else
						yield return randomNumber;
			}
		}
	}
}

