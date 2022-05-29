using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CITYAPI.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        [Range(1, 5)]
        public byte TouristRating { get; set; }
        public DateTime DateEstablished { get; set; }
        public int EstPopulation { get; set; }
   
    }
}