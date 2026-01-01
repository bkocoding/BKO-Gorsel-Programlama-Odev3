namespace Domain.Entities;

public class NewsUiCategory(string title, string rssUrl)
{
    public string Title { get; set; } = title;
    public string RssUrl { get; set; } = rssUrl;
}