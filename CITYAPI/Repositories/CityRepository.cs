using CITYAPI.Data;
using CITYAPI.DTOs;
using CITYAPI.Entities;
using CITYAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;


namespace CITYAPI.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public CityRepository(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<City> AddCity(City city)
        {
            var result = await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<City> DeleteCity(int id)
        {
            var result = await _context.Cities.FindAsync(id);
            if (result != null)
            {
               _context.Cities.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }

            return null;
        }

        public async Task<City> GetCity(int id)
        {
            return await _context.Cities.FindAsync(id);
        }

        public async Task<IEnumerable<City>> GetCityByName(string cityname)
        {
            return await _context.Cities.Where(c => c.CityName == cityname).ToListAsync();
        }

        public async Task<bool> CityExists(string cityname)
        { 
         return  await _context.Cities.AnyAsync(c => c.CityName == cityname.ToLower());
        }
        public async Task<IEnumerable<City>> GetCities()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task<City> UpdateCity(City city)
        {
            var result = await _context.Cities.FindAsync(city.Id);
            if (result != null)
            {
               
                result.TouristRating = city.TouristRating;
                result.DateEstablished = city.DateEstablished;
                result.EstPopulation = city.EstPopulation;

                _context.Entry(result).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return result;
            }

            return null;
        }

        public async Task<DTOCountryDetails> GetCountryDetails(string countryname)
        {
            //Fetch the JSON string from URL.
           
            DTOCountryDetails countrydetails = new DTOCountryDetails();
            string apiurl = String.Format(_config["countryAPI"], countryname);

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(apiurl))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        var dynamicObject = JsonConvert.DeserializeObject<dynamic>(apiResponse)!;

                        countrydetails.cca2 = dynamicObject[0]["cca2"];
                        countrydetails.cca3 = dynamicObject[0]["cca3"];
                        var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject((dynamicObject[0]["currencies"])));
                        var currencies = new List<string>();
                        foreach (KeyValuePair<string, object> kvp in dict)
                        {
                            currencies.Add(kvp.Key);
                        }
                        countrydetails.currencies = currencies;
                        countrydetails.latlng = new int[] { (dynamicObject[0]["latlng"])[0], (dynamicObject[0]["latlng"])[1] };
                    }
                   
                    

                }
            }

            return countrydetails;


        }

        public async Task<string> GetWeatherDetails(int[] latlng)
        {
            if ((latlng != null ) && (latlng.Length == 2))
            {
                //compose ur using formatted string with config data
                string apiurl = String.Format(_config["weatherAPI"], latlng[0], latlng[1], _config["weatherAPIKey"]);

                //Fetch the JSON string from URL.
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(apiurl))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(apiResponse)!;

                            string currentweather = ((dynamicObject["current"])["weather"][0])["description"];
                            return currentweather;
                        }
                        else
                        {
                            return "";
                        }
                    }
                }

            }
             
            return "";


        }
    }
}
