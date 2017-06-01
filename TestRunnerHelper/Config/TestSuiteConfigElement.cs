using System.Configuration;

namespace TestrunnerHelper.Config
{
    public class TestSuiteConfigElement : ConfigurationElement
    {
        public TestSuiteConfigElement()
        {
        }

        public TestSuiteConfigElement(string name, string nBIconfigFile)
        {
            Name = name;
            NBIconfigFile = nBIconfigFile;
        }

        [ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "")]
        public string Name
        {
            get => (string) this["name"];
            set => this["name"] = value;
        }

        [ConfigurationProperty("nBIconfigFile", IsRequired = false, IsKey = false, DefaultValue = "")]
        public string NBIconfigFile
        {
            get => (string) this["nBIconfigFile"];
            set => this["nBIconfigFile"] = value;
        }
    }
}