﻿using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using EcommerceAppTestingFramework.Drivers;
using NUnit.Framework.Interfaces;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Reflection;

namespace EcommerceAppTestingFramework.Reports
{
    public class ExtentReporting
    {
        private ExtentReports _extentReports;
        private ExtentTest _extentTest;
        private IDriverActions _driver;

        public ExtentReporting(IDriverActions driver)
        {
            _driver = driver;
            _extentReports = StartReporting();
        }

        private ExtentReports StartReporting()
        {
            var extentReportPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/results/extentreport.html";
            //Console.WriteLine("path: " + extentReportPath);

            var extentReports = new ExtentReports();
            var spark = new ExtentSparkReporter(extentReportPath);
            extentReports.AttachReporter(spark);

            return extentReports;
        }

        public void CreateTest(string testName)
        {
            _extentTest = _extentReports.CreateTest(testName);
        }

        public void EndTest()
        {
            var testStatus = TestContext.CurrentContext.Result.Outcome.Status;
            var message = TestContext.CurrentContext.Result.Message;

            if (testStatus == TestStatus.Failed)
            {
                LogFail($"Test failed: {message}");
            }
            else if (testStatus == TestStatus.Passed)
            {
                LogPass($"Test passed man");
            }

            LogScreenshot("Ending test", GetScreenshot());
        }

        public void EndReporting()
        {
            _extentReports.Flush();
        }

        public void LogPass(string info)
        {
            _extentTest.Pass(info);
        }

        public void LogFail(string info)
        {
            _extentTest.Fail(info);
        }

        public void LogInfo(string info)
        {
            _extentTest.Info(info);
        }

        public void LogScreenshot(string info, string image)
        {
            _extentTest.Info(info, MediaEntityBuilder.CreateScreenCaptureFromBase64String(image).Build());
        }

        public string GetScreenshot()
        {
            var file = ((ITakesScreenshot)_driver).GetScreenshot();
            var img = file.AsBase64EncodedString;
            return img;
        }
    }
}
