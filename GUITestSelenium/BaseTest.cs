using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace GUITestSelenium
{
    public class BaseTest : IDisposable
    {
        protected IWebDriver Driver;
        protected string BaseUrl = "https://localhost:7062"; 

        public BaseTest()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--ignore-certificate-errors"); 
            Driver = new ChromeDriver(options);

            Driver.Navigate().GoToUrl(BaseUrl);
        }

        public void Dispose()
        {
            Driver.Quit();
        }
    }
}
