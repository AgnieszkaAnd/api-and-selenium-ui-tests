using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StarWarsApi.Tests.ApiModels
{
    public class Planet
    {
        public string Name { get; set; }

        [JsonProperty("rotation_period")]
        public int RotationPeriod { get; set; }

        [JsonProperty("orbital_period")]
        public int OrbitalPeriod { get; set; }

        public int Diameter { get; set; }

        public string Climate { get; set; }

        public string Gravity { get; set; }

        public string Terrain { get; set; }

        [JsonProperty("surface_water")]
        public int SurfaceWater { get; set; }

        public int Population { get; set; }

        public List<string> Residents { get; set; }

        public List<string> Films { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime Edited { get; set; }
        
        public string Url { get; set; }
    }
}
