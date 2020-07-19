using GooberEatsAPI.Models.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace GooberEatsAPI.Models.Services
{
    public class PlacesService : IPlaces
    {
        /// <summary>
        /// Constructor method to instantiate a new PlacesService instance.
        /// </summary>
        /// <param name="configuration">The IConfiguration interface to access our User Secrets.</param>
        public PlacesService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// The IConfiguration interface to access our User Secrets data.
        /// </summary>
        private readonly IConfiguration Configuration;

        /// <summary>
        /// The HttpClient to make web requests.
        /// </summary>
        private static readonly HttpClient _client = new HttpClient();

        /// <summary>
        /// A List to contain all of our returned Places.
        /// </summary>
        List<Place> Places = new List<Place>();

        /// <summary>
        /// Takes in location data from the GooberEats Android application, and forwards that data (with supplemental options) to the Google Places API to retrieve a list of results.
        /// </summary>
        /// <param name="latitude">The Android device location latitude.</param>
        /// <param name="longitude">The Android device location longitude.</param>
        /// <param name="radius">The distance radius the user is willing to travel.</param>
        /// <param name="keyword">The keyword the user intends to filter by.</param>
        /// <returns>A single Place result.</returns>
        public async Task<Place> GetPlaces(double latitude, double longitude, int radius, string keyword)
        {
            // Our key used to build our uri to make our web request.
            var placesKey = Configuration["ApiKeys:GooglePlacesApiKey"];

            // The uri used to make our actual web request to the Google Places API.
            string uri = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?&location={latitude},{longitude}&radius={radius}&type=restaurant&keyword={keyword}&key={placesKey}";

            // Make our initial query to the Google Places API.
            var jsonResult = await QueryPlaces(uri);

            // Check if a next_page_token exists.
            while (jsonResult.NextPage != null)
            {
                // Set the uri to query the next page of the returned results.
                uri = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?pagetoken={jsonResult.NextPage}&key={placesKey}";

                // Request the next page of results.
                jsonResult = await QueryPlaces(uri);
            }

            // Convert our List to an array, and randomly select an index to return.
            Place[] placesArray = Places.ToArray();
            int numberOfPlaces = placesArray.Length;

            Random random = new Random();
            int randomNumber = random.Next(0, numberOfPlaces);

            return placesArray[randomNumber];
        }

        /// <summary>
        /// Queries the Google Places API, deserializes the Place objects, and adds them to the Places List.
        /// </summary>
        /// <param name="uri">The uri to query with parameters.</param>
        /// <returns>The deserialized PlacesResponse object.</returns>
        public async Task<PlacesResponse> QueryPlaces(string uri)
        {
            // Clearing and setting our _client headers to prep it for retrieval of json data.
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // The actual web request being made.
            var request = await _client.GetStreamAsync(uri);

            // Reading and deserializing the data that comes back from the Google Places API.
            using (var reader = new StreamReader(request))
            {
                var response = reader.ReadToEnd();
                var jsonResult = JsonConvert.DeserializeObject<PlacesResponse>(response);

                // Converting each result into a Place, and adding it to our List.
                foreach (var result in jsonResult.Results)
                {
                    Place place = new Place
                    {
                        Name = result.Name,
                        Latitude = result.Geometry.Location.Latitude,
                        Longitude = result.Geometry.Location.Longitude,
                        Rating = result.Rating,
                        TotalReviews = result.TotalReviews,
                        Address = result.Vicinity,
                        PlusCode = result.PlusCode.CompoundCode
                    };

                    Places.Add(place);
                }

                return jsonResult;
            }
        }
    }
}
