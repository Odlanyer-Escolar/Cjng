using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Cjng.Contexto;
using Cjng.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cjng;

public partial class Reportes : ContentPage
{
    private string tipoReporteActual = "";
    private ObservableCollection<string> datosReporte = new();
    private CJNG_ChecadorDB _db = new();

    public Reportes()
    {
        InitializeComponent();
        InicializarDatos();
    }

    private void InicializarDatos()
    {
        // Asegurar que hay datos en la BD
        try
        {
            if (_db.Trabajadores.Count() < 10)
            {
                AgregarMasEmpleados();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error inicializando datos: {ex.Message}");
        }
    }

    private void AgregarMasEmpleados()
    {
        try
        {
            var empleadosExistentes = _db.Trabajadores.Count();
            if (empleadosExistentes == 0)
            {
                // Empleados iniciales
                _db.Trabajadores.Add(new Trabajador { Nombre = "Juan García", Cargo = "Gerente de Operaciones", Salario = 50000, FechaRegistro = DateTime.Now.AddDays(-30), Estado = true });
                _db.Trabajadores.Add(new Trabajador { Nombre = "María López", Cargo = "Contador Principal", Salario = 35000, FechaRegistro = DateTime.Now.AddDays(-60), Estado = true });
                _db.Trabajadores.Add(new Trabajador { Nombre = "Carlos Rodríguez", Cargo = "Asistente Administrativo", Salario = 20000, FechaRegistro = DateTime.Now.AddDays(-15), Estado = true });
                _db.Trabajadores.Add(new Trabajador { Nombre = "Ana Martínez", Cargo = "Jefe de Almacén", Salario = 28000, FechaRegistro = DateTime.Now.AddDays(-45), Estado = true });
                _db.Trabajadores.Add(new Trabajador { Nombre = "David Peña", Cargo = "Contador Junior", Salario = 18000, FechaRegistro = DateTime.Now.AddDays(-5), Estado = false });
            }

            // Agregar más empleados si no existen
            if (empleadosExistentes < 10)
            {
                _db.Trabajadores.Add(new Trabajador { Nombre = "Laura Fernández", Cargo = "Especialista en RR.HH", Salario = 32000, FechaRegistro = DateTime.Now.AddDays(-90), Estado = true });
                _db.Trabajadores.Add(new Trabajador { Nombre = "Roberto Silva", Cargo = "Supervisor de Ventas", Salario = 38000, FechaRegistro = DateTime.Now.AddDays(-20), Estado = true });
                _db.Trabajadores.Add(new Trabajador { Nombre = "Patricia González", Cargo = "Coordinadora de Proyectos", Salario = 36000, FechaRegistro = DateTime.Now.AddDays(-40), Estado = true });
                _db.Trabajadores.Add(new Trabajador { Nombre = "Miguel Hernández", Cargo = "Operario de Producción", Salario = 22000, FechaRegistro = DateTime.Now.AddDays(-25), Estado = true });
                _db.Trabajadores.Add(new Trabajador { Nombre = "Sofía Romero", Cargo = "Asistente de Calidad", Salario = 21000, FechaRegistro = DateTime.Now.AddDays(-10), Estado = true });
            }

            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error agregando empleados: {ex.Message}");
        }
    }

    private void btnReporteAsistencia_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        tipoReporteActual = "Asistencias";
        var lblTitle = this.FindControl<TextBlock>("lblReporteTitle");
        if (lblTitle != null)
            lblTitle.Text = "📊 Reporte de Asistencias";
        ActualizarVistaPreliminar();
    }

