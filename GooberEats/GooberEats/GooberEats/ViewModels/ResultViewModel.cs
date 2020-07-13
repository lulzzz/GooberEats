using GooberEatsAPI.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GooberEats.ViewModels
{
    class ResultViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor method to instantiate new ResultViewModel object.
        /// </summary>
        /// <param name="navigation">The INavigation interface.</param>
        /// <param name="result">The returned Place result from the query.</param>
        public ResultViewModel(INavigation navigation, Place result)
        {
            _navigation = navigation;
            Result = result;
            OpenMap = new Command(MapResult);
        }

        // Implements the INavigation interface.
        private readonly INavigation _navigation;

        // Bind Place result.
        Place result;
        public Place Result
        {
            get => result;
            set
            {
                result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        // Map Result Command to open the returned Place result in the native Maps application.
        public ICommand OpenMap { get; }
        async void MapResult()
        {
            await Launcher.OpenAsync($"geo:{result.Latitude},{result.Longitude}?q={result.Address}");
        }

        // Handle property change events.
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
