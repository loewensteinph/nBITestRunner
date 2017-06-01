using System.Collections.Generic;
using System.IO;

namespace TestrunnerHelper
{
    public class TestSuite
    {
        public TestSuite(string pathToTestProject, string fileNameNBiConfigFile)
        {
            PathToTestProject = pathToTestProject;
            ExecutionPath = Directory.GetCurrentDirectory();
            TestsToRun = new List<string>();
            NBiConfigFile = new NBiConfigFile(fileNameNBiConfigFile, pathToTestProject);
        }

        public string ExecutionPath { get; set; }
        public ResultStatus ResultStatus { get; set; }
        public string PathToTestProject { get; set; }
        public NBiConfigFile NBiConfigFile { get; set; }
        public string FullNamenNunitConfigFileName { get; set; }
        public string TestSuiteName { get; set; }

        private string _testsToRunConcat { get; set; }

        public string TestsToRunConcat
        {
            get => FormatTestsToRun();
            set => _testsToRunConcat = value;
        }

        public List<string> TestsToRun { get; set; }

        public string ResultFilenameHTML => Path.ChangeExtension(TestSuiteName, "html");

        public string ResultFilenameXML => Path.ChangeExtension(TestSuiteName, "xml");

        private string FormatTestsToRun()
        {
            string fixedString = null;
            if (TestsToRun.Count > 0)
            {
                var fixedList = new List<string>();

                foreach (var test in TestsToRun)
                {
                    var newtest = string.Format("NBi.NUnit.Runtime.TestProject.{0}", test);
                    fixedList.Add(newtest);
                }
                fixedString = string.Join(",", fixedList);
            }
            return fixedString;
        }
    }
}