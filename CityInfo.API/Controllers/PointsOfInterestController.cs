using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/v{version:apiversion}/cities/{cityId}/pointsOfInterest")]
    // [Authorize(Policy ="MustBeFromBaku")]
    [ApiVersion("2.0")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _repository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(
            ILogger<PointsOfInterestController> logger,
            IMailService mailService, 
            ICityInfoRepository repository,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            //var cityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;

            //if(!await _repository.CityNameMatcherCityId(cityName, cityId))
            //{
            //    return Forbid();
            //}

            if(!await _repository.CityExistAsync(cityId))
            {
                _logger.LogInformation($"--> City with Id {cityId} wasn't found when accessing points of interest");
                return NotFound();
            }

            var pointOfInterestForCity = await _repository.GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointOfInterestForCity));
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _repository.CityExistAsync(cityId))
            {
                _logger.LogInformation($"--> City with Id {cityId} wasn't found when accessing points of interest");
                return NotFound();
            }

            var pointOfInterest = await _repository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId, PointsOfInterestForCreationDto pointOfInterestDto)
        {
            if (!await _repository.CityExistAsync(cityId))
            {
                return NotFound();
            }

            var finalPointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterestDto);

            await _repository.AddPointOfInterestforCityAsyc(cityId, finalPointOfInterest);

            await _repository.SaveChangesAsync();

            var createdPointOfInterest = _mapper.Map<PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",
                 new
                 {
                     cityId = cityId,
                     pointOfInterestId = createdPointOfInterest.Id
                 },
                 createdPointOfInterest
                );
        }

        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointofInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterestForUpdateDto)
        {
            if (!await _repository.CityExistAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = await _repository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            _mapper.Map(pointOfInterestForUpdateDto, pointOfInterest);

            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(
            int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {

            if (!await _repository.CityExistAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = await _repository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterest);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestToPatch, pointOfInterest);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _repository.CityExistAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = await _repository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            _repository.DeletePointOfInterest(pointOfInterest);

             await _repository.SaveChangesAsync();

            _mailService.Send(
                "Point of interest deleted.",
                $"Point of interest {pointOfInterest.Name} with id {pointOfInterest.Id} was deleted.");

            return NoContent();
        }
    }
}
