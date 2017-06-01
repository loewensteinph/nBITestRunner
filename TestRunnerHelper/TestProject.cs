using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TestrunnerHelper
{
    public class TestProject
    {
        public TestProject(string pathToTestProject, string nunitProjectFileName)
        {
            PathToTestProject = pathToTestProject;
            ExecutionPath = Directory.GetCurrentDirectory();
            TestSuites = new List<TestSuite>();
            NunitProjectFileName = nunitProjectFileName;
            NunitProjectFile = GetNunitProjectFile();
        }

        public string ExecutionPath { get; set; }
        public List<TestSuite> TestSuites { get; set; }
        public string PathToTestProject { get; set; }
        public string NunitProjectFileName { get; set; }
        public NunitProjectFile NunitProjectFile { get; set; }

        public NunitProjectFile GetNunitProjectFile()
        {
            var newAppBase = Path.GetFullPath(Path.Combine(ExecutionPath, PathToTestProject));
            newAppBase = Path.GetFullPath(Path.Combine(newAppBase, @"..\"));

            var nunitProjectFile = new NunitProjectFile(newAppBase);

            nunitProjectFile.FullNameNunitProjectFilename =
                Directory.GetFiles(ExecutionPath, NunitProjectFileName, SearchOption.AllDirectories).FirstOrDefault();
            return nunitProjectFile;
        }
    }
}