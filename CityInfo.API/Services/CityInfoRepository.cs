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

        public async Task<City?> GetCitydAsync(int id, bool includePointsOfInterest)
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
    }
}
