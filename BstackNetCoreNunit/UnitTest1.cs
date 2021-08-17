using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace BstackNetCoreNunit
{
    public class Credentials
    {
        public string Username { get; set; }
        public string AccessKey { get; set; }
    }
    public class Platform
    {
        public string OS { get; set; }
        public string OS_Version { get; set; }
        public string Browser { get; set; }
        public string Browser_Version { get; set; }
        public string Device { get; set; }
    }

    public class Tests
    {
        String username;
        String accessKey;
        String appSettingsPath = "/Users/nithyamani/Projects/BstackNetCoreNunit/BstackNetCoreNunit/";

        [Test]
        [TestCase("chrome")]
        [TestCase("firefox")]
        [TestCase("edge")]
        [TestCase("safari")]
        [TestCase("pixel")]
        [TestCase("iPhone")]
        [Parallelizable(ParallelScope.All)]
        public void Test1(String platform)
        {
            IWebDriver driver;
          
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(appSettingsPath+"appSettings.json", false);
            var configuration = builder.Build();


            var credentials = new Credentials();
            var platforms = new Platform();

            var credentialsSection = configuration.GetSection("credentials");
            var platformSection = configuration.GetSection("platforms").GetSection(platform);

            credentials = credentialsSection.Get<Credentials>();
            platforms = platformSection.Get<Platform>();

            //Console.WriteLine(credentialsClass.Username + " Accesskey:" + credentialsClass.AccessKey);
            //Console.WriteLine("Browser: " + platformClass.Browser + " BrowserVersion: " + platformClass.Browser_Version + " OS: " + platformClass.OS + " OS Version: " + platformClass.OS_Version+" Device: "+platformClass.Device);

            username = credentials.Username;
            accessKey = credentials.AccessKey;
            

            OpenQA.Selenium.Chrome.ChromeOptions capability = new OpenQA.Selenium.Chrome.ChromeOptions();
            capability.AddAdditionalCapability("os_version", platforms.OS_Version, true);
            capability.AddAdditionalCapability("browser", platforms.Browser, true);
            capability.AddAdditionalCapability("browser_version", platforms.Browser_Version, true);
            capability.AddAdditionalCapability("os", platforms.OS, true);
            capability.AddAdditionalCapability("device", platforms.Device, true);
            capability.AddAdditionalCapability("build", ".NetCore Nunit", true); // test name
            capability.AddAdditionalCapability("name", "with_app_settings", true); // CI/CD job or build name
            capability.AddAdditionalCapability("browserstack.user", username, true);
            capability.AddAdditionalCapability("browserstack.key", accessKey, true);

            driver = new RemoteWebDriver(
              new Uri("https://hub-cloud.browserstack.com/wd/hub/"), capability
            );
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            driver.Navigate().GoToUrl("https://www.google.com");
            Console.WriteLine(driver.Title);
            IWebElement query = driver.FindElement(By.Name("q"));
            query.SendKeys("BrowserStack");
            query.Submit();
            Thread.Sleep(10);
            String title = driver.Title;
            Console.WriteLine(title);
            // Setting the status of test as 'passed' or 'failed' based on the condition; if title of the web page starts with 'BrowserStack'
            if (title.Contains("Google"))
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \" Title matched!\"}}");
            }
            else
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \" Title not matched \"}}");
            }
            driver.Quit();
        }
    }
}