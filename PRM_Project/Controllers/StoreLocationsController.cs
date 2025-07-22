using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM_Project.Models;
using static System.Net.WebRequestMethods;

namespace PRM_Project.Controllers
{
    [Route("api/store-locations")]
    [ApiController]
    public class StoreLocationsController : ControllerBase
    {
        private readonly SalesAppDbContext dbContext;
        private ILogger<StoreLocationsController> _logger;
        private readonly IMapper _mapper;
        public StoreLocationsController(SalesAppDbContext dbContext, ILogger<StoreLocationsController> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
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
        public async Task<ActionResult<List<StoreLocation>>> AddStoreLocation(StoreLocationDTO storeLocation)
        {
            StoreLocation storeLocationObject = _mapper.Map<StoreLocation>(storeLocation);
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
        public async Task<ActionResult<List<StoreLocation>>> UpdateStoreLocation(int id, StoreLocationDTO updateStoreLocation)
        {
            var storeLocation = await dbContext.StoreLocations.FindAsync(id);

            if (storeLocation == null)
            {
                return NotFound(id);
            }

            storeLocation = _mapper.Map<StoreLocation>(updateStoreLocation);

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
