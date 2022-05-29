using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CITYAPI.Data;
using CITYAPI.DTOs;
using CITYAPI.Entities;
using CITYAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CITYAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AccessControlController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenservice;
        public AccessControlController(DataContext context, ITokenService tokenservice)
        {
            _tokenservice = tokenservice;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<DTOUser>> Register([FromBody] DTOLogin dtologin)
        {
            if(await UserExists(dtologin.username)) return BadRequest("User Already Exists");

                using var hmac = new HMACSHA512();
                var user = new User{
                    UserName = dtologin.username.ToLower(),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dtologin.password)),
                    PasswordSalt = hmac.Key
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return new DTOUser{
                    username = user.UserName,
                 token = _tokenservice.CreateToken(user)
                };
        
        
        }

        [HttpPost("login")]
        public async Task<ActionResult<DTOUser>> login(DTOLogin dtologin)
        {
             var user = await _context.Users.SingleOrDefaultAsync( u =>  u.UserName == dtologin.username.ToLower());
             if(user == null) return Unauthorized("Unauthorized User");

             using var hmac = new HMACSHA512(user.PasswordSalt);

             var computehash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dtologin.password));

             for(int i=0; i< computehash.Length; i++)
             {
                 if(computehash[i] != user.PasswordHash[i])
                     return Unauthorized("Invalid Password");
             };

             return new DTOUser{
                    username = user.UserName,
                 token = _tokenservice.CreateToken(user)
                };



        }    

        private async Task<bool> UserExists(string username)
        {
           return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}