using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innovian.Aspects.Logging.Testing.Aspects
{
    internal class ContainsPrimitiveArgument
    {
        public int DoSomething(int number, string name)
        {
            Console.WriteLine("Expected a name {name} and a number {number}", name, number);
            return number;
        }
    }
}
