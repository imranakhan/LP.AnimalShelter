using LP.AnimalShelter.API.Enums;
using System.Text.Json.Serialization;

namespace LP.AnimalShelter.API.Models
{
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
