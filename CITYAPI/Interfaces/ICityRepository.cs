using CITYAPI.DTOs;
using CITYAPI.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CITYAPI.Interfaces
{
    public interface ICityRepository
    {
        Task<City> AddCity(City city);

        Task<City> DeleteCity(int id);

        Task<City> GetCity(int id);

        Task<IEnumerable<City>> GetCityByName(string cityname);
        
        Task<bool> CityExists(string cityname);

        Task<IEnumerable<City>> GetCities();

        Task<City> UpdateCity(City city);

        Task<DTOCountryDetails> GetCountryDetails(string countryname);

        Task<string> GetWeatherDetails(int[] latlng);


    }
}
