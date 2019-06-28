using monitor.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace monitor.Views.HomeView
{
    /// <summary>
    /// Interaction logic for StopModel.xaml
    /// </summary>
    public partial class StopModel : Page
    {
        private EstacionRepository _estacionRepository;

        List<Estacion> estaciones;
        List<Monitoreo> estacionesWindows = new List<Monitoreo>();
        public StopModel()
        {
            InitializeComponent();
            Loaded += StopModel_Loaded;

            _estacionRepository = new EstacionRepository();
            estaciones = _estacionRepository.GetEstaciones(); 
        }
        private void StopModel_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.isRunning)
            {
                lblModelo.Content = App.modelo.NumeroModelo;
                lblPID.Content = App.PID;

                StartMonitoringAsync();
            } 
        }

        private async void StartMonitoringAsync()
        {
            App.isRunning = true;
            var screens = Screen.AllScreens;

            foreach (var estacion in estaciones)
            {
                //To do: Buscar la forma de seleccionar la pantalla por configuracion. 
                var screen = screens.Where(w => w.DeviceName == estacion.Monitor).FirstOrDefault();

                if (screen != null)
                {
                    Monitoreo monitoreoWindow = new Monitoreo(estacion.Nombre);
                    monitoreoWindow.Left = screen.WorkingArea.Left;
                    monitoreoWindow.Top = screen.WorkingArea.Top;
                    monitoreoWindow.Width = screen.Bounds.Width;
                    monitoreoWindow.Height = screen.Bounds.Height;
                    monitoreoWindow.WindowState = WindowState.Normal;
                     
                    monitoreoWindow.Show();

                    estacionesWindows.Add(monitoreoWindow);
                }
                await Task.Delay(100);
            }
        }

        private void StopMonitoring()
        {
            //To do: Detener proceso
            App.isRunning = false;

            foreach (var estacion in estacionesWindows)
            {
                estacion.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult boxButton = MessageBox.Show("¿Seguro que desea detener el proceso?", "Detener proceso", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (boxButton == MessageBoxResult.Yes)
            {
                StopMonitoring();

                FinishModel page = new FinishModel();
                NavigationService.Navigate(page);
            }
        }
    }
}
