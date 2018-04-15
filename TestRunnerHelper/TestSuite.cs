using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TestrunnerHelper.Log;

namespace TestrunnerHelper
{
    public class TestSuite
    {
        private static readonly ILog Log = LogFactory.GetLogger(typeof(TestSuite));
        public TestSuite(string pathToTestProject, string fileNameNBiConfigFile)
        {
            PathToTestProject = pathToTestProject;
            ExecutionPath = Directory.GetCurrentDirectory();
            TestsToRun = new List<string>();
            NBiConfigFile = new NBiConfigFile(fileNameNBiConfigFile, pathToTestProject);
            ISO_Date = DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss_ffff");
            CheckResultFolderExists();
        }

        public string ExecutionPath { get; set; }
        public ResultStatus ResultStatus { get; set; }
        public string PathToTestProject { get; set; }
        public NBiConfigFile NBiConfigFile { get; set; }
        public string FullNamenNunitConfigFileName { get; set; }
        public string TestSuiteName { get; set; }
        public string TestSuiteFulllName => Path.ChangeExtension(TestSuiteName, "nbits");

        private string _testsToRunConcat { get; set; }

        public string TestsToRunConcat
        {
            get => FormatTestsToRun();
            set => _testsToRunConcat = value;
        }

        public List<string> TestsToRun { get; set; }


        public string ISO_Date { get; set; }

        public string ResultFilenameHTML => Path.ChangeExtension(TestSuiteName + ISO_Date, "html");

        public string ResultFullFilenameHTML => Path.GetFullPath(Path.Combine(Path.Combine(ExecutionPath, "TestResults"), ResultFilenameHTML));

        public string ResultFilenameXML => Path.ChangeExtension(TestSuiteName + ISO_Date, "xml");

        public string ResultFullFilenameXML => Path.GetFullPath(Path.Combine(Path.Combine(ExecutionPath, "TestResults"), ResultFilenameXML));

        private string FormatTestsToRun()
        {
            string fixedString = null;
            if (TestsToRun.Count > 0)
            {
                var fixedList = new List<string>();

                foreach (var test in TestsToRun)
                {
                    var newtest = string.Format("NBi.NUnit.Runtime.TestSuite.{0}", test);
                    fixedList.Add(newtest);
                }
                fixedString = string.Join(",", fixedList);
            }
            return fixedString;
        }

        public bool CheckFileExists()
        {
            var searchPatch = Path.GetFullPath(Path.Combine(ExecutionPath, PathToTestProject));

            var FullNameNBiConfigFile =
                Directory.GetFiles(searchPatch, TestSuiteFulllName, SearchOption.AllDirectories).FirstOrDefault(path => !path.Contains(@"\bin\"));

            if (FullNameNBiConfigFile != null)
                return true;

            return false;
        }

        public void CheckResultFolderExists()
        {
            if (!Directory.Exists(Path.Combine(ExecutionPath, "TestResults")))
            {
                Directory.CreateDirectory(Path.Combine(ExecutionPath, "TestResults"));
            }
        }
    }
}