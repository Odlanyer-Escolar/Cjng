using System;
namespace Cjng.ViewModels;


public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Hello Avalonia!";
    public string FechaHoy { get; } = "hoy es " + DateTime.Now.ToString("dd/MM/yyyy");
}
