using System.Text.Json.Serialization;

namespace LP.AnimalShelter.API.Models
{
    public class Animal
    {
        public string Type { get; set; }

        public string Name { get; set; }

        [JsonPropertyName("size-in-lbs")]
        public decimal SizeInPounds { get; set; }
    }
}
