using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Cjng.Contexto;
using Cjng.Models;
using Cjng.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Linq;

namespace Cjng.Views;

public partial class MainWindow : ContentPage
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
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
    .GetMessageBoxStandard("Error", "Usuario no puede estar vacío.", ButtonEnum.Ok);
            await box.ShowAsync();
            IsEnabled= true;
            return;
        }
        if (string.IsNullOrWhiteSpace(txtContrasena.Text))
        {
            var box = MessageBoxManager
    .GetMessageBoxStandard("Error", "Contraseña no puede estar vacía.", ButtonEnum.Ok);
            await box.ShowAsync();
            IsEnabled = true;
            return;
        }

        try
        {
            using var db = new CJNG_ChecadorDB();
            Usuario? admin = db.Usuarios.Where(x => x.usuario == txtUsuario.Text.Trim() && x.Contrasena == txtContrasena.Text.Trim()).FirstOrDefault();
            if (admin == null)
            {
                var box = MessageBoxManager
          .GetMessageBoxStandard("Error", "Credenciales incorrectas. Verifique su usuario y contraseña.", ButtonEnum.Ok);
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
        catch (Exception ex)
        {
            var box = MessageBoxManager
        .GetMessageBoxStandard("Error", $"Se produjo un error: {ex.Message}", ButtonEnum.Ok);
            await box.ShowAsync();
            IsEnabled = true;
        }
    }

    private void Window_Loaded(object? sender, RoutedEventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error en carga de ventana: {ex.Message}");
        }
    }
}