    private void btnReporteSalarios_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        tipoReporteActual = "Salarios";
        var lblTitle = this.FindControl<TextBlock>("lblReporteTitle");
        if (lblTitle != null)
            lblTitle.Text = "💰 Reporte de Salarios";
        ActualizarVistaPreliminar();
    }

    private void btnReporteEmpleados_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        tipoReporteActual = "Personal";
        var lblTitle = this.FindControl<TextBlock>("lblReporteTitle");
        if (lblTitle != null)
            lblTitle.Text = "👥 Reporte de Personal";
        ActualizarVistaPreliminar();
    }

    private void btnReporteGeneral_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        tipoReporteActual = "General";
        var lblTitle = this.FindControl<TextBlock>("lblReporteTitle");
        if (lblTitle != null)
            lblTitle.Text = "📈 Reporte General";
        ActualizarVistaPreliminar();
    }

    private void btnGenerarReporte_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(tipoReporteActual))
        {
            var content = this.FindControl<TextBlock>("lblReporteContent");
            if (content != null)
                content.Text = "⚠️ Seleccione un tipo de reporte antes de generar.";
            return;
        }

        var fechaInicio = this.FindControl<TextBox>("txtFechaInicio")?.Text ?? "";
        var fechaFin = this.FindControl<TextBox>("txtFechaFin")?.Text ?? "";

        GenerarDatosReporte(tipoReporteActual, fechaInicio, fechaFin);
    }

    private void ActualizarVistaPreliminar()
    {
        var content = this.FindControl<TextBlock>("lblReporteContent");
        if (content != null)
        {
            content.Text = $"✓ Reporte de {tipoReporteActual} cargado.\n\n" +
                          "Ingrese el rango de fechas y haga clic en 'GENERAR' para ver los datos completos.";
        }
    }

    private void GenerarDatosReporte(string tipo, string fechaInicio, string fechaFin)
    {
        datosReporte.Clear();

        try
        {
            var empleados = _db.Trabajadores.ToList();

            switch (tipo)
            {
                case "Asistencias":
                    datosReporte.Add("--- REPORTE DE ASISTENCIAS ---");
                    datosReporte.Add("Período: " + (string.IsNullOrEmpty(fechaInicio) ? "Todo el tiempo" : $"{fechaInicio} - {fechaFin}"));
                    datosReporte.Add("");
                    datosReporte.Add("ID | Nombre | Cargo | Entradas | Salidas | Asistencia %");
                    datosReporte.Add("=" + new string('=', 100));

                    int id = 1;
                    foreach (var emp in empleados.Where(e => e.Estado))
                    {
                        int entradas = 20 + id;
                        int salidas = 20 + id;
                        int asistencia = 90 + (id % 3);
                        datosReporte.Add($"{id} | {emp.Nombre.PadRight(20)} | {emp.Cargo.PadRight(25)} | {entradas} | {salidas} | {asistencia}%");
                        id++;
                    }

                    datosReporte.Add("");
                    datosReporte.Add($"Total de Empleados Activos: {empleados.Count(e => e.Estado)}");
                    datosReporte.Add($"Asistencia Promedio: 94.2%");
                    break;

                case "Salarios":
                    datosReporte.Add("--- REPORTE DE SALARIOS ---");
                    datosReporte.Add("Período: " + (string.IsNullOrEmpty(fechaInicio) ? "Mes Actual" : $"{fechaInicio} - {fechaFin}"));
                    datosReporte.Add("");
                    datosReporte.Add("ID | Nombre | Cargo | Salario Mensual | Acumulado (3m) | Descuentos");
                    datosReporte.Add("=" + new string('=', 110));

                    double totalNomina = 0;
                    double totalDescuentos = 0;
                    id = 1;

                    foreach (var emp in empleados.Where(e => e.Estado))
                    {
                        double acumulado = emp.Salario * 3;
                        double descuentos = emp.Salario * 0.06; // 6% de descuentos
                        totalNomina += emp.Salario;
                        totalDescuentos += descuentos;

                        datosReporte.Add($"{id} | {emp.Nombre.PadRight(20)} | {emp.Cargo.PadRight(25)} | ${emp.Salario:N0} | ${acumulado:N0} | ${descuentos:N0}");
                        id++;
                    }

                    datosReporte.Add("");
                    datosReporte.Add($"TOTAL NÓMINA MENSUAL: ${totalNomina:N0}");
                    datosReporte.Add($"TOTAL DESCUENTOS: ${totalDescuentos:N0}");
                    datosReporte.Add($"TOTAL A PAGAR: ${totalNomina - totalDescuentos:N0}");
                    break;

                case "Personal":
                    datosReporte.Add("--- REPORTE DE PERSONAL ---");
                    datosReporte.Add("Fecha de generación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    datosReporte.Add("");
                    datosReporte.Add("ID | Nombre | Cargo | Estado | Fecha Registro | Días Laborando");
                    datosReporte.Add("=" + new string('=', 110));

                    id = 1;
                    foreach (var emp in empleados)
                    {
                        int diasLaborando = (int)(DateTime.Now - emp.FechaRegistro).TotalDays;
                        string estado = emp.Estado ? "✓ ACTIVO" : "✗ INACTIVO";
                        datosReporte.Add($"{id} | {emp.Nombre.PadRight(20)} | {emp.Cargo.PadRight(25)} | {estado.PadRight(12)} | {emp.FechaRegistro:dd/MM/yyyy} | {diasLaborando}");
                        id++;
                    }

                    datosReporte.Add("");
                    int activos = empleados.Count(e => e.Estado);
                    int inactivos = empleados.Count(e => !e.Estado);
                    datosReporte.Add($"TOTAL EMPLEADOS: {empleados.Count} ({activos} Activos, {inactivos} Inactivos)");
                    break;

                case "General":
                    datosReporte.Add("--- REPORTE GENERAL DEL SISTEMA ---");
                    datosReporte.Add("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    datosReporte.Add("");
                    datosReporte.Add("📊 ESTADÍSTICAS GENERALES:");
                    datosReporte.Add($"  • Total de Empleados: {empleados.Count}");

                    int actCount = empleados.Count(e => e.Estado);
                    int inactCount = empleados.Count(e => !e.Estado);
                    datosReporte.Add($"  • Empleados Activos: {actCount}");
                    datosReporte.Add($"  • Empleados Inactivos: {inactCount}");
                    datosReporte.Add($"  • Asistencia Promedio: 94.2%");

                    double nomina = empleados.Where(e => e.Estado).Sum(e => e.Salario);
                    datosReporte.Add($"  • Nómina Total Mensual: ${nomina:N0}");

                    datosReporte.Add("");
                    datosReporte.Add("💰 RESUMEN FINANCIERO:");
                    double descuentoTotal = nomina * 0.06;
                    datosReporte.Add($"  • Gasto en Nómina (30 días): ${nomina:N0}");
                    datosReporte.Add($"  • Descuentos Aplicados: ${descuentoTotal:N0}");
                    datosReporte.Add($"  • Total a Pagar: ${nomina - descuentoTotal:N0}");

                    datosReporte.Add("");
                    datosReporte.Add("👥 DISTRIBUCIÓN POR CARGO:");
                    var cargosCont = empleados.GroupBy(e => e.Cargo).OrderByDescending(g => g.Count());
                    foreach (var grupo in cargosCont)
                    {
                        datosReporte.Add($"  • {grupo.Key}: {grupo.Count()} empleado(s)");
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            datosReporte.Add($"Error generando reporte: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
        }

        var lstData = this.FindControl<ListBox>("lstReporteData");
        if (lstData != null)
        {
            lstData.ItemsSource = datosReporte;
        }
    }

    private void btnDescargarPDF_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("Descargando PDF...");
    }

    private void btnDescargarExcel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("Descargando Excel...");
    }

    private async void btnVolver_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        IsEnabled = false;
        await Navigation.PopAsync();
        IsEnabled = true;
    }
}
