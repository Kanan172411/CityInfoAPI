using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
        }      
        
        public async Task<(IEnumerable<City>, PagiNationMetaData)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            var collection = _context.Cities as IQueryable<City>;

            if (!string.IsNullOrEmpty(name))
            {
                name = name.Trim();
                collection = collection.Where(c => c.Name == name);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(a => a.Name.Contains(searchQuery)
                    || (a.Description != null && a.Description.Contains(searchQuery)));
            }

            var totalItemCount = await collection.CountAsync();
            var paginationMetaData = new PagiNationMetaData(
                totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection
                .OrderBy(c => c.Name)
                .Skip(pageSize * (pageNumber-1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetaData);
        }

        public async Task<bool> CityExistAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task<City?> GetCityAsync(int id, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return await _context.Cities
                    .Include(c => c.PointOfInterest)
                    .Where(c =>c.Id == id)
                    .FirstOrDefaultAsync();
            }

            return await _context.Cities
                    .Where(c => c.Id == id)
                    .FirstOrDefaultAsync();
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterstId)
        {
            return await _context.PointOfInterests
                .Where(p => p.CityId == cityId && p.Id == pointOfInterstId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await _context.PointOfInterests
                .Where(p => p.CityId == cityId).ToListAsync();
        }

        public async Task AddPointOfInterestforCityAsyc(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);
            if (city != null) 
            {
                city.PointOfInterest.Add(pointOfInterest);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _context.PointOfInterests.Remove(pointOfInterest);
        }

        public async Task<bool> CityNameMatcherCityId(string cityName, int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId && c.Name == cityName);
        }
    }
}
