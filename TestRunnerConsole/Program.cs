using TestrunnerHelper;

namespace TestRunnerConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var testrun = new NbiTestRun();
            var testsuites = testrun.RunTests();
        }
    }
}