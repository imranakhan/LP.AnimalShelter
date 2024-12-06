using LP.AnimalShelter.API.Interface;

namespace LP.AnimalShelter.API.Models
{
    public class Shelter : IShelter
    {
        public Shelter(int totalLargeKernels, int totalMediumKernels, int totalSmallKernels)
        {
            TotalLargeKernels = totalLargeKernels;
            TotalMediumKernels = totalMediumKernels;
            TotalSmallKernels = totalSmallKernels;
        }

        public int TotalLargeKernels { get; set; }
        public int TotalMediumKernels { get; set; }
        public int TotalSmallKernels { get;set; }

    }
}
