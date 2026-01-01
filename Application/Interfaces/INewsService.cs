using Domain.Entities;

namespace Application.Interfaces;

public interface INewsService
{
    Task<List<NewsItem>> GetNewsAsync(string rssUrl);
}