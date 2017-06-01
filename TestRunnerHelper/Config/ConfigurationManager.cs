using System;
using System.Configuration;
using System.IO;

namespace TestrunnerHelper.Config
{
    public class ConfigurationFinder
    {
        public TestrunnerSection Find()
        {
            var configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            //Try to find a config file, if existing take the path inside for the TestProject
            if (File.Exists(configFile))
            {
                //line bellow to avoid .Net framework bug: http://support.microsoft.com/kb/2580188/en-us
                var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                var section = (TestrunnerSection) configuration.GetSection("testrun");
                if (section != null)
                    return section;
            }
            return new TestrunnerSection();
        }
    }
}