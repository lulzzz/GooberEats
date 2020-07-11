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


        public async Task<Place> GetPlaces(double latitude, double longitude, int radius)
        {
            // Our variables used to build our uri to make our web request.
            var placesKey = Configuration["ApiKeys:GooglePlacesApiKey"];
            string keyword = "take%20out";

            // The uri used to make our actual web request to the Google Places API.
            string uri = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?type=restaurant&location={latitude},{longitude}&radius={radius}&keyword={keyword}&key={placesKey}";

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

                // Creating a List to hold all of our results, in order to randomly select one after.
                List<Place> Places = new List<Place>();
                
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
                        Address = result.Vicinity
                    };

                    Places.Add(place);
                }

                // Convert our List to an array, and randomly select an index to return.
                Place[] placesArray = Places.ToArray();
                int numberOfPlaces = placesArray.Length;

                Random random = new Random();
                int randomNumber = random.Next(0, numberOfPlaces);

                return placesArray[randomNumber];
            }

            
        }
    }
}
