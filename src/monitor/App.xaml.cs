using monitor.Data;
using monitor.Views;
using monitor.Views.HomeView;
using System;
using System.Collections.Generic;
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
        private static EstacionRepository _estacionRepository = new EstacionRepository();

        public static Usuario usuario;   

        public static List<Estacion> estaciones;
        public static List<Monitoreo> estacionesWindows = new List<Monitoreo>();

        public static Dictionary<int,bool> modelsRunning = new Dictionary<int,bool>();
        public static Dictionary<int, Modelo> models = new Dictionary<int, Modelo>();
        public static Dictionary<int, string> PIDs = new Dictionary<int, string>();

        public static int id;


        public App()
        {  
            id = 0;

            modelsRunning.Add(0, false);
            modelsRunning.Add(1, false);
            modelsRunning.Add(2, false);
            modelsRunning.Add(3, false);

            models.Add(0, null);
            models.Add(1, null);
            models.Add(2, null);
            models.Add(3, null);

            PIDs.Add(0, "");
            PIDs.Add(1, "");
            PIDs.Add(2, "");
            PIDs.Add(3, "");


            usuario = new Usuario()
            {
                Activo = 1,
                Estatus = 1,
                TipoEmpleado = 1
            };
        }

        public static void CargarEstaciones()
        {
            estaciones = _estacionRepository.GetEstaciones();
        } 
    }
}
