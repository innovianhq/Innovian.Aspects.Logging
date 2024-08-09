using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innovian.Aspects.Logging.Testing.Aspects
{
    internal class ContainsIEnumerableArgument
    {
        public void DoSomething(List<int> numbers)
        {
            Console.WriteLine($"There are {numbers.Count} numbers!");
        }
    }
}
