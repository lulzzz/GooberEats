using GooberEatsAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace GooberEats.ViewModels
{
    class ResultViewModel : INotifyPropertyChanged
    {
        public ResultViewModel(INavigation navigation, Place result)
        {
            _navigation = navigation;
            Result = result;
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

        // Handle property change events.
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
