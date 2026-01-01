using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.ViewModels;

namespace UI.Views;

public partial class NewsPage : ContentPage
{
    
    
    public NewsPage(NewsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}