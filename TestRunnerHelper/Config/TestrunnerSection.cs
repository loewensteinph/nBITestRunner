using System.Configuration;
using log4net;

namespace TestrunnerHelper.Config
{
    public class TestrunnerSection : ConfigurationSection
    {
        public const string SECTION_NAME = "TestrunnerSection";
        private static readonly ILog Log = LogManager.GetLogger(typeof(TestrunnerSection));

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public TestProjectCollection TestSuites
        {
            get
            {
                var testProjectCollection = (TestProjectCollection) base[""];

                return testProjectCollection;
            }
        }

        [ConfigurationProperty("runConfig")]
        public TestRunConfigElement TestRun
        {
            get
            {
                var testRunConfigElement = (TestRunConfigElement) base["runConfig"];

                return testRunConfigElement;
            }
        }
    }
}