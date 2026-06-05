using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Cjng.Contexto;
using Cjng.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cjng;

public class RegistroAcceso
{
    public string Trabajador { get; set; } = "";
    public DateTime Hora { get; set; } = DateTime.Now;
    public string Tipo { get; set; } = "";

    public override string ToString() => $"{Trabajador} - {Tipo} ({Hora:HH:mm:ss})";
}

public partial class Control_De_Acceso : ContentPage
{
    private ObservableCollection<RegistroAcceso> historialAccesos = new();
    private ObservableCollection<Trabajador> trabajadores = new();
    private CJNG_ChecadorDB _db = new();

    public Control_De_Acceso()
    {
        InitializeComponent();
        CargarDatos();
        ActualizarHora();
    }

    private void CargarDatos()
    {
        try
        {
            var trabajadoresDb = _db.Trabajadores.ToList();

            // Si no hay datos, agregar ejemplos
            if (!trabajadoresDb.Any())
            {
                _db.Trabajadores.Add(new Trabajador { Nombre = "Juan García", Cargo = "Gerente de Operaciones", Salario = 50000, Estado = true });
                _db.Trabajadores.Add(new Trabajador { Nombre = "María López", Cargo = "Contador", Salario = 35000, Estado = true });
                _db.Trabajadores.Add(new Trabajador { Nombre = "Carlos Rodríguez", Cargo = "Asistente", Salario = 20000, Estado = true });
                _db.SaveChanges();
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
        }

        var cmbTrabajadores = this.FindControl<ComboBox>("cmbTrabajadores");
        if (cmbTrabajadores != null)
        {
            cmbTrabajadores.ItemsSource = trabajadores;
            if (trabajadores.Count > 0)
                cmbTrabajadores.SelectedIndex = 0;
        }

        var lstHistorial = this.FindControl<ListBox>("lstHistorial");
        if (lstHistorial != null)
            lstHistorial.ItemsSource = historialAccesos;
    }

    private void ActualizarHora()
    {
        var txtHora = this.FindControl<TextBlock>("txtHoraActual");
        if (txtHora != null)
        {
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += (s, e) =>
            {
                txtHora.Text = DateTime.Now.ToString("HH:mm:ss");
            };
            dispatcherTimer.Start();
        }
    }

    private void btnEntrada_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RegistrarAcceso("ENTRADA");
    }

    private void btnSalida_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RegistrarAcceso("SALIDA");
    }

    private void RegistrarAcceso(string tipo)
    {
        var cmbTrabajadores = this.FindControl<ComboBox>("cmbTrabajadores");
        if (cmbTrabajadores?.SelectedItem is Trabajador trabajador)
        {
            var registro = new RegistroAcceso
            {
                Trabajador = trabajador.Nombre,
                Hora = DateTime.Now,
                Tipo = tipo
            };
            historialAccesos.Insert(0, registro);
        }
    }

    private async void btnVolver_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        IsEnabled = false;
        await Navigation.PopAsync();
        IsEnabled = true;
    }
}
