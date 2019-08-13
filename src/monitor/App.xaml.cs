using monitor.Data;
using monitor.Views;
using monitor.Views.HomeView;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public static List<ModelRunning> modelsRunning = new List<ModelRunning>();
        public static int id;

        public static System.Windows.Forms.Screen[] screens;

        public App()
        {
            CargarEstaciones();

            id = 0;
            modelsRunning.Add(new ModelRunning { RunId = 0, isRunning = false, model = null, PID = "" });
            modelsRunning.Add(new ModelRunning { RunId = 1, isRunning = false, model = null, PID = "" });
            modelsRunning.Add(new ModelRunning { RunId = 2, isRunning = false, model = null, PID = "" });
            modelsRunning.Add(new ModelRunning { RunId = 3, isRunning = false, model = null, PID = "" });

            usuario = new Usuario()
            {
                Activo = 1,
                Estatus = 1,
                TipoEmpleado = 1
            };
        }

        public static void CargarEstaciones()
        {
            try
            {
                estaciones = _estacionRepository.GetEstaciones();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al conectar con el servidor. Revise la cadena de conexión.");
            }
        }

        public static void CargarMonitores()
        {
            screens = System.Windows.Forms.Screen.AllScreens;
        }

        public static void CargarEstacionesDisponibles()
        {
            CargarMonitores();

            foreach (var estacion in estaciones)
            {
                //Verifica si la estacion tiene su monitor conectado. 
                if (screens.Any(a => a.DeviceName == estacion.Monitor))
                {
                    estacion.isRunning = true;
                     
                    //Indica si esta corriendo un modelo o no.
                    if (estacion.Modelo != null)
                    {
                        estacion.Mensaje = "Modelo Actual:";
                    }
                    else
                    {
                        estacion.Mensaje = "Libre";
                    }
                } 

                
            }
        }
    }
}
