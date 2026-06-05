using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;

namespace Cjng;

public partial class Menu : ContentPage
{
    public Menu()
    {
        InitializeComponent();
    }

    private async void BotonGestion_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        IsEnabled = false;
        await Navigation.PushAsync(new Gestion_de_personal());
        IsEnabled = true;
    }

    private async void BotonAdministrador_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        IsEnabled = false;
        await Navigation.PushAsync(new Administrador());
        IsEnabled = true;
    }

    private async void BotonControl_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        IsEnabled = false;
        await Navigation.PushAsync(new Control_De_Acceso());
        IsEnabled = true;
    }

    private async void BotonReportes_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        IsEnabled = false;
        await Navigation.PushAsync(new Reportes());
        IsEnabled = true;
    }

    private async void btnCerrarSesion_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        IsEnabled = false;
        await Navigation.PopAsync();
        IsEnabled = true;
    }
}

