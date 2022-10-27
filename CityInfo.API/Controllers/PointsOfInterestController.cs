﻿using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsOfInterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly CitiesDataStore _citiesDataStore;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, CitiesDataStore citiesDataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            try
            {
                var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);

                if (city == null)
                {
                    _logger.LogInformation($"--> City with id {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }

                return Ok(city.PointOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"--> Exception while getting points of interest for city with id {cityId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);

            if(pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointsOfInterestForCreationDto pointOfInterestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var maxPointOfInterestId = _citiesDataStore.Cities
                .SelectMany(c => c.PointOfInterest).Max(p => p.Id);
            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterestDto.Name,
                Description = pointOfInterestDto.Description
            };

            city.PointOfInterest.Add(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",
                 new
                 {
                     cityId = cityId,
                     pointOfInterestId = finalPointOfInterest.Id
                 },
                 finalPointOfInterest
                );
        }

        [HttpPut("{pointOfInterestId}")]
        public ActionResult UpdatePointofInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterestForUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            pointOfInterest.Name = pointOfInterestForUpdateDto.Name;
            pointOfInterest.Description = pointOfInterestForUpdateDto.Description;

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public ActionResult PartiallyUpdatePointOfInterest(
            int cityId, int pointOfInterestId, 
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {

            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch =
                new PointOfInterestForUpdateDto()
                {
                    Name = pointOfInterest.Name,
                    Description = pointOfInterest.Description
                };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            pointOfInterest.Name = pointOfInterestToPatch.Name;
            pointOfInterest.Description=pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            city.PointOfInterest.Remove(pointOfInterest);
            _mailService.Send(
                "Point of interest deleted.",
                $"Point of interest {pointOfInterest.Name} with id {pointOfInterest.Id} was deleted.");
            return NoContent();
        }
    }
}
