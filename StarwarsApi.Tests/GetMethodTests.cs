using System;
using System.Threading.Tasks;
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
        public async Task VerifyIfPersonHomeWorldName_ReturnsExpectedPlanetName(string personName, string expectedPlanetName)
        {
            Person person = await GetPersonByName(_client, personName);
            Planet planet = await GetPersonHomePlanet(_client, person);

            Assert.AreEqual(expectedPlanetName, planet.Name);
        }

        private async Task<Person> GetPersonByName(RestClient client, string personName)
        {
            RestRequest request = new RestRequest("people/?search=" + personName, Method.GET);
            IRestResponse response = await _client.ExecuteAsync(request);
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

        private async Task<Planet> GetPersonHomePlanet(RestClient client, Person person)
        {
            RestRequest request = new RestRequest(person.HomeWorld, Method.GET);
            IRestResponse response = await _client.ExecuteAsync(request);
            
            Planet planet = new JsonDeserializer().Deserialize<Planet>(response);

            return planet;
        }
    }
}
