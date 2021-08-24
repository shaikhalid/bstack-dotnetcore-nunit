using System;
using NUnit.Framework;
using OpenQA.Selenium;

namespace BstackNetCoreNunit
{
    [TestFixture("firefox","single","single_test",".NetCore Nunit")]
    public class SingleTest : BaseTest
    {

        public SingleTest(String platform, String profile, String session_name, String build) : base(platform, profile, session_name, build) { }

        [Test]
        public void SingleTestCase ()
        {
            //driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            driver.Navigate().GoToUrl("https://www.google.com/ncr");

            IWebElement query = driver.FindElement(By.Name("q"));
            query.SendKeys("BrowserStack");
            //query.SendKeys(Keys.Enter);
            query.Submit();
            System.Threading.Thread.Sleep(5000);
            Assert.AreEqual("BrowserStack - Google Search", driver.Title);
            if (driver.Title.Equals("BrowserStack - Google Search"))
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \"Expected\"}}");
            }
            else
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"Unexpected\"}}");
            }
            //System.Threading.Thread.Sleep(5000);

        }
    }
}
