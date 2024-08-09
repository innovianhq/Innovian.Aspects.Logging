namespace Innovian.Aspects.Logging.Testing.Aspects
{
    internal class ReturnListTypeInMethod
    {
        public List<int> DoSomething()
        {
            var numbers = new List<int>();
            for (var a = 0; a < 1000; a++)
            {
                numbers.Add(a);
            }
            return numbers;
        }
    }
}
