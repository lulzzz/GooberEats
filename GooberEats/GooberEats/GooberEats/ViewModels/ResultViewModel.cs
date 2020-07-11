using GooberEatsAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GooberEats.ViewModels
{
    class ResultViewModel : INotifyPropertyChanged
    {
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

        // Map Result Command
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
