using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CITYAPI.DTOs
{
    public class DTOWeatherDetails
    {

        public string id { get; set; }

        public string main { get; set; }

        public List<string> description { get; set; }

        public int[] latlng { get; set; }



    }
}