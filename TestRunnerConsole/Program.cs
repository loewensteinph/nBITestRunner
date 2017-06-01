using TestrunnerHelper;

namespace TestRunnerConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var testrun = new TestRun();
            var testsuites = testrun.RunTests();
        }
    }
}