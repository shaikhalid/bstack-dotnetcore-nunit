using System;

using NUnit.Framework;
using OpenQA.Selenium;

namespace BstackNetCoreNunit
{
    [TestFixture("firefox","parallel","paralle_firefox", ".NetCore Nunit")]
    [TestFixture("chrome", "parallel","parallel_chrome", ".NetCore Nunit")]
    [TestFixture("edge", "parallel","parallel_edge", ".NetCore Nunit")]
    [TestFixture("safari", "parallel","parallel_safari", ".NetCore Nunit")]
    [TestFixture("pixel", "parallel","parallel_pixel", ".NetCore Nunit")]
    [TestFixture("iPhone", "parallel","parallel_iPhone", ".NetCore Nunit")]
    [Parallelizable(ParallelScope.Fixtures)]
   
    public class ParallelTest : BaseTest
    {

        public ParallelTest(String platform, String profile, String session_name, String build) : base(platform, profile, session_name, build) { }

        [Test]
        public void ParallelTestCase()
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
