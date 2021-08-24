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

    public class BaseTest
    {
        String username;
        String accessKey;
        public IWebDriver driver;
        String appSettingsPath = "/Users/nithyamani/Projects/BstackNetCoreNunit/BstackNetCoreNunit/";

        public String platform;
        public String profile;
        public String session_name;
        public String build;
        public BaseTest(String platform, String profile, String session_name, String build)
        {
            this.platform = platform;
            this.profile = profile;
            this.session_name = session_name;
            this.build = build;
        }

        [SetUp]
        public void SetupDriver()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(appSettingsPath+"appSettings.json", false);
            var configuration = builder.Build();


            var credentials = new Credentials();
            var credentialsSection = configuration.GetSection("credentials");
            credentials = credentialsSection.Get<Credentials>();

            var platforms = new Platform();
            var platformSection = configuration.GetSection("platforms").GetSection(platform);
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
            capability.AddAdditionalCapability("build", build, true); // test name
            capability.AddAdditionalCapability("name", session_name, true); // CI/CD job or build name
            capability.AddAdditionalCapability("browserstack.user", username, true);
            capability.AddAdditionalCapability("browserstack.key", accessKey, true);
            //add more caps 
            capability.AddAdditionalCapability("browserstack.debug", "true", true);
            capability.AddAdditionalCapability("browserstack.console", "verbose", true);

            if (profile.Equals("local")){
                capability.AddAdditionalCapability("browserstack.local", "true", true);
            }

            driver = new RemoteWebDriver(
              new Uri("https://hub-cloud.browserstack.com/wd/hub/"), capability
            );
        }
        [TearDown]
        public void TearDownDriver()
        {
            driver.Quit();
        }
    }
}