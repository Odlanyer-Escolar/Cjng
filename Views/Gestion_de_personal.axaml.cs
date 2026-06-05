using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Cjng.Contexto;
using Cjng.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cjng;

public partial class Gestion_de_personal : ContentPage
{
    private ObservableCollection<Trabajador> trabajadores = new();
    private ObservableCollection<Trabajador> trabajadoresFiltrados = new();
    private CJNG_ChecadorDB _db = new();

    public Gestion_de_personal()
    {
        InitializeComponent();
        CargarDatos();
    }

    private void CargarDatos()
    {
        trabajadores.Clear();
        try
        {
            var trabajadoresDb = _db.Trabajadores.ToList();

            // Si no hay datos en la DB, agregar ejemplos
            if (!trabajadoresDb.Any())
            {
                AgregarEjemplos();
                trabajadoresDb = _db.Trabajadores.ToList();
            }

            foreach (var t in trabajadoresDb)
            {
                trabajadores.Add(t);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error cargando datos: {ex.Message}");
            AgregarEjemplos();
        }

        var lstTrabajadores = this.FindControl<ListBox>("lstTrabajadores");
        if (lstTrabajadores != null)
        {
            lstTrabajadores.ItemsSource = trabajadores;
        }

        var cmbEstado = this.FindControl<ComboBox>("cmbEstado");
        if (cmbEstado != null)
        {
            cmbEstado.SelectedIndex = 0;
        }
    }

    private void AgregarEjemplos()
    {
        try
        {
            _db.Trabajadores.Add(new Trabajador { Nombre = "Juan García", Cargo = "Gerente de Operaciones", Salario = 50000, FechaRegistro = DateTime.Now.AddDays(-30), Estado = true });
            _db.Trabajadores.Add(new Trabajador { Nombre = "María López", Cargo = "Contador Principal", Salario = 35000, FechaRegistro = DateTime.Now.AddDays(-60), Estado = true });
            _db.Trabajadores.Add(new Trabajador { Nombre = "Carlos Rodríguez", Cargo = "Asistente Administrativo", Salario = 20000, FechaRegistro = DateTime.Now.AddDays(-15), Estado = true });
            _db.Trabajadores.Add(new Trabajador { Nombre = "Ana Martínez", Cargo = "Jefe de Almacén", Salario = 28000, FechaRegistro = DateTime.Now.AddDays(-45), Estado = true });
            _db.Trabajadores.Add(new Trabajador { Nombre = "David Peña", Cargo = "Contador Junior", Salario = 18000, FechaRegistro = DateTime.Now.AddDays(-5), Estado = false });
            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error agregando ejemplos: {ex.Message}");
        }
    }

    private void btnBuscar_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var txtBuscar = this.FindControl<TextBox>("txtBuscar");
        var cmbEstado = this.FindControl<ComboBox>("cmbEstado");

        string busqueda = txtBuscar?.Text?.ToLower() ?? "";
        string estado = cmbEstado?.SelectedItem?.ToString() ?? "Todos";

        var resultado = trabajadores.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            resultado = resultado.Where(t => t.Nombre.ToLower().Contains(busqueda) || t.Cargo.ToLower().Contains(busqueda));
        }

        if (estado == "Activos")
            resultado = resultado.Where(t => t.Estado);
        else if (estado == "Inactivos")
            resultado = resultado.Where(t => !t.Estado);

        var lstTrabajadores = this.FindControl<ListBox>("lstTrabajadores");
        if (lstTrabajadores != null)
        {
            lstTrabajadores.ItemsSource = new ObservableCollection<Trabajador>(resultado);
        }
    }

    private void lstTrabajadores_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        var lstTrabajadores = this.FindControl<ListBox>("lstTrabajadores");
        if (lstTrabajadores?.SelectedItem is Trabajador trabajador)
        {
            MostrarDetallesTrabajador(trabajador);
        }
    }

    private void MostrarDetallesTrabajador(Trabajador trabajador)
    {
        var stkSinSeleccionar = this.FindControl<StackPanel>("stkSinSeleccionar");
        var stkConDetalles = this.FindControl<StackPanel>("stkConDetalles");

        if (stkSinSeleccionar != null && stkConDetalles != null)
        {
            stkSinSeleccionar.IsVisible = false;
            stkConDetalles.IsVisible = true;
        }

        this.FindControl<TextBlock>("lblNombre").Text = trabajador.Nombre;
        this.FindControl<TextBlock>("lblId").Text = trabajador.Id.ToString();
        this.FindControl<TextBlock>("lblCargo").Text = trabajador.Cargo;
        this.FindControl<TextBlock>("lblSalario").Text = $"${trabajador.Salario:N2}";
        this.FindControl<TextBlock>("lblFechaRegistro").Text = trabajador.FechaRegistro.ToString("dd/MM/yyyy");
        this.FindControl<TextBlock>("lblEstado").Text = trabajador.Estado ? "✓ Activo" : "✗ Inactivo";
        this.FindControl<TextBlock>("lblEstado").Foreground = trabajador.Estado ? new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Green) : new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red);
    }

    private async void btnVolver_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        IsEnabled = false;
        await Navigation.PopAsync();
        IsEnabled = true;
    }
}
