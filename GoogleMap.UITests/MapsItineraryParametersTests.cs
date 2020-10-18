using GoogleMap.UITests.PageObjectModels;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GoogleMap.UITests
{
    public class MapsItineraryParametersTests
    {
        private readonly IWebDriver _driver;

        public MapsItineraryParametersTests(string browserType = "chrome")
        {
            switch (browserType)
            {
                case "chrome":
                    _driver = new ChromeDriver();
                    break;
                default:
                    _driver = new ChromeDriver();
                    break;
            }
        }

        [Theory]
        [Trait("Category", "Distance")]
        [InlineData("Plac Defilad 1, Warszawa", "Chłodna 51, Warszawa", 3, "Pieszo")]
        [InlineData("Chłodna 51, Warszawa", "Plac Defilad 1, Warszawa", 3, "Pieszo")]
        [InlineData("Plac Defilad 1, Warszawa", "Chłodna 51, Warszawa", 3, "Na rowerze")]
        [InlineData("Chłodna 51, Warszawa", "Plac Defilad 1, Warszawa", 3, "Na rowerze")]
        public void VerifyDistance_ReturnsLessThanGiven(
            string startLocation,
            string endLocation,
            int maxDistanceInKilometers,
            string modeOfTransport)
        {
            using (IWebDriver driver = _driver)
            {
                // Arrange
                var mapsHomePage = new GoogleMapsHomePage(driver);
                mapsHomePage.NavigateTo();
                mapsHomePage.AcceptGooglePrivacyPolicy();

                mapsHomePage.FindLocationOnMap(endLocation);
                mapsHomePage.ClickFindItinerary();
                mapsHomePage.InsertStartLocation(startLocation);
                mapsHomePage.SearchForItinerary();

                // Act
                mapsHomePage.ChooseModeOfTransport(modeOfTransport);
                mapsHomePage.ChooseDistanceUnits("km");
                List<double> proposedDistanceInKilometers = mapsHomePage.FindProposedItineraryDistanceValues();

                // Assert
                Assert.True(proposedDistanceInKilometers.Min() < maxDistanceInKilometers);
            }
        }

        [Theory]
        [Trait("Category", "TravelTime")]
        [InlineData("Plac Defilad 1, Warszawa", "Chłodna 51, Warszawa", 40, "Pieszo")]
        [InlineData("Chłodna 51, Warszawa", "Plac Defilad 1, Warszawa", 40, "Pieszo")]
        [InlineData("Plac Defilad 1, Warszawa", "Chłodna 51, Warszawa", 40, "Na rowerze")]
        [InlineData("Chłodna 51, Warszawa", "Plac Defilad 1, Warszawa", 40, "Na rowerze")]
        public void VerifyTravelTime_ReturnsLessThanGiven(
            string startLocation,
            string endLocation,
            int travelTimeInMinutes,
            string modeOfTransport)
        {
            using (IWebDriver driver = _driver)
            {
                // Arrange
                var mapsHomePage = new GoogleMapsHomePage(driver);
                mapsHomePage.NavigateTo();
                mapsHomePage.AcceptGooglePrivacyPolicy();

                mapsHomePage.FindLocationOnMap(endLocation);
                mapsHomePage.ClickFindItinerary();
                mapsHomePage.InsertStartLocation(startLocation);
                mapsHomePage.SearchForItinerary();

                // Act
                mapsHomePage.ChooseModeOfTransport(modeOfTransport);
                List<int> proposedTravelTime = mapsHomePage.FindProposedItineraryTimeInMinutes();

                // Assert
                Assert.True(proposedTravelTime.Min() < travelTimeInMinutes);
            }
        }
    }
}
