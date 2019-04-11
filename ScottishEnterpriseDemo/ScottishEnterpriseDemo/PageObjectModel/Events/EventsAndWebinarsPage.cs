using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PageObjectModel
{
    public class EventsAndWebinarsPage
    {
        private readonly IWebDriver driver;

        public EventsAndWebinarsPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void SelectEventLocation(string eventLocation)
        {
            SelectElement eventLocationsCombo = GetEventLocationCombo();
            eventLocationsCombo.SelectByText(eventLocation);
        }

        public List<string> GetAllEventLocations()
        {
            List<string> eventLocations = new List<string>();
            SelectElement selectEventsCombo = GetEventLocationCombo();
            IList<IWebElement> options = selectEventsCombo.Options;
            foreach (IWebElement option in options)
            {
                eventLocations.Add(option.Text);
            }
            return eventLocations;
        }

        public int GetNumberOfEventsForLocation()
        {
            //UGLY UGLY UGLY
            //An Ajax event is firing and there is nothing on the page that 
            //we can use to check that it's finished. 
            //So we are left with this horror
            Thread.Sleep(2000);
            try
            {
                IWebElement dataEventsFound = driver.FindElement(By.XPath(@"//div[@class='events-found']"));
                return int.Parse(dataEventsFound.GetAttribute("data-events-shown"));
            }
            catch (NoSuchElementException noSuchExcp)
            {
                //This is o.k to catch becuase sometimes an location will have no events
                return 0;
            }
        }

        public bool NoEventsMessageIsVisible()
        {
            return GetNoEventsListedMesssage().Displayed;
        }

        public string GetNoEventsMessageText()
        {
            return GetNoEventsListedMesssage().Text;
        }

        protected IWebElement GetNoEventsListedMesssage()
        {
            IWebElement noEventListedMessage = driver.FindElement(By.XPath(@"//p[text()='Sorry, there are no events listed.']"));
            return noEventListedMessage;
        }

        protected SelectElement GetEventLocationCombo()
        {
            SelectElement eventLocationsCombo = new SelectElement(driver.FindElement(By.Id("select-box__event-location")));
            return eventLocationsCombo;
        }
    }
}
