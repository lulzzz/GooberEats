using GooberEats.ViewModels;
using GooberEatsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GooberEats.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPage : ContentPage
    {
        public ResultPage(Place result)
        {
            InitializeComponent();
            BindingContext = new ResultViewModel(Navigation, result);
        }
    }
}