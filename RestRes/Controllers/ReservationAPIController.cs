using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using RestRes.Models;
using RestRes.Services;

namespace RestRes.ApiControllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ReservationsApiController : ControllerBase
    {
        private readonly IReservationService _ReservationService;
        private readonly IRestaurantService _RestaurantService;

        public ReservationsApiController(IReservationService reservationService, IRestaurantService restaurantService)
        {
            _ReservationService = reservationService;
            _RestaurantService = restaurantService;
        }

        // GET: api/Reservations
        [HttpGet]
        public IActionResult GetAll()
        {
            var reservations = _ReservationService.GetAllReservations();
            return Ok(reservations);
        }

        // GET: api/Reservations/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Invalid ID");

            var result = _ReservationService.GetReservationById(new ObjectId(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST: api/Reservations
        [HttpPost]
        public IActionResult Create(Reservation reservation)
        {
            if (reservation == null)
                return BadRequest();

            // you could also check if restaurant exists
            _ReservationService.AddReservation(reservation);
            return CreatedAtAction(nameof(GetById), new { id = reservation.Id.ToString() }, reservation);
        }

        // PUT: api/Reservations/{id}
        [HttpPut("{id}")]
        public IActionResult Update(string id, Reservation reservation)
        {
            if (id != reservation.Id.ToString())
                return BadRequest("ID mismatch!");

            var existing = _ReservationService.GetReservationById(new ObjectId(id));
            if (existing == null)
                return NotFound();

            _ReservationService.EditReservation(reservation);
            return NoContent();
        }

        // DELETE: api/Reservations/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Invalid ID");

            var existing = _ReservationService.GetReservationById(new ObjectId(id));
            if (existing == null)
                return NotFound();

            _ReservationService.DeleteReservation(existing);
            return NoContent();
        }
    }
}
