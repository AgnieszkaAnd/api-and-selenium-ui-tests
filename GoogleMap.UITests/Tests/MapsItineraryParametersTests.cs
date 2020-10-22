using GoogleMap.UITests.PageObjectModels;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace GoogleMap.UITests.Tests
{
    [TestFixture]
    public class MapsItineraryParametersTests : TestBase
    {
        [TestCase("Plac Defilad 1, Warszawa", "Chłodna 51, Warszawa", 3, "Pieszo")]
        [TestCase("Chłodna 51, Warszawa", "Plac Defilad 1, Warszawa", 3, "Pieszo")]
        [TestCase("Plac Defilad 1, Warszawa", "Chłodna 51, Warszawa", 3, "Na rowerze")]
        [TestCase("Chłodna 51, Warszawa", "Plac Defilad 1, Warszawa", 3, "Na rowerze")]
        public void VerifyDistance_ReturnsLessThanGiven(
            string startLocation,
            string endLocation,
            int maxDistanceInKilometers,
            string modeOfTransport)
        {
            extent.CreateTest(
                $"{MethodBase.GetCurrentMethod().Name}" +
                $" from {startLocation}" +
                $" to {endLocation}" +
                $" mode of transport: {modeOfTransport}" +
                $" given: {maxDistanceInKilometers}km"
            );

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

        [TestCase("Plac Defilad 1, Warszawa", "Chłodna 51, Warszawa", 40, "Pieszo")]
        [TestCase("Chłodna 51, Warszawa", "Plac Defilad 1, Warszawa", 40, "Pieszo")]
        [TestCase("Plac Defilad 1, Warszawa", "Chłodna 51, Warszawa", 40, "Na rowerze")]
        [TestCase("Chłodna 51, Warszawa", "Plac Defilad 1, Warszawa", 40, "Na rowerze")]
        public void VerifyTravelTime_ReturnsLessThanGiven(
            string startLocation,
            string endLocation,
            int travelTimeInMinutes,
            string modeOfTransport)
        {
            extent.CreateTest(
                $"{MethodBase.GetCurrentMethod().Name}" +
                $" from {startLocation}" +
                $" to {endLocation}" +
                $" mode of transport: {modeOfTransport}" +
                $" given: {travelTimeInMinutes} minutes"
            );

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
