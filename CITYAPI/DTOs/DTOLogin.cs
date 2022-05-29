using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CITYAPI.DTOs
{
    public class DTOLogin
    {
       [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
    }
}