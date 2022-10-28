using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<(IEnumerable<City>, PagiNationMetaData)> GetCitiesAsync(string? name, string? sarchQuery, int pageNumber, int pageSize);
        Task<City?> GetCityAsync(int id, bool includePointsOfInterest);
        Task<bool> CityExistAsync(int cityId);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId); 
        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterstId);
        Task AddPointOfInterestforCityAsyc(int cityId, PointOfInterest pointOfInterest);
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
        Task<bool> CityNameMatcherCityId(string cityName, int cityId);
        Task<bool> SaveChangesAsync();
    }
}
