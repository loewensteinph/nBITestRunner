using System.Configuration;

namespace TestrunnerHelper.Config
{
    public class TestRunConfigElement : ConfigurationElement
    {
        public TestRunConfigElement()
        {
        }

        public TestRunConfigElement(string nunitProjectFile, string nunitConsoleParams, string nunitBinaryPath)
        {
            NunitProjectFile = nunitProjectFile;
            NunitConsoleParams = nunitConsoleParams;
            NunitBinaryPath = nunitBinaryPath;
        }

        [ConfigurationProperty("nunitProjectFile", IsRequired = true, IsKey = false, DefaultValue = "")]
        public string NunitProjectFile
        {
            get => (string) this["nunitProjectFile"];
            set => this["nunitProjectFile"] = value;
        }

        [ConfigurationProperty("nunitConsoleParams", IsRequired = true, IsKey = false, DefaultValue = "")]
        public string NunitConsoleParams
        {
            get => (string) this["nunitConsoleParams"];
            set => this["nunitConsoleParams"] = value;
        }

        [ConfigurationProperty("nunitBinaryPath", IsRequired = true, IsKey = false, DefaultValue = "")]
        public string NunitBinaryPath
        {
            get => (string) this["nunitBinaryPath"];
            set => this["nunitBinaryPath"] = value;
        }
    }
}