using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "London",
                    Description = "The one with that big clock."
                },               
                
                new CityDto()
                {
                    Id = 2,
                    Name = "Berlin",
                    Description = "The one with that big clock."
                },                
                
                new CityDto()
                {
                    Id = 3,
                    Name = "Sydney",
                    Description = "The one with that big clock."
                },                
                
                new CityDto()
                {
                    Id = 4,
                    Name = "Moscow",
                    Description = "The one with that big clock."
                },
            };
        }
    }
}
