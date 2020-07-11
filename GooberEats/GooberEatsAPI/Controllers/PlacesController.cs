using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GooberEatsAPI.Models;
using GooberEatsAPI.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GooberEatsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        /// <summary>
        /// The IPlaces interface, brought in through dependency injection.
        /// </summary>
        private readonly IPlaces _places;

        /// <summary>
        /// Constructor method to bring in the IPlaces interface upon instantiation.
        /// </summary>
        /// <param name="places">The IPlaces interface to bring in.</param>
        public PlacesController(IPlaces places)
        {
            _places = places;
        }

        [Route("{latitude}/{longitude}/{radius}")]
        public async Task<Place> Index(double latitude, double longitude, int radius)
        {
            var place = await _places.GetPlaces(latitude, longitude, radius);

            return place;
        }
    }
}
