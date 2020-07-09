using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace GooberEats.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            GetLocationCommand = new Command(GetLocation);
            CallAPI = new Command(FindAPlace);
        }

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
        double latitude;
        public double Latitude
        {
            get => latitude;
            set
            {
                latitude = value;
                OnPropertyChanged(nameof(Latitude));
            }
        }

        double longitude;
        public double Longitude
        {
            get => longitude;
            set
            {
                longitude = value;
                OnPropertyChanged(nameof(Longitude));
            }
        }

        // Get the user's current device location.
        public ICommand GetLocationCommand { get; }
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

        // Request a restaurant from the GooberEatsAPI.
        public ICommand CallAPI { get; }
        void FindAPlace()
        {

        }

        // Handle property change events.
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}
