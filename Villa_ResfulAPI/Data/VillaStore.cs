using Villa_ResfulAPI.Models.DTO;

namespace Villa_ResfulAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> listVilla = new List<VillaDto>
        {
            new VillaDto{Id = 1, Name = "Faisal",Occupancy=3,Sqft=200},
            new VillaDto{Id = 2, Name = "Mostakbal",Occupancy=4,Sqft=270}
        };
    }
}
