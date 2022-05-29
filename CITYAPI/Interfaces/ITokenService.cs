using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CITYAPI.Entities;

namespace CITYAPI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user); 
    }
}