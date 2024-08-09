namespace Innovian.Aspects.Logging.Testing.Aspects
{
    internal class ContainsComplexArgument
    {
        public int DoSomething(Random rand)
        {
            return rand.Next();
        }
    }
}
