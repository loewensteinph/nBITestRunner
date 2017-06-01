using System.Configuration;

namespace TestrunnerHelper.Config
{
    public class TestSuitesConfigElement : ConfigurationElement
    {
        public TestSuitesConfigElement()
        {
        }

        public TestSuitesConfigElement(string nBIconfigFile)
        {
            NBIconfigFile = nBIconfigFile;
        }

        [ConfigurationProperty("nBIconfigFile", IsRequired = true, IsKey = true, DefaultValue = "")]
        public string NBIconfigFile
        {
            get => (string) this["nBIconfigFile"];
            set => this["nBIconfigFile"] = value;
        }
    }
}