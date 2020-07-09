using GooberEatsAPI.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GooberEatsAPI.Models.Services
{
    public class PlacesService : IPlaces
    {
        public Task<List<Place>> GetPlaces()
        {
            throw new NotImplementedException();
        }
    }
}
