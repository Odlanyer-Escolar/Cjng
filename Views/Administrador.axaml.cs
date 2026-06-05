using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Cjng.Contexto;
using Cjng.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cjng;

public partial class Administrador : ContentPage
{
    private ObservableCollection<Trabajador> trabajadores = new();
    private CJNG_ChecadorDB _db = new();

    public Administrador()
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

        var cmbTrabajadores = this.FindControl<ComboBox>("cmbTrabajadores");
        if (cmbTrabajadores != null)
        {
            cmbTrabajadores.ItemsSource = trabajadores;
            if (trabajadores.Count > 0)
                cmbTrabajadores.SelectedIndex = 0;
        }

        var cmbEstadoNuevo = this.FindControl<ComboBox>("cmbEstadoNuevo");
        if (cmbEstadoNuevo != null)
            cmbEstadoNuevo.SelectedIndex = 0;
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
            _db.Trabajadores.Add(new Trabajador { Nombre = "Laura Fernández", Cargo = "Especialista en RR.HH", Salario = 32000, FechaRegistro = DateTime.Now.AddDays(-90), Estado = true });
            _db.Trabajadores.Add(new Trabajador { Nombre = "Roberto Silva", Cargo = "Supervisor de Ventas", Salario = 38000, FechaRegistro = DateTime.Now.AddDays(-20), Estado = true });
            _db.Trabajadores.Add(new Trabajador { Nombre = "Patricia González", Cargo = "Coordinadora de Proyectos", Salario = 36000, FechaRegistro = DateTime.Now.AddDays(-40), Estado = true });
            _db.Trabajadores.Add(new Trabajador { Nombre = "Miguel Hernández", Cargo = "Operario de Producción", Salario = 22000, FechaRegistro = DateTime.Now.AddDays(-25), Estado = true });
            _db.Trabajadores.Add(new Trabajador { Nombre = "Sofía Romero", Cargo = "Asistente de Calidad", Salario = 21000, FechaRegistro = DateTime.Now.AddDays(-10), Estado = true });
            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error agregando ejemplos: {ex.Message}");
        }
    }

    private void btnAgregarEmpleado_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var txtNombre = this.FindControl<TextBox>("txtNombreNuevo");
        var txtCargo = this.FindControl<TextBox>("txtCargoNuevo");
        var txtSalario = this.FindControl<TextBox>("txtSalarioNuevo");

        if (string.IsNullOrWhiteSpace(txtNombre?.Text) || string.IsNullOrWhiteSpace(txtCargo?.Text))
        {
            System.Diagnostics.Debug.WriteLine("❌ Complete los campos requeridos");
            return;
        }

        if (!double.TryParse(txtSalario?.Text, out double salario))
        {
            System.Diagnostics.Debug.WriteLine("❌ Ingrese un salario válido");
            return;
        }

        try
        {
            var nuevoTrabajador = new Trabajador
            {
                Nombre = txtNombre.Text,
                Cargo = txtCargo.Text,
                Salario = salario,
                FechaRegistro = DateTime.Now,
                Estado = true
            };

            _db.Trabajadores.Add(nuevoTrabajador);
            _db.SaveChanges();

            trabajadores.Add(nuevoTrabajador);
            btnLimpiar_Click(null, e);
            System.Diagnostics.Debug.WriteLine("✓ Empleado agregado exitosamente");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ Error al agregar empleado: {ex.Message}");
        }
    }

    private void btnLimpiar_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.FindControl<TextBox>("txtNombreNuevo").Text = "";
        this.FindControl<TextBox>("txtCargoNuevo").Text = "";
        this.FindControl<TextBox>("txtSalarioNuevo").Text = "";
        this.FindControl<TextBox>("txtFechaRegistro").Text = "";
    }

    private void btnEditar_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var cmbTrabajadores = this.FindControl<ComboBox>("cmbTrabajadores");
        if (cmbTrabajadores?.SelectedItem is Trabajador trabajador)
        {
            var txtCargo = this.FindControl<TextBox>("txtCargoEditar");
            var txtSalario = this.FindControl<TextBox>("txtSalarioEditar");

            if (!string.IsNullOrEmpty(txtCargo?.Text))
                trabajador.Cargo = txtCargo.Text;

            if (double.TryParse(txtSalario?.Text, out double salario))
                trabajador.Salario = salario;

            System.Diagnostics.Debug.WriteLine("✓ Empleado actualizado exitosamente");
        }
    }

    private void btnEliminar_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var cmbTrabajadores = this.FindControl<ComboBox>("cmbTrabajadores");
        if (cmbTrabajadores?.SelectedItem is Trabajador trabajador)
        {
            try
            {
                var trabajadorDb = _db.Trabajadores.FirstOrDefault(t => t.Id == trabajador.Id);
                if (trabajadorDb != null)
                {
                    _db.Trabajadores.Remove(trabajadorDb);
                    _db.SaveChanges();
                }

                trabajadores.Remove(trabajador);
                System.Diagnostics.Debug.WriteLine("✓ Empleado eliminado exitosamente");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error al eliminar empleado: {ex.Message}");
            }
        }
    }

    private async void btnVolver_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        IsEnabled = false;
        await Navigation.PopAsync();
        IsEnabled = true;
    }
}
