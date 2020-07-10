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

        public async Task<string> Index()
        {
            var output = await _places.GetPlaces();

            string debugText = @"Name: {0}
Rating: {1}
Reviews: {2}
Latitude: {3}
Longitude: {4}
Address: {5}";

            return string.Format(debugText, output.Name, output.Rating, output.TotalReviews, output.Latitude, output.Longitude, output.Address);
        }
    }
}
