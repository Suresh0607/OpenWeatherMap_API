using NUnit.Framework;
using System;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json.Linq;
using AventStack.ExtentReports;
using System.Configuration;
using System.Collections.Specialized;

namespace WeatherAPITesting
{
    [TestFixture]
    public class WhrApiTesting_expV3
    {
        ExtentReports rep = ExtentManager.getInstance();
        ExtentTest test;

        [TestCase("Sydney", 20)]
        [Test]       
        public void WeathAPITest(string City, int temperature)
        {
            test = rep.CreateTest("WeatherForeCast for Next 5 Days with Temp > 20 for the City-" + City);
            var client = new RestClient(ConfigurationManager.AppSettings["WeatherAPI_5dayForcast"] + City + "&APPID=" +ConfigurationManager.AppSettings["WeatherAPI_Key"]);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            
            dynamic api = JObject.Parse(response.Content);
            var categories = api.list;
            Dictionary<string, int> Days_Temp = new Dictionary<string, int>();
            Dictionary<string, int> SummyDays = new Dictionary<string, int>();

            foreach (var item in categories)
            {
                var tempinKelvin = item.main.temp;
                var tempinDegrees = tempinKelvin - 273.15;
                int tempDegree = tempinDegrees;

                if (tempDegree > temperature)
                {
                    string tdate = item.dt_txt;
                    string _Date = tdate.Split(' ')[0];

                    int ttemp = tempinDegrees;
                    string d = Convert.ToDateTime(_Date).ToString("dddd, dd MMMM yyyy");

                    if (!Days_Temp.ContainsKey(d))
                        Days_Temp.Add(d, ttemp);
                }

                int cloud_status = item.clouds.all;
                
                if (cloud_status == 0)
                {
                    string tdate = item.dt_txt;
                    string _Date = tdate.Split(' ')[0];
                                        
                    string d = Convert.ToDateTime(_Date).ToString("dddd, dd MMMM yyyy");

                    if (!SummyDays.ContainsKey(d))
                        SummyDays.Add(d, cloud_status);
                }
            }

            foreach (KeyValuePair<string, int> item in Days_Temp)
            {
               Console.WriteLine("Date:--" + item.Key + "       Temperature:--" + item.Value); 
               test.Log(Status.Info, "Date:--" + item.Key + "       Temperature:--" + item.Value);
            }

            test.Log(Status.Info, "Total Sunny Days :--" + SummyDays.Count);


            rep.Flush();
        }

       
    }
   
}
