using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using PageObjectModel;
using System.Linq;
using OpenQA.Selenium.Chrome;
using ScottishEnterpriseDemo.PageObjectModel.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PageTests
{
    [TestClass]
    public class EventAndWebinarTests
    {
        IWebDriver driver;

        EventsAndWebinarsPage eventsAndWebinarsPage;

        List<string> knownEventLocations = new List<string>
            {
            "All locations",
            "Aberdeen",
            "Alloa",
            "Bellshill",
            "Brussels",
            "Carnoustie",
            "Dumfries",
            "Dundee",
            "Edinburgh",
            "Glasgow",
            "Glenrothes",
            "Greenock",
            "Inverness",
            "Lerwick",
            "Orkney",
            "Perth",
            "Prestwick",
            "Selkirk",
            "Stirling"
            };

        [TestInitialize]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            driver.Navigate().GoToUrl(@"https://www.scottish-enterprise.com/learning-zone/events-and-webinars");
            eventsAndWebinarsPage = new EventsAndWebinarsPage(driver);
        }

       
        [TestMethod]
        public void CheckEventLocationsComboBoxPopulatedWithExpectedValues()
        {
            List<string> actualEventLocations = eventsAndWebinarsPage.GetAllEventLocations();

            //Check what we expect is in the list is present in the actual returned result
            IEnumerable<string> missingExpectedLocations = knownEventLocations.Except(actualEventLocations);
            Assert.IsFalse(missingExpectedLocations.Any(), "The Following Expected Event Locations were missing " + String.Join(",", missingExpectedLocations.ToArray()));

            //Check we do not have extras in the returned list that we weren't expecting
            IEnumerable<string> extraLocationsNotInTest = actualEventLocations.Except(knownEventLocations);
            Assert.IsFalse(extraLocationsNotInTest.Any(), "The Following Actual Event Locations were returned that were not expected " + String.Join(",", extraLocationsNotInTest.ToArray()));
        }

        [TestMethod]
        public void LocationsWithNoScheduledEventsDisplayAHelpfulMessage()
        {
            foreach (String location in knownEventLocations)
            {
                eventsAndWebinarsPage.SelectEventLocation(location);
                int numberOfEvents = eventsAndWebinarsPage.GetNumberOfEventsForLocation();
                if (numberOfEvents == 0)
                {
                    string expectedNoEventLocationsMessage = "Sorry, there are no events listed.";
                    string actualNoEventLocationsMessage = eventsAndWebinarsPage.GetNoEventsMessageText();
                    Assert.IsTrue(eventsAndWebinarsPage.NoEventsMessageIsVisible());
                    Assert.AreEqual(expectedNoEventLocationsMessage, actualNoEventLocationsMessage);
                }
            }
        }


        [TestMethod]
        public void LocationsWhichHaveScheduledEventsDisplayUsefulInformation()
        {
            foreach (String location in knownEventLocations)
            {
                eventsAndWebinarsPage.SelectEventLocation(location);
                int totalNumberOfEvents = eventsAndWebinarsPage.GetNumberOfEventsForLocation();

                for (int eventNumber = 1; eventNumber <= totalNumberOfEvents; eventNumber++)
                {
                    System.Diagnostics.Debug.WriteLine("Found " + totalNumberOfEvents + " for " + location);
                    EventInfoPage eventInfoPage = new EventInfoPage(driver, eventNumber);

                    string eventAddress = eventInfoPage.GetEventAddress();
                    string eventDate = eventInfoPage.GetEventDate();
                    string eventTitle = eventInfoPage.GetEventTitle();
                    string findOutMore = eventInfoPage.GetFindOutMore();

                    System.Diagnostics.Debug.WriteLine("Event number: " + eventNumber +
                        " eventAddress: " + eventAddress +
                        " eventDate: " + eventDate +
                        " eventTitle: " + eventTitle +
                        " findOutMore: " + findOutMore
                        );
                 
                    Assert.IsNotNull(eventAddress);
                    Assert.IsNotNull(eventDate);
                    Assert.IsNotNull(eventTitle);
                    Assert.IsNotNull(findOutMore);
                }
            }
        }


        [TestCleanup]
        public void Teardown()
        {
            driver?.Close();
            driver = null;
        }

        // finalizer in case tear down not got chance to execute
        // happens if set up does not complete 
        ~EventAndWebinarTests()
        {
            driver?.Close();
        }
    }
}