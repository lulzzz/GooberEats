using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GooberEatsAPI.Models.Interfaces
{
    public interface IPlaces
    {
        /// <summary>
        /// Takes in location data from the GooberEats Android application, and forwards that data (with supplemental options) to the Google Places API to retrieve a list of results.
        /// </summary>
        /// <param name="latitude">The Android device location latitude.</param>
        /// <param name="longitude">The Android device location longitude.</param>
        /// <param name="radius">The distance radius the user is willing to travel.</param>
        /// <returns>A single Place result.</returns>
        public Task<Place> GetPlaces(double latitude, double longitude, int radius);
    }
}
