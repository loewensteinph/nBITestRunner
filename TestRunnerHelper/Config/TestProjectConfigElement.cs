using System.Configuration;

namespace TestrunnerHelper.Config
{
    public class TestProjectConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("nBIconfigFile", IsRequired = false, IsKey = false, DefaultValue = "")]
        public string NBIconfigFile
        {
            get => (string) this["nBIconfigFile"];
            set => this["nBIconfigFile"] = value;
        }

        [ConfigurationProperty("relativePath", IsRequired = true, IsKey = true)]
        public string RelativePath
        {
            get => (string) this["relativePath"];
            set => this["relativePath"] = value;
        }

        [ConfigurationProperty("testSuites", IsDefaultCollection = false)]
        public TestSuitesCollection TestSuites => (TestSuitesCollection) base["testSuites"];
    }
}