using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCitydAsync(int id, bool includePointsOfInterest);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId); 
        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterstId); 

    }
}
