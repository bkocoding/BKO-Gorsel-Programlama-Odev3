using Domain.Entities;

namespace Application.Interfaces;

public interface IWeatherService
{
    Task<List<WeatherCity>> GetSavedCitiesAsync();
    
    Task AddCityAsync(WeatherCity city);
    
    Task RemoveCityAsync(string cityName);
    
    List<string> GetAllCityNames();
}