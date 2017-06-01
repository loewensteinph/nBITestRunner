using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TestrunnerHelper.Config;

namespace TestrunnerHelper
{
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

    internal static class TestRunConfig
    {
        static TestRunConfig()
        {
            TestProjectList = new List<TestProject>();
        }

        public static string NunitConsoleParams { get; set; }
        public static string NunitBinaryPath { get; set; }
        public static string NunitProjectFile { get; set; }

        public static List<TestProject> TestProjectList { get; set; }
    }

    public class TestRun
    {
        public TestRun(List<string> testsToRun) : this()
        {
            foreach (var testproject in TestRunConfig.TestProjectList)
            foreach (var testsuite in testproject.TestSuites)
                testsuite.TestsToRun = testsToRun;
        }

        public TestRun()
        {
            try
            {
                var cf = new ConfigurationFinder();
                var result = cf.Find();

                var runConfigElement = result.TestRun;

                TestRunConfig.NunitConsoleParams = result.TestRun.NunitConsoleParams;
                TestRunConfig.NunitBinaryPath = result.TestRun.NunitBinaryPath;
                TestRunConfig.NunitProjectFile = result.TestRun.NunitProjectFile;

                foreach (TestProjectConfigElement tp in result.TestSuites)
                {
                    var testProject = new TestProject(tp.RelativePath, runConfigElement.NunitProjectFile);
                    foreach (TestSuiteConfigElement ts in tp.TestSuiteses)
                    {
                        var testSuite = new TestSuite(tp.RelativePath,
                            string.IsNullOrEmpty(ts.NBIconfigFile) ? tp.NBIconfigFile : ts.NBIconfigFile);
                        testSuite.TestSuiteName = ts.Name;
                        testProject.TestSuites.Add(testSuite);
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
        ///     VSTest Ausführung: Silent Prozess
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

                    testProject.NunitProjectFile.PrepareNunitProject(testsuite.NBiConfigFile.FullNameNBiConfigFile);


                    var nugetPackagePath = TestRunConfig.NunitBinaryPath;
                    var searchPatch = nugetPackagePath.Contains(@"..\")
                        ? Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), nugetPackagePath))
                        : nugetPackagePath;
                    var nunitConsole =
                        Directory.GetFiles(searchPatch, "nunit3-console.exe", SearchOption.AllDirectories)
                            .FirstOrDefault();

                    var cb = new StringBuilder();

                    cb.Append(testProject.NunitProjectFile.FullNameNunitProjectFilename);
                    cb.Append(string.Format(@" --result {0};transform=result.xslt", testsuite.ResultFilenameHTML));
                    cb.Append(string.Format(@" --result {0}", testsuite.ResultFilenameXML));
                    cb.Append(string.Format(@" {0}", TestRunConfig.NunitConsoleParams));

                    if (testsuite.TestsToRun.Count > 0)
                        cb.Append(string.Format(@" --test {0}", testsuite.TestsToRunConcat));

                    var cmdparams = cb.ToString();

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

                        process.Start();
                        process.BeginErrorReadLine();
                        process.BeginOutputReadLine();

                        process.WaitForExit();

                        Console.WriteLine(sb.ToString());

                        var exitCode = process.ExitCode;

                        switch (exitCode)
                        {
                            case 0:
                                ResultStatus = ResultStatus.OK;
                                break;
                            case -1:
                                ResultStatus = ResultStatus.INVALID_ARG;
                                break;
                            case -2:
                                ResultStatus = ResultStatus.INVALID_ASSEMBLY;
                                break;
                            case -3:
                                ResultStatus = ResultStatus.FIXTURE_NOT_FOUND;
                                break;
                            case -4:
                                ResultStatus = ResultStatus.INVALID_TEST_FIXTURE;
                                break;
                            default:
                                if (exitCode > 0)
                                    ResultStatus = ResultStatus.TEST_FAILED;
                                break;
                        }

                        TestSuiteList.Add(testsuite);
                    }
                }
                catch (Exception ex)
                {
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