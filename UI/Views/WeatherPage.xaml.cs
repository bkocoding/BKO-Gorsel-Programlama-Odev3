namespace UI.Views;

public partial class WeatherPage : ContentPage
{
    public WeatherPage(UI.ViewModels.WeatherViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    
    private void OnWidgetLoaded(object sender, WebNavigatedEventArgs e)
    {
        if (sender is not WebView { Parent: Grid parentGrid }) return;
        if (parentGrid.Children
                .FirstOrDefault(x => x is ActivityIndicator) is not ActivityIndicator spinner) return;
        spinner.IsRunning = false;
        spinner.IsVisible = false;
    }
}