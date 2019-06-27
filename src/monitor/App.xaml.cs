using monitor.Data;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace monitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Usuario usuario;
        public static Modelo modelo;
        public static string PID;

        public App()
        {
            modelo = new Modelo()
            {
                NumeroModelo = "LM89769",
                RutaAyudaVisual = @"C:\Users\Flores\Dropbox\Monitor\",
                Routing = 12.3
            };

            usuario = new Usuario()
            {
                Activo = 1,
                Estatus = 1,
                TipoEmpleado = 1
            };
        }
    }
}
