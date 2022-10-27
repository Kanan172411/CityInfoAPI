using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }
        //public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "London",
                    Description = "London is the capital city of the United Kingdom.",
                    PointOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id=1,
                            Name = "Big Ben",
                            Description = "Big Ben is a tower clock known for its accuracy and for its massive hour bell."
                        },
                        new PointOfInterestDto()
                        {
                            Id=2,
                            Name = "Big Ben",
                            Description = "Big Ben is a tower clock known for its accuracy and for its massive hour bell."
                        }
                    }
                },               
                
                new CityDto()
                {
                    Id = 2,
                    Name = "Berlin",
                    Description = "The one with that big clock.",
                    PointOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id=3,
                            Name = "Big Ben",
                            Description = "Big Ben is a tower clock known for its accuracy and for its massive hour bell."
                        },
                        new PointOfInterestDto()
                        {
                            Id=4,
                            Name = "Big Ben",
                            Description = "Big Ben is a tower clock known for its accuracy and for its massive hour bell."
                        }
                    }
                },                
                
                new CityDto()
                {
                    Id = 3,
                    Name = "Sydney",
                    Description = "The one with that big clock.",
                    PointOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id=5,
                            Name = "Big Ben",
                            Description = "Big Ben is a tower clock known for its accuracy and for its massive hour bell."
                        },
                        new PointOfInterestDto()
                        {
                            Id=6,
                            Name = "Big Ben",
                            Description = "Big Ben is a tower clock known for its accuracy and for its massive hour bell."
                        }
                    }
                },                
                
                new CityDto()
                {
                    Id = 4,
                    Name = "Moscow",
                    Description = "The one with that big clock.",
                    PointOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id=7,
                            Name = "Big Ben",
                            Description = "Big Ben is a tower clock known for its accuracy and for its massive hour bell."
                        },
                        new PointOfInterestDto()
                        {
                            Id=8,
                            Name = "Big Ben",
                            Description = "Big Ben is a tower clock known for its accuracy and for its massive hour bell."
                        }
                    }
                },
            };
        }
    }
}
