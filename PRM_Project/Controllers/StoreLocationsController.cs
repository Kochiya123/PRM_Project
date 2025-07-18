using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM_Project.Models;
using static System.Net.WebRequestMethods;

namespace PRM_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StoreLocationsController : ControllerBase
    {
        private readonly SalesAppDbContext dbContext;
        private ILogger _logger;
        public StoreLocationsController(SalesAppDbContext dbContext, ILogger logger)
        {
            this.dbContext = dbContext;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<StoreLocation>>> GetAllStoreLocation()
        {
            var storeLocation = await dbContext.StoreLocations.ToListAsync();
            return Ok(storeLocation);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<StoreLocation>> GetStoreLocationbyID(int id)
        {
            var storeLocation = await dbContext.StoreLocations.FindAsync(id);

            if (storeLocation == null)
            {
                return NotFound();
            }

            return Ok(storeLocation);
        }

        [HttpPost]
        public async Task<ActionResult<List<StoreLocation>>> AddStoreLocation(AddStoreLocationDTO StoreLocation)
        {
            var storeLocationObject = new StoreLocation()
            {
                Latitude = StoreLocation.Latitude,
                Longitude = StoreLocation.Longitude,
                Address = StoreLocation.Address,
            };
            try
            {
                dbContext.StoreLocations.Add(storeLocationObject);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Add Store Location Failed!!");
                return StatusCode(500, "Please try again later");
            }
            await dbContext.SaveChangesAsync();

            return Ok(storeLocationObject);

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<List<StoreLocation>>> UpdateStoreLocation(int id, UpdateStoreLocationDTO updateStoreLocation)
        {
            var storeLocation = await dbContext.StoreLocations.FindAsync(id);

            if (storeLocation == null)
            {
                return NotFound(id);
            }

            storeLocation.Latitude = updateStoreLocation.Latitude;
            storeLocation.Longitude = updateStoreLocation.Longitude;
            storeLocation.Address = updateStoreLocation.Address;

            await dbContext.SaveChangesAsync();

            return Ok(storeLocation);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<List<StoreLocation>>> DeleteStoreLocation(int id)
        {
            var storeLocation = await dbContext.StoreLocations.FindAsync(id);
            if (storeLocation == null)
            {
                return NotFound();
            }

            dbContext.StoreLocations.Remove(storeLocation);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
