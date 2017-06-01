using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestrunnerHelper
{
    public class NBiConfigFile
    {
        public NBiConfigFile(string fileNameNBiConfigFile, string pathToTestProject)
        {
            ExecutionPath = Directory.GetCurrentDirectory();
            FileNameNBiConfigFile = fileNameNBiConfigFile;
            PathToTestProject = pathToTestProject;
            FullNameNBiConfigFile = GetNBiConfigFile();
        }

        public string ExecutionPath { get; set; }
        public string PathToTestProject { get; set; }
        public string FileNameNBiConfigFile { get; set; }
        public string FullNameNBiConfigFile { get; set; }
        private string _relativePathToNBiConfigFile { get; set; }

        public string RelativePathToNBiConfigFile
        {
            get => PathHelper.MakeRelative(ExecutionPath, FullNameNBiConfigFile);
            set => _relativePathToNBiConfigFile = value;
        }

        public string GetNBiConfigFile()
        {
            var searchPatch = Path.GetFullPath(Path.Combine(ExecutionPath, PathToTestProject));

            var FullNameNBiConfigFile =
                Directory.GetFiles(searchPatch, FileNameNBiConfigFile, SearchOption.AllDirectories)
                    .FirstOrDefault(path => !path.Contains(@"\bin\"));
            return FullNameNBiConfigFile;
        }

        /// <summary>
        ///     Setzt in Test.config die aktuell auszuführende Test Suite
        /// </summary>
        public void PrepareNunitConfig(TestSuite currentTestSuite)
        {
            try
            {
                var searchPatch =
                    Path.GetFullPath(Path.Combine(ExecutionPath, currentTestSuite.NBiConfigFile.PathToTestProject));
                var pathToTestSuite =
                    Directory.GetFiles(searchPatch, string.Format(@"{0}.nbits", currentTestSuite.TestSuiteName),
                        SearchOption.AllDirectories).FirstOrDefault(path => !path.Contains(@"\bin\"));
                var relativePathToTestSuite = PathHelper.MakeRelative(ExecutionPath, pathToTestSuite);
                relativePathToTestSuite = relativePathToTestSuite.Replace(@"..\..\..\", "");

                var doc = new XmlDocument();
                var streamreader = new StreamReader(FullNameNBiConfigFile);
                doc.PreserveWhitespace = true;
                doc.Load(streamreader);

                var nodeList = doc.SelectNodes("//nbi");
                if (nodeList != null)
                    foreach (XmlNode node in nodeList)
                        if (node.Attributes != null)
                            node.Attributes["testSuite"].Value = string.Format(@"{0}", relativePathToTestSuite);

                doc.PreserveWhitespace = true;
                streamreader.Close();
                var wrtr = new XmlTextWriter(FullNameNBiConfigFile, Encoding.ASCII);

                doc.WriteTo(wrtr);
                wrtr.Close();
            }
            catch (Exception ex)
            {
                Debug.Write("Aktuelle TestProject nbits konnte nicht angepasst werden: " + ex.Message);
            }
        }
    }
}