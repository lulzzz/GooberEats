using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using GooberEatsAPI.Models;
using GooberEats.Views;
using System.Threading.Tasks;

namespace GooberEats.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor method to instantiate the MainViewModel, and define our Commands.
        /// </summary>
        public MainViewModel(INavigation navigation)
        {
            _navigation = navigation;
            CallAPI = new Command(FindAPlace);
        }

        // Implements the INavigation interface.
        INavigation _navigation;

        // Prep our HttpClient object to accept development certs in local environment.
       static HttpClientHandler insecureHandler = GetInsecureHandler();

        //Establish our HttpClient object.
        private static readonly HttpClient _client = new HttpClient(insecureHandler);

        // Bind activity indicator IsBusy status.
        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        // Present and bind options for distance picker.
        List<string> distanceOptions = new List<string>
        {
            "5 miles",
            "10 miles",
            "15 miles",
            "25 miles"
        };
        public List<string> DistanceOptions
        {
            get => distanceOptions;
            set
            {
                distanceOptions = value;
                OnPropertyChanged(nameof(DistanceOptions));
            }
        }

        // Bind selected distance.
        string distance;
        public string Distance
        {
            get => distance;
            set
            {
                distance = value;
                OnPropertyChanged(nameof(Distance));
            }
        }

        // Bind device location.
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Bind keyword search entry.
        string keyword;
        public string Keyword
        {
            get => keyword;
            set
            {
                keyword = value;
                OnPropertyChanged(nameof(Keyword));
            }
        }

        // Request a restaurant from the GooberEatsAPI.
        public ICommand CallAPI { get; }
        async void FindAPlace()
        {
            // Enable Activity Indicator.
            IsBusy = true;

            // Get the user's current device location.
            await GetLocation();

            // Convert the user selected distance radius from a string in miles to an int in meters.
            int radius;
            switch (Distance)
            {
                case "5 miles":
                    radius = 8050;
                    break;
                case "10 miles":
                    radius = 16100;
                    break;
                case "15 miles":
                    radius = 24150;
                    break;
                case "25 miles":
                    radius = 40240;
                    break;
                default:
                    radius = 8050;
                    break;
            }

            // Apply the default keyword of "take out" if the user does not enter a keyword.
            if (Keyword == null)
            {
                Keyword = "take%20out";
            }

            // Clear and set the HttpClient headers to accept json data from the API.
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Production API uri.
            string uri = $"https://goobereatsapi.azurewebsites.net/api/places/{Latitude}/{Longitude}/{radius}/{Keyword}";

            // Submit and consume our GET request to the GooberEatsAPI server.
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                // Read and then deserialize the returned result into a Place object.
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Place>(content);

                // Disable Activity Indicator.
                IsBusy = false;

                await _navigation.PushAsync(new ResultPage(result));
            }
            else
            {
                // Custom Place "Result" if API response failed...
                Place result = new Place
                {
                    Name = "Please try again...",
                    Rating = 0,
                    TotalReviews = 0,
                    Address = "Failed to retrieve result."
                };

                // Disable Activity Indicator.
                IsBusy = false;

                // Navigate application to the ResultPage view, passing the returned result.
                await _navigation.PushAsync(new NavigationPage(new ResultPage(result)));
            }
        }

        // Get the user's current device location.
        async Task GetLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                }

            }
            catch (Exception ex)
            {

                Debug.WriteLine($"Location Exception: {ex}");
            }
        }

        // Allow local development certificate to be accepted when debugging.
        public static HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                {
                    return true;
                }

                return errors == System.Net.Security.SslPolicyErrors.None;
            };

            return handler;
        }

        // Handle property change events.
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}
