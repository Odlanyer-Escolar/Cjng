using System;
namespace Cjng.ViewModels;


public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Hello Avalonia!";
//Si me salió el binding de la fecha jajaja 😂
    public string FechaHoy { get; } = "hoy es " + DateTime.Now.ToString("dd/MM/yyyy");
}
