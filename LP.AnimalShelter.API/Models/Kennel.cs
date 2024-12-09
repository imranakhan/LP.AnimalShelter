using LP.AnimalShelter.API.Enums;
using System.Text.Json.Serialization;

namespace LP.AnimalShelter.API.Models
{
    /// <summary>
    /// Kennel class that has a specific kennel size type and can hold an animal
    /// </summary>
    public class Kennel
    {
        public KennelType Type { get; set; }

        public Animal? Animal { get; set; }

        [JsonIgnore]
        public bool IsAvailable 
        {
            get
            {
                return Animal == null;
            }
        }
    }
}
