using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CITYAPI.DTOs
{
    public class DTOCity
    {
       
        [Required]
        [ReadOnly(true)]
        public string cityname { get; set; }
        [Required]
        [ReadOnly(true)]
        public string state { get; set; }
        [Required]
        public string country { get; set; }
        [Range(1, 5)]
        public byte touristrating { get; set; }
        public DateTime dateestablished { get; set; }
        public int estpopulation { get; set; }
        
    }
}