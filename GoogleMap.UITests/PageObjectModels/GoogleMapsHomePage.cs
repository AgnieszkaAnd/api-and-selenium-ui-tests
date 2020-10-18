using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace GoogleMap.UITests.PageObjectModels
{
    public class GoogleMapsHomePage
    {
        private const string _pageUrl = "https://www.google.pl/maps/";

        private const int _standardTimeoutInSeconds = 60;

        private readonly IWebDriver _driver;

        public GoogleMapsHomePage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void NavigateTo()
        {
            _driver.Navigate().GoToUrl(_pageUrl);
            _driver.Manage().Window.Maximize();
        }

        public void AcceptGooglePrivacyPolicy()
        {
            var consentFrame =
                (new WebDriverWait(_driver, TimeSpan.FromSeconds(_standardTimeoutInSeconds)))
                .Until(d => d.SwitchTo().Frame(d.FindElement(By.ClassName("widget-consent-frame"))));
            IWebElement agreeButton =
                (new WebDriverWait(consentFrame, TimeSpan.FromSeconds(_standardTimeoutInSeconds)))
                .Until(d => d.FindElement(By.Id("introAgreeButton")));
            agreeButton.Click();
        }

        public void InputSearchLocationText(string location)
        {
            var inputEndLocation =
                (new WebDriverWait(_driver, TimeSpan.FromSeconds(_standardTimeoutInSeconds)))
                .Until(d => d.FindElement(By.Id("searchboxinput")));
            inputEndLocation.SendKeys(location);
        }

        public void ClickSearchForLocation() => _driver.FindElement(By.Id("searchbox-searchbutton")).Click();

        public void FindLocationOnMap(string location)
        {
            InputSearchLocationText(location);
            ClickSearchForLocation();
        }

        public void ClickFindItinerary()
        {
            var findDirections =
                (new WebDriverWait(_driver, TimeSpan.FromSeconds(_standardTimeoutInSeconds)))
                .Until(d => d.FindElement(By.XPath("//button[@data-value='Wyznacz trasę']")));
            findDirections.Click();
        }

        public void InsertStartLocation(string startLocation)
        {
            var startLocationInput =
                (new WebDriverWait(_driver, TimeSpan.FromSeconds(_standardTimeoutInSeconds)))
                .Until(d => d.FindElement(By.Id("directions-searchbox-0")));
            startLocationInput
                .FindElement(By.TagName("input"))
                .Clear();
            startLocationInput
                .FindElement(By.TagName("input"))
                .SendKeys(startLocation);
        }

        public void SearchForItinerary()
        {
            ClickEnterInStartItineraryInput();
        }

        public void ClickEnterInStartItineraryInput()
        {
            var startLocationInput =
                (new WebDriverWait(_driver, TimeSpan.FromSeconds(_standardTimeoutInSeconds)))
                .Until(d => d.FindElement(By.Id("directions-searchbox-0")));
            startLocationInput
                .FindElement(By.TagName("input"))
                .SendKeys(Keys.Enter);
        }

        public void ChooseModeOfTransport(string modeOfTransportName)
        {
            string selector = "//img[@aria-label='" + modeOfTransportName + "']";

            var elementEnabled =
                (new WebDriverWait(_driver, TimeSpan.FromSeconds(_standardTimeoutInSeconds)))
                .Until(d => d.FindElement(By.XPath(selector)).Enabled);
            var element =
                (new WebDriverWait(_driver, TimeSpan.FromSeconds(_standardTimeoutInSeconds)))
                .Until(d => d.FindElement(By.XPath(selector)));
            element.Click();
        }

        public void ChooseDistanceUnits(string unit)
        {
            if (unit == "km" || unit == "miles" || unit == "auto")
            {
                OpenItineraryOptions();

                try
                {
                    ClickDistanceUnitRadioButton(unit);
                }
                catch
                {
                    OpenItineraryOptions();
                    ClickDistanceUnitRadioButton(unit);
                }

                CloseItineraryOptions();

            } else
            {
                throw new ArgumentException("Provided distance units are not available");
            }
        }

        public void OpenItineraryOptions()
        {
            new WebDriverWait(_driver, TimeSpan.FromSeconds(_standardTimeoutInSeconds))
                .Until(d => d.FindElement(By.XPath("//button/span[text()='Opcje']")).Displayed);

            var optionsElementEnabled =
                (new WebDriverWait(_driver, TimeSpan.FromSeconds(_standardTimeoutInSeconds)))
                .Until(d => d.FindElement(By.XPath("//span[text()='Opcje']")).Enabled);
            var optionsElement = (new WebDriverWait(_driver, TimeSpan.FromSeconds(_standardTimeoutInSeconds)))
                .Until(d => d.FindElement(By.XPath("//span[text()='Opcje']")));
            optionsElement.Click();
        }

        public void CloseItineraryOptions()
        {
            var optionsElementCloseEnabled =
                (new WebDriverWait(_driver, TimeSpan.FromSeconds(_standardTimeoutInSeconds)))
                .Until(d => d.FindElement(By.XPath("//button/span[text()='Zamknij']")).Enabled);
            var optionsElementClose = _driver.FindElement(By.XPath("//button/span[text()='Zamknij']"));
            optionsElementClose.Click();
        }

        public void ClickDistanceUnitRadioButton(string unit)
        {
            string selector = "//label[@for='pane.directions-options-units-" + unit + "']";

            var radioButtonToClick =
                (new WebDriverWait(_driver, TimeSpan.FromSeconds(5)))
                .Until(d => d.FindElement(By.XPath(selector)));
            radioButtonToClick.Click();
        }

        public List<double> FindProposedItineraryDistanceValues()
        {
            List<double> proposedDistanceList = new List<double>();

            int itinerariesCount = FindProposedItinerariesCount();

            for (int i = 0; i < itinerariesCount; i++)
            {
                string sectionNumber = i.ToString();
                string section = "section-directions-trip-" + sectionNumber;

                var itinerarySection =
                    (new WebDriverWait(_driver, TimeSpan.FromSeconds(_standardTimeoutInSeconds)))
                    .Until(d => d.FindElement(By.Id(section)));
                string distanceInKilometers = itinerarySection
                    .FindElement(By.XPath("//div[contains(@class, 'section-directions-trip-distance') and contains(@class, 'section-directions-trip-secondary-text')]")).Text;

                proposedDistanceList.Add(Convert.ToDouble(distanceInKilometers.Split(' ')[0]));
            }

            return proposedDistanceList;
        }

        public List<int> FindProposedItineraryTimeInMinutes()
        {
            List<int> proposedTimeList = new List<int>();

            int itinerariesCount = FindProposedItinerariesCount();

            for (int i = 0; i < itinerariesCount; i++)
            {
                string sectionNumber = i.ToString();
                string section = "section-directions-trip-" + sectionNumber;

                var itinerarySection =
                    (new WebDriverWait(_driver, TimeSpan.FromSeconds(_standardTimeoutInSeconds)))
                    .Until(d => d.FindElement(By.Id(section)));
                string time = itinerarySection.FindElement(By.ClassName("section-directions-trip-duration")).Text;

                if (!time.Contains("godz."))
                {
                    bool minutesParsing = Int32.TryParse(time.Split(' ')[0], out int minutes);
                    proposedTimeList.Add(minutes);
                }
                else
                {
                    string[] arrayOfTimeData = time.Split(' ');
                    Int32.TryParse(arrayOfTimeData[0], out int hours);
                    Int32.TryParse(arrayOfTimeData[2], out int minutes);

                    proposedTimeList.Add(hours*60 + minutes);
                }
            }

            return proposedTimeList;
        }

        public int FindProposedItinerariesCount()
        {
            var proposedItinerariesElementAreVisible =
                (new WebDriverWait(_driver, TimeSpan.FromSeconds(_standardTimeoutInSeconds)))
                .Until(d => d.FindElement(By.XPath("//div[contains(@id, 'section-directions-trip-')]")).Displayed);

            var proposedItinerariesElements =
                (new WebDriverWait(_driver, TimeSpan.FromSeconds(_standardTimeoutInSeconds)))
                .Until(d => d.FindElements(By.XPath("//div[contains(@id, 'section-directions-trip-')]")));

            return proposedItinerariesElements.Count;
        }
    }
}
