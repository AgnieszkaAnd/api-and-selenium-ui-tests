using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharp.Serialization.Json;
using StarWarsApi.Tests.ApiModels;

namespace StarWarsApi.Tests
{
    [TestClass]
    public class GetMethodTests
    {
        private const string _baseApiUrl = "https://swapi.dev/api";

        private RestClient _client;

        [TestInitialize]
        public void Setup()
        {
            _client = new RestClient(_baseApiUrl);
        }

        [DataRow("Luke Skywalker", "Tatooine")]
        [TestMethod]
        public void VerifyIfPersonHomeWorldName_ReturnsExpectedPlanetName(string personName, string expectedPlanetName)
        {
            Person person = GetPersonByName(_client, personName);
            Planet planet = GetPersonHomePlanet(_client, person);

            Assert.AreEqual(expectedPlanetName, planet.Name);
        }

        private Person GetPersonByName(RestClient client, string personName)
        {
            RestRequest request = new RestRequest("people/?search=" + personName, Method.GET);
            IRestResponse response = _client.Execute(request);
            QueryResult<Person> result = new JsonDeserializer().Deserialize<QueryResult<Person>>(response);
            
            if(result.Results.Count == 1)
            {
                return result.Results[0];
            }
            else
            {
                throw new ArgumentException($"Multiple people found with the given name: {personName}");
            }
        }

        private Planet GetPersonHomePlanet(RestClient client, Person person)
        {
            RestRequest request = new RestRequest(person.HomeWorld, Method.GET);
            IRestResponse response = _client.Execute(request);
            
            Planet planet = new JsonDeserializer().Deserialize<Planet>(response);

            return planet;
        }
    }
}
