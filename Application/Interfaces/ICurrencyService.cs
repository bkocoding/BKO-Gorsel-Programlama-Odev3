using Domain.Entities;

namespace Application.Interfaces;

public interface ICurrencyService
{
    Task<List<Currency>> GetCurrenciesAsync();
}