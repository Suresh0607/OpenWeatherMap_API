using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAPITesting
{
    public class ExtentManager
    {
        public static ExtentHtmlReporter htmlReporter;

        private static ExtentReports extent;

        private ExtentManager()
        {

        }

        public static ExtentReports getInstance()
        {
            if (extent == null)
            {
                string reportFile = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_") + ".html";
                htmlReporter = new ExtentHtmlReporter(@"C:\WeatherApp_APITesting\TestResults\"+"Report"+ reportFile);
                extent = new ExtentReports();
                extent.AttachReporter(htmlReporter);
                htmlReporter.LoadConfig(@"C:\WeatherApp_APITesting\WeatherAPITesting\extent-config.xml");
            }
            return extent;
        }
    }
}
