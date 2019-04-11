using System;
using System.Text;
using OpenQA.Selenium;

namespace ScottishEnterpriseDemo.PageObjectModel.Events
{
    public class EventInfoPage
    {
        private IWebDriver driver;
        private string eventcontainerXpath = "(//*[@class='cards__container'])[{eventNumber}]";

        public EventInfoPage(IWebDriver driver, int eventNumber)
        {
            this.driver = driver;
            eventcontainerXpath = eventcontainerXpath.Replace("{eventNumber}", eventNumber.ToString());
        }

        public string GetEventDate()
        {
            StringBuilder xpathBuilder = new StringBuilder();
            xpathBuilder.Append(eventcontainerXpath);
            xpathBuilder.Append(@"//div[@class='event-info__section'][1]/p[@class='event-info__text']");
            return driver.FindElement(By.XPath(xpathBuilder.ToString())).Text;
        }

        public string GetEventAddress()
        {
            StringBuilder xpathBuilder = new StringBuilder();
            xpathBuilder.Append(eventcontainerXpath);
            xpathBuilder.Append(@"//div[@class='event-info__section'][2]/p[@class='event-info__text']");
            return driver.FindElement(By.XPath(xpathBuilder.ToString())).Text;
        }

        public string GetEventTitle()
        {
            StringBuilder xpathBuilder = new StringBuilder();
            xpathBuilder.Append(eventcontainerXpath);
            xpathBuilder.Append(@"//h2[@class='cards__title']");
            return driver.FindElement(By.XPath(xpathBuilder.ToString())).Text;
        }

        public string GetFindOutMore()
        {
            StringBuilder xpathBuilder = new StringBuilder();
            xpathBuilder.Append(eventcontainerXpath);
            xpathBuilder.Append(@"//span[@class='sr-only']");
            return driver.FindElement(By.XPath(xpathBuilder.ToString())).Text;
        }
    }
}
