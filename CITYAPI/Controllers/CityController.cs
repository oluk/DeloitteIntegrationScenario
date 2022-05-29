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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace CITYAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CityController : ControllerBase
    {
        
        private readonly ICityRepository _cityRepository;
        public CityController(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }


        [HttpPost("createcity")]
        [Authorize]
        public async Task<ActionResult<City>> create(DTOCity dtocity)
        {
            try
            { 

            if(await _cityRepository.CityExists(dtocity.cityname)) return BadRequest("City Already Exists");
          

            var city = new City{
                    CityName = dtocity.cityname.ToLower(),
                    State = dtocity.state,
                    Country = dtocity.country,
                    TouristRating = dtocity.touristrating,
                    DateEstablished = dtocity.dateestablished,
                    EstPopulation = dtocity.estpopulation,
                   
                };

                await _cityRepository.AddCity(city);

                return city;
               
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error Creating City");
            }
        }

       

       


        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<ActionResult<City>> update(int id, City city)
        {
            try
            { 
                   if(id != city.Id) return BadRequest("Mismatched City Id");
                 
                   var updatedCity = await _cityRepository.UpdateCity(city);
                    if (updatedCity == null)
                        return NotFound("City not found");

                     return updatedCity;
                   
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating city");
            }


        }

 
        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> deletecity(int id)
        {
            try
            {
                
              var city = await _cityRepository.DeleteCity(id);
              if(city == null)
                return NotFound("Invalid City");
             
              return Ok("City Removed Successfully");    
            }
            catch (Exception)
            {
               return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting City");
            }  
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<City>> getcity(int id)
        {
            try
            {
                return await _cityRepository.GetCity(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     "Error Getting City");
            }

        }

      
        [HttpGet("search/{cityname}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<DTOCityVM>>> searchcitybyname(string cityname)
        {
            try
            {
                List<DTOCityVM> searchresponse = new List<DTOCityVM>();
                IEnumerable <City> citylist =  await _cityRepository.GetCityByName(cityname);
                if (citylist == null)
                {
                    return NotFound("No city found");
                }
                else
                {
                    foreach (City city in citylist)
                    {
                         
                        DTOCityVM cityvm = new DTOCityVM();
                        cityvm.cityname = city.CityName;
                        cityvm.state = city.State;
                        cityvm.country = city.Country;
                        cityvm.touristrating = city.TouristRating;
                        cityvm.estpopulation = city.EstPopulation;

                        DTOCountryDetails countrydetails = await _cityRepository.GetCountryDetails(city.Country);
                       
                        if (countrydetails != null)
                        {
                            cityvm.countrycode2 = countrydetails.cca2;
                            cityvm.countrycode3 = countrydetails.cca3;
                            cityvm.currencies = countrydetails.currencies;
                            cityvm.weather = await _cityRepository.GetWeatherDetails(countrydetails.latlng);
                           
                        }
                       
                        searchresponse.Add(cityvm);

                   }

                    return searchresponse;


               }



            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     "Error Getting City");
            }

        }



    }
}