using CommunityToolkit.Mvvm.ComponentModel;

namespace Domain.Entities;

public partial class TodoTask : ObservableObject
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public required string Title { get; set; }

    public required string Detail { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan Time { get; set; }
    
    [ObservableProperty]
    private bool _isCompleted;
}