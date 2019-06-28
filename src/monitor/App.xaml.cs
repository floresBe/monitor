using monitor.Data;
using monitor.Views.HomeView;
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
        public static bool isRunning;
        public static StopModel stopModelPage;
        public App()
        { 
            usuario = new Usuario()
            {
                Activo = 1,
                Estatus = 1,
                TipoEmpleado = 1
            };
        }
    }
}
