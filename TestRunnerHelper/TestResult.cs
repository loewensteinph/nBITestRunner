using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestrunnerHelper
{
    [Serializable()]
    [XmlRoot("test-run", Namespace = "", IsNullable = false)]
    public class TestRun
    {
        [XmlAttribute]
        public string testcasecount { get; set; }
        [XmlAttribute]
        public string result { get; set; }
        [XmlElement("test-suite")]
        public ResultTestSuite testsuite { get; set; }
    }
    public class ResultTestSuite
    {
        [XmlAttribute]
        public string result { get; set; }

        [XmlAttribute]
        public string fullname { get; set; }
        [XmlAttribute]
        public string testcasecount { get; set; }
        [XmlAttribute]
        public string total { get; set; }
        [XmlAttribute]
        public string passed { get; set; }
        [XmlAttribute]
        public string failed { get; set; }
        [XmlAttribute]
        public string inconclusive { get; set; }
        [XmlAttribute]
        public string skipped { get; set; }

    }

    public class TestResult
    {
        public TestRun testrun { get; set; }
        public TestResult(string resultPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TestRun));
            StreamReader reader = new StreamReader(resultPath);
            testrun = new TestRun();
            XmlSerializer xml = new XmlSerializer(typeof(TestRun));
            testrun = (TestRun)xml.Deserialize(reader);
            reader.Close();
        }
    }
}
