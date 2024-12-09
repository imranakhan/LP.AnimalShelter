using LP.AnimalShelter.API.Enums;

namespace LP.AnimalShelter.API.Models
{
    public class Shelter
    {
        public Shelter(int totalLargeKennels, int totalMediumKennels, int totalSmallKennels)
        {
            NumOfLargeKennels = totalLargeKennels;
            NumOfMediumKennels = totalMediumKennels;
            NumOfSmallKennels = totalSmallKennels;

            Kennels = new List<Kennel>();
            AddKennels(KennelType.Large, NumOfLargeKennels);
            AddKennels(KennelType.Medium, NumOfMediumKennels);
            AddKennels(KennelType.Small, NumOfSmallKennels);
        }

        public int NumOfLargeKennels { get; set; }
        public int NumOfMediumKennels { get; set; }
        public int NumOfSmallKennels { get; set; }
        public List<Kennel> Kennels { get; set; }

        public int TotalKennels
        {
            get
            {
                return NumOfLargeKennels + NumOfMediumKennels + TotalKennels;
            }
        }

        public int TotalAvailableKennels
        {
            get
            {
                return Kennels.Where(k => k.IsAvailable).Count();
            }
        }

        public bool HasAvailableLargeKennels
        {
            get
            {
                return Kennels.Where(k => k.IsAvailable && k.Type == KennelType.Large).Count() > 0;
            }
        }

        public bool HasAvailableMediumKennels
        {
            get
            {
                return Kennels.Where(k => k.IsAvailable && k.Type == KennelType.Medium).Count() > 0;
            }
        }

        public bool HasAvailableSmallKennels
        {
            get
            {
                return Kennels.Where(k => k.IsAvailable && k.Type == KennelType.Small).Count() > 0;
            }
        }

        public bool HasAvailableKennels
        {
            get
            {
                return Kennels.Where(k => k.IsAvailable).Count() > 0;
            }
        }

        public List<int> ExistingAnimalIds
        {
            get
            {
                return Kennels.Where(k => !k.IsAvailable).Select(s => s.Animal.Id).ToList();
            }
        }

        private void AddKennel(KennelType type)
        {
            Kennels.Add(new Kennel { Type = type });
        }

        private void AddKennels(KennelType type, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                AddKennel(type);
            }
        }
    }
}
