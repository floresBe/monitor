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
        private EstacionRepository _estacionRepository;

        public static Usuario usuario;   

        public static List<Estacion> estaciones;
        public static List<Monitoreo> estacionesWindows = new List<Monitoreo>();

        public App()
        {
            _estacionRepository = new EstacionRepository();
            estaciones = _estacionRepository.GetEstaciones();

            usuario = new Usuario()
            {
                Activo = 1,
                Estatus = 1,
                TipoEmpleado = 1
            };
        }
    }
}
