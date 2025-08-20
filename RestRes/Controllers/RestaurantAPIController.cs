using Microsoft.AspNetCore.Mvc;
using RestRes.Models;
using RestRes.Services;

namespace RestRes.ApiControllers
{
    [ApiController]
    [Route("/[controller]")]
    public class RestaurantsApiController : ControllerBase
    {
        private readonly IRestaurantService _RestaurantService;

        public RestaurantsApiController(IRestaurantService RestaurantService)
        {
            _RestaurantService = RestaurantService;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_RestaurantService.GetAllRestaurants());

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var rest = _RestaurantService.GetRestaurantById(new MongoDB.Bson.ObjectId(id));
            if (rest == null) return NotFound();
            return Ok(rest);
        }

        [HttpPost]
        public IActionResult Create(Restaurant restaurant)
        {
            _RestaurantService.AddRestaurant(restaurant);
            return CreatedAtAction(nameof(GetById), new { id = restaurant.Id }, restaurant);
        }
    }
}
