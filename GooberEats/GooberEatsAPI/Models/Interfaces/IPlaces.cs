using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GooberEatsAPI.Models.Interfaces
{
    public interface IPlaces
    {
        public Task<Place> GetPlaces(double latitude, double longitude, int radius);
    }
}
