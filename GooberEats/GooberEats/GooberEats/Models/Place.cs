namespace GooberEatsAPI.Models
{
    public class Place
    {
        /// <summary>
        /// Constructor method to instantiate a new Place object.
        /// </summary>
        public Place()
        {

        }

        /// <summary>
        /// The name of the Place.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The latitude of the Place.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// The longitude of the Place.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// The overall rating of the Place.
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// The total number of reviews for the Place.
        /// </summary>
        public int TotalReviews { get; set; }

        /// <summary>
        /// The address of the Place.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The Google Places Plus Code (Compound Code) of the Place.
        /// </summary>
        public string PlusCode { get; set; }
    }
}
