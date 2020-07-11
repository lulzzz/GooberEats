using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using GooberEatsAPI.Models;
using GooberEats.Views;

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

        // Present and bind options for distance picker.
        List<string> distanceOptions = new List<string>
        {
            "5 miles",
            "10 miles",
            "25 miles",
            "50 miles"
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

        // Request a restaurant from the GooberEatsAPI.
        public ICommand CallAPI { get; }
        async void FindAPlace()
        {
            // Get the user's current device location.
            GetLocation();

            // Convert the user selected distance radius from a string in miles to an int in meters.
            int radius = 0;
            switch (Distance)
            {
                case "5 miles":
                    radius = 8050;
                    break;
                case "10 miles":
                    radius = 16100;
                    break;
                case "25 miles":
                    radius = 40240;
                    break;
                case "50 miles":
                    radius = 80500;
                    break;
                default:
                    radius = 8050;
                    break;
            }

            // Clear and set the HttpClient headers to accept json data from the API.
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Debug API uri.
            // string uri = $"https://10.0.0.56:5001/api/places/47.126166/-122.412825/{radius}";
            string uri = $"https://10.0.0.56:5001/api/places/{Latitude}/{Longitude}/{radius}";

            // Submit and consume our GET request to the GooberEatsAPI server.
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                // Read and then deserialize the returned result into a Place object.
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Place>(content);

                await _navigation.PushAsync(new ResultPage(result));
            }
            else
            {
                Place result = new Place
                {
                    Name = "Didn't work..."
                };
                await _navigation.PushAsync(new NavigationPage(new ResultPage(result)));
            }
        }

        // Get the user's current device location.
        async void GetLocation()
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
                return true;
                /*if (cert.Issuer.Equals("CN=localhost"))
                {
                    return true;
                }

                return errors == System.Net.Security.SslPolicyErrors.None;*/
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
