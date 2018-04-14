using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestrunnerHelper;

namespace NBITestVSTest
{
    [TestClass]
    public class MyFirstTestSuite
    {
        private List<TestSuite> _testsuites = new List<TestSuite>();

        public TestContext TestContext { get; set; }

        [TestCleanup]
        public void TestCleanup()
        {
            foreach (var result in _testsuites)
            {
                TestContext.WriteLine("Result: " + result.TestSuiteName);
                TestContext.AddResultFile(result.ResultFilenameHTML);
                TestContext.AddResultFile(result.ResultFilenameXML);
            }
        }

        [TestMethod]
        [TestCategory("DBTests")]
        public void TestnameFilterDemo()
        {
            var testsToRun = new List<string> {"CSVTest"};
            var testrun = new NbiTestRun(testsToRun);
            _testsuites = testrun.RunTests();
            Assert.AreEqual(ResultStatus.OK, testrun.ResultStatus);

            foreach (var result in _testsuites)
                Assert.AreEqual(ResultStatus.OK, result.ResultStatus);
        }
        [TestMethod]
        [TestCategory("SingleTest")]
        public void SingleTest()
        {
            var testsToRun = new List<string> { "SingleTest" };
            var testrun = new NbiTestRun(testsToRun);
            _testsuites = testrun.RunTests();
            Assert.AreEqual(ResultStatus.OK, testrun.ResultStatus);

            foreach (var result in _testsuites)
                Assert.AreEqual(ResultStatus.OK, result.ResultStatus);
        }
        [TestMethod]
        [TestCategory("SingleTest")]
        public void SingleTest2()
        {
            var testsToRun = new List<string> { "SingleTest2" };
            var testrun = new NbiTestRun(testsToRun);
            _testsuites = testrun.RunTests();
            Assert.AreEqual(ResultStatus.OK, testrun.ResultStatus);

            foreach (var result in _testsuites)
                Assert.AreEqual(ResultStatus.OK, result.ResultStatus);
        }
        [TestMethod]
        [TestCategory("SingleTest")]
        public void SingleTest3()
        {
            var testsToRun = new List<string> { "SingleTest3" };
            var testrun = new NbiTestRun(testsToRun);
            _testsuites = testrun.RunTests();
            Assert.AreEqual(ResultStatus.OK, testrun.ResultStatus);

            foreach (var result in _testsuites)
                Assert.AreEqual(ResultStatus.OK, result.ResultStatus);
        }
        [TestMethod]
        [TestCategory("SingleTest")]
        public void SingleTestWrongFilter()
        {
            var testsToRun = new List<string> { "SingleTestXXX3" };
            var testrun = new NbiTestRun(testsToRun);
            _testsuites = testrun.RunTests();
            Assert.AreEqual(ResultStatus.INVALID_TEST_FIXTURE, testrun.ResultStatus);

            foreach (var result in _testsuites)
                Assert.AreEqual(ResultStatus.INVALID_TEST_FIXTURE, result.ResultStatus);
        }

        [TestMethod]
        [TestCategory("DBTests")]
        public void RunAllTestDemo()
        {
            var testrun = new NbiTestRun();
            _testsuites = testrun.RunTests();
            Assert.AreEqual(ResultStatus.OK, testrun.ResultStatus);

            foreach (var result in _testsuites)
                Assert.AreEqual(ResultStatus.OK, result.ResultStatus);
        }
    }
}