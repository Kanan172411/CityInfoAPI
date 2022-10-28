using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCityAsync(int id, bool includePointsOfInterest);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId); 
        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterstId);
        Task<bool> CityExistAsync(int cityId);
        Task AddPointOfInterestforCityAsyc(int cityId, PointOfInterest pointOfInterest);
        Task<bool> SaveChangesAsync();
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
    }
}
