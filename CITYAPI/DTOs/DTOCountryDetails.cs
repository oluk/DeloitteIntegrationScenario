using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CITYAPI.DTOs
{
    public class DTOCountryDetails
    {

        public string cca2 { get; set; }

        public string cca3 { get; set; }

        public List<string> currencies { get; set; }

        public int[] latlng { get; set; }



    }
}