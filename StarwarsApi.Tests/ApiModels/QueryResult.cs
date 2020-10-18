using System.Collections.Generic;

namespace StarWarsApi.Tests.ApiModels
{
    public class QueryResult<T> where T : class
    {
        public int Count { get; set; }

        public string Next { get; set; }

        public string Previous { get; set; }

        public List<T> Results { get; set; }
    }
}
