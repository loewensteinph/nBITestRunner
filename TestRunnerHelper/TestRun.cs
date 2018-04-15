using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TestrunnerHelper.Config;
using TestrunnerHelper.Log;

namespace TestrunnerHelper
{
    /// <summary>
    /// Enumeration contains possible result states
    /// </summary>
    public enum ResultStatus
    {
        OK,
        INVALID_ARG,
        INVALID_ASSEMBLY,
        FIXTURE_NOT_FOUND,
        INVALID_TEST_FIXTURE,
        UNEXPECTED_ERROR,
        WRAPPER_ERROR,
        TEST_FAILED
    }
    /// <summary>
    /// Contains the TestRunConfig which includes all relevant params
    /// </summary>
    internal static class TestRunConfig
    {
        static TestRunConfig()
        {
            TestProjectList = new List<TestProject>();
        }

        public static string NunitConsoleParams { get; set; }
        public static string NunitBinaryPath { get; set; }
        public static string NunitProjectFile { get; set; }
        public static string NnitConsoleBinaryName { get; set; }

        public static List<TestProject> TestProjectList { get; set; }
    }
    /// <summary>
    /// Reflects a NbiTestRun which can contain multiple projects
    /// and their testsuites.
    /// A run can host multiple nUNIT Console calls
    /// </summary>
    public class NbiTestRun
    {
        private static readonly ILog Log = LogFactory.GetLogger(typeof(NbiTestRun));
        /// <summary>
        /// Extends default constructor with a param to pass a list of tests
        /// which acts as a filter
        /// </summary>
        /// <param name="testsToRun"></param>
        public NbiTestRun(List<string> testsToRun) : this()
        {
            foreach (var testproject in TestRunConfig.TestProjectList)
                foreach (var testsuite in testproject.TestSuites)
                    testsuite.TestsToRun = testsToRun;
        }
        public NbiTestRun()
        {
            try
            {
                Log.Info("Starting Testrun");
                TestRunConfig.TestProjectList = new List<TestProject>();

                var cf = new ConfigurationFinder();
                var result = cf.Find();

                var runConfigElement = result.TestRun;

                TestRunConfig.NunitConsoleParams = result.TestRun.NunitConsoleParams;
                TestRunConfig.NunitBinaryPath = result.TestRun.NunitBinaryPath;
                TestRunConfig.NunitProjectFile = result.TestRun.NunitProjectFile;
                TestRunConfig.NnitConsoleBinaryName = result.TestRun.NnitConsoleBinaryName;

                Log.Info("Params from App.config:");
                Log.Info(String.Format("NunitConsoleParams: {0}", TestRunConfig.NunitConsoleParams));
                Log.Info(String.Format("NunitBinaryPath: {0}", TestRunConfig.NunitBinaryPath));
                Log.Info(String.Format("NunitProjectFile: {0}", TestRunConfig.NunitProjectFile));

                foreach (TestProjectConfigElement tp in result.TestSuites)
                {
                    Log.Info("Interpreting testrun config section in App.config:");
                    Log.Info(String.Format("Found: {0} testsuites", tp.TestSuites.Count));
                    Log.Info("Check existence of configured testsuites...");

                    var testProject = new TestProject(tp.RelativePath, runConfigElement.NunitProjectFile);
                    foreach (TestSuiteConfigElement ts in tp.TestSuites)
                    {
                        var testSuite = new TestSuite(tp.RelativePath,
                            string.IsNullOrEmpty(ts.NBIconfigFile) ? tp.NBIconfigFile : ts.NBIconfigFile);
                        testSuite.TestSuiteName = ts.Name;
                        // Check File existence
                        if (testSuite.CheckFileExists())
                        {
                            Log.Info(String.Format("Testsuites: {0} was found", testSuite.TestSuiteFulllName));
                            testProject.TestSuites.Add(testSuite);
                        }
                        else
                        {
                            Log.Warn(String.Format("Testsuites: {0} was NOT found", testSuite.TestSuiteFulllName));
                            Debug.WriteLine(String.Format(@"TestSuite {0}\{1} not found! Check app.config or make sure testsuite exists! ", testSuite.PathToTestProject, testSuite.TestSuiteFulllName));
                        }
                    }
                    TestRunConfig.TestProjectList.Add(testProject);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                ResultStatus = ResultStatus.WRAPPER_ERROR;
            }
        }

        public ResultStatus ResultStatus { get; set; }

        /// <summary>
        ///     VSTest run as silent process
        /// </summary>
        public List<TestSuite> RunTests()
        {
            var TestSuiteList = new List<TestSuite>();

            foreach (var testProject in TestRunConfig.TestProjectList)
                foreach (var testsuite in testProject.TestSuites)
                {
                    var sb = new StringBuilder();
                    try
                    {
                        sb = new StringBuilder();

                        testsuite.NBiConfigFile.PrepareNunitConfig(testsuite);

                        Log.Info(String.Format("Preparing nUnit config file {0}", testsuite.NBiConfigFile.FullNameNBiConfigFile));
                        testProject.NunitProjectFile.PrepareNunitProject(testsuite.NBiConfigFile.FullNameNBiConfigFile);

                        string nugetPackagePath = TestRunConfig.NunitBinaryPath;

                        string searchPatch = nugetPackagePath.Contains(@"..\")
                            ? Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), nugetPackagePath))
                            : nugetPackagePath;

                        string nunitConsole =
                            Directory.GetFiles(searchPatch, TestRunConfig.NnitConsoleBinaryName, SearchOption.AllDirectories)
                                .FirstOrDefault();

                        Log.Info(String.Format("nUnit Console Binaries {0}", nunitConsole));

                        var cb = new StringBuilder();

                        Log.Info("Building nUnit Console arguments");

                        cb.Append(testProject.NunitProjectFile.FullNameNunitProjectFilename);
                        cb.Append(string.Format(@" --result {0};transform=result.xslt", testsuite.ResultFullFilenameHTML));
                        cb.Append(string.Format(@" --result {0}", testsuite.ResultFullFilenameXML));
                        cb.Append(string.Format(@" {0}", TestRunConfig.NunitConsoleParams));

                        if (testsuite.TestsToRun.Count > 0)
                        {
                            cb.Append(string.Format(@" --test {0}", testsuite.TestsToRunConcat));
                            Log.Info(String.Format("Filter was passed containing {0} tests", testsuite.TestsToRun.Count));
                        }

                        var cmdparams = cb.ToString();

                        Log.Info(String.Format("Final Console arguments {0}", cmdparams));

                        if (nunitConsole != null)
                        {
                            var process = new Process
                            {
                                StartInfo =
                            {
                                FileName = nunitConsole,
                                Arguments = cmdparams,
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true
                            }
                            };

                            process.ErrorDataReceived += (sendingProcess, errorLine) => sb.AppendLine(errorLine.Data);
                            process.OutputDataReceived += (sendingProcess, dataLine) => sb.AppendLine(dataLine.Data);

                            Log.Info("Starting nUnit console process...");

                            process.Start();
                            process.BeginErrorReadLine();
                            process.BeginOutputReadLine();

                            process.WaitForExit();

                            Console.WriteLine(sb.ToString());

                            var exitCode = process.ExitCode;

                            Log.Info(String.Format("nUnit console process exited with code {0}", exitCode));

                            Log.Info("Deserialize nUnit result.xml file.");
                            TestResult tr = new TestResult(testsuite.ResultFullFilenameXML);

                            switch (exitCode)
                            {
                                case 0:
                                    ResultStatus = ResultStatus.OK;
                                    testsuite.ResultStatus = ResultStatus.OK;
                                    break;
                                case -1:
                                    ResultStatus = ResultStatus.INVALID_ARG;
                                    testsuite.ResultStatus = ResultStatus.INVALID_ARG;
                                    break;
                                case -2:
                                    ResultStatus = ResultStatus.INVALID_ASSEMBLY;
                                    testsuite.ResultStatus = ResultStatus.INVALID_ASSEMBLY;
                                    break;
                                case -3:
                                    ResultStatus = ResultStatus.FIXTURE_NOT_FOUND;
                                    testsuite.ResultStatus = ResultStatus.FIXTURE_NOT_FOUND;
                                    break;
                                case -4:
                                    ResultStatus = ResultStatus.INVALID_TEST_FIXTURE;
                                    testsuite.ResultStatus = ResultStatus.INVALID_TEST_FIXTURE;
                                    break;
                                default:
                                    if (exitCode > 0)
                                        ResultStatus = ResultStatus.TEST_FAILED;
                                    testsuite.ResultStatus = ResultStatus.TEST_FAILED;
                                    break;
                            }

                            if (tr.testrun.testsuite.result.Equals("Inconclusive"))
                            {
                                ResultStatus = ResultStatus.INVALID_TEST_FIXTURE;
                                testsuite.ResultStatus = ResultStatus.INVALID_TEST_FIXTURE;
                            }
                            TestSuiteList.Add(testsuite);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(String.Format("Error occured during testrun {0}.", ex.Message));
                        Debug.WriteLine(ex.Message);
                        sb.Append(ex.Message);
                        ResultStatus = ResultStatus.WRAPPER_ERROR;
                        testsuite.ResultStatus = ResultStatus.WRAPPER_ERROR;
                    }
                }
            return TestSuiteList;
        }
    }
}