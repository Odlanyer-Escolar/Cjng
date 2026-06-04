using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Cjng.Contexto;
using Cjng.Models;
using Cjng.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Linq;

namespace Cjng.Views;

public partial class MainWindow : ContentPage
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void TextBox_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
    }

    private async void btnIniciarSesion_Click(object? sender, RoutedEventArgs e)
    {
        IsEnabled = false;
        if (string.IsNullOrWhiteSpace(txtUsuario.Text))
        {
            var box = MessageBoxManager
    .GetMessageBoxStandard("Error", "Texto usuario no valido.", ButtonEnum.Ok);
            await box.ShowAsync();
            IsEnabled= true;
            return;
        }
        if (string.IsNullOrWhiteSpace(txtContrasena.Text))
        {
            var box = MessageBoxManager
    .GetMessageBoxStandard("Error", "Texto contraseña no valido.", ButtonEnum.Ok);
            await box.ShowAsync();
            IsEnabled = true;
            return;
        }

        using var db = new CJNG_ChecadorDB();
        Usuario? admin = db.Usuarios.Where(x => x.usuario == txtUsuario.Text.Trim() && x.Contrasena == txtContrasena.Text.Trim()).FirstOrDefault();
        if (admin == null)
        {
            var box = MessageBoxManager
  .GetMessageBoxStandard("Error", "Credenciales erroneas", ButtonEnum.Ok);
            await box.ShowAsync();
            IsEnabled = true;
            return;
        }
        else
        {
            Persistencia.UsuarioActual = admin;
            IsEnabled = true;
            await Navigation.PushAsync(new Menu());

        }

    }

    private void Window_Loaded(object? sender, RoutedEventArgs e)
    {
        using var db = new CJNG_ChecadorDB();
        Usuario? admin = db.Usuarios.Where(x => x.usuario == "Admin").FirstOrDefault();
        if (admin == null)
        {
            db.Usuarios.Add(new Usuario()
            {
                usuario = "Admin",
                Rol = RolUsuario.Admin,
                Contrasena = "0000",
            });
            db.SaveChanges();
        }
    }

    private void btnIniciarSesion_Click_1(object? sender, RoutedEventArgs e)
    {
    }
}