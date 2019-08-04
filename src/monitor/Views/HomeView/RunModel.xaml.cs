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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace monitor.Views.HomeView
{
    /// <summary>
    /// Interaction logic for RunModel.xaml
    /// </summary>
    public partial class RunModel : UserControl
    {
        private ModeloRepository _modeloRepository = new ModeloRepository();

        Modelo modelo;
        string PID;

        System.Windows.Forms.Screen[] screens;

        int id;

        public RunModel()
        {
            InitializeComponent();
            Loaded += RunModel_Loaded;
        }
        private void RunModel_Loaded(object sender, RoutedEventArgs e)
        {
            Initialize(); 
        }
        private void Initialize()
        {
            cbModelos.ItemsSource = _modeloRepository.GetModelos();
            CargarEstacionesDisponibles();

            //Elegir id de la pantalla 
            if (App.id < 4)
            {
                id = App.id;
                App.id++;
            }
            else
            {
                id = 0;
                App.id = 1;
            }

            //Si la pantalla ya tiene un modelo corriendo, cargar los datos. 
            if (App.modelRunnings.Where(w => w.RunId == id).First().isRunning)
            {
                btnIniciar.Visibility = Visibility.Collapsed;
                grdModeloCorriendo.Visibility = Visibility.Visible;

                modelo = App.modelRunnings.Where(w => w.RunId == id).First().model;
                PID = App.modelRunnings.Where(w => w.RunId == id).First().PID;

                lblModelo.Content = modelo.NumeroModelo;
                lblPID.Content = PID;
            }
        }
        //Pagina 1
        private void BtnIniciar_Click(object sender, RoutedEventArgs e)
        {
           //Muestra grid modelo + PID
            grdIniciarModelo.Visibility = Visibility.Visible;
            //Esconde Boton 1 
            btnIniciar.Visibility = Visibility.Collapsed;
        }
        //Pagina 2
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void BtnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            //Valida campos
            if (cbModelos.SelectedItem == null)
            {
                MessageBox.Show("Seleccione modelo.");
                return;
            }

            if (string.IsNullOrEmpty(txtPID.Text))
            {
                MessageBox.Show("Ingresar número de PID.");
                return;
            } 
             
            PID = txtPID.Text;
            modelo = (Modelo)cbModelos.SelectedItem;

            lblPID.Content = PID;
            lblModelo.Content = modelo.NumeroModelo;
            
            //Esconde grid modelo + PID 
            grdIniciarModelo.Visibility = Visibility.Collapsed;
            //Muestra lista de estaciones
            grdEstaciones.Visibility = Visibility.Visible; 
        }
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            grdIniciarModelo.Visibility = Visibility.Collapsed;
            btnIniciar.Visibility = Visibility.Visible;
        }
        //Pagina 3
        private void EstacionesItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Estacion selectedEstacion = (Estacion)e.AddedItems[0];

                //Validacion para utilizar una estación ya ocupada.
                if (selectedEstacion.Modelo != null)
                {
                    MessageBoxResult boxButton = MessageBox.Show("¿Seguro que desea utilizar una estación que ya esta siendo ocupada?", "Utilizar estación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (boxButton == MessageBoxResult.No)
                    {
                        estacionesItems.SelectedItems.Remove(e.AddedItems[0]);
                    }
                }
            }
        }
        private void BtnCancelar2_Click(object sender, RoutedEventArgs e)
        {
            //Se esconde lista de estaciones
            grdEstaciones.Visibility = Visibility.Collapsed;
            //Se muestra nuevamente grid modelo + PID
            grdIniciarModelo.Visibility = Visibility.Visible;
        }
        private async void BtnIniciarModelo_Click(object sender, RoutedEventArgs e)
        {  
            if (estacionesItems.SelectedItems.Count == 0)
            {
                MessageBox.Show("No selecciono ninguna estación.", "Atención");
                return;
            }

            Loading.Visibility = Visibility.Visible;
            await Task.Delay(100);

            await AbrirEstaciones();

            //Se esconde lista de estaciones
            grdEstaciones.Visibility = Visibility.Collapsed;
            //Se muestra informacion del modelo corriendo
            grdModeloCorriendo.Visibility = Visibility.Visible;
              
            Loading.Visibility = Visibility.Collapsed;
        } 
        //Pagina 4
        private void BtnDetener_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult boxButton = MessageBox.Show("¿Seguro que desea detener el proceso del PID: " + PID + "?", "Detener proceso", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (boxButton == MessageBoxResult.Yes)
            {
                //Buscar estaciones donde el PID esta corriendo. 
                foreach (var estacion in App.estaciones.Where(w => w.PID == PID))
                {
                    estacion.Modelo = null;
                    estacion.Mensaje = "Libre";

                    //Cerrar y eliminar estación.
                    if (App.estacionesWindows.Any(a => a.Estacion.EstacionId == estacion.EstacionId))
                    {
                        App.estacionesWindows.Where(a => a.Estacion.EstacionId == estacion.EstacionId).FirstOrDefault().Close();
                        App.estacionesWindows.Remove(App.estacionesWindows.Where(a => a.Estacion.EstacionId == estacion.EstacionId).FirstOrDefault());
                    }
                }

                //Reiniciar valores.
                txtPID.Text = null;
                cbModelos.SelectedItem = null;

                App.modelRunnings.Where(w => w.RunId == id).First().isRunning = false;
                App.modelRunnings.Where(w => w.RunId == id).First().PID = "";
                App.modelRunnings.Where(w => w.RunId == id).First().model = null;

                estacionesItems.UnselectAll(); 
                CargarEstacionesDisponibles();

                grdModeloCorriendo.Visibility = Visibility.Collapsed;
                grdPiezasTomadas.Visibility = Visibility.Visible;
            }
        }
        //Pagina 5
        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            bool todosCero = true;
            MessageBoxResult boxButton = MessageBox.Show("¿Seguro que desea registrar las piezas tomadas?", "Registrar piezas tomadas", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (boxButton == MessageBoxResult.Yes)
            {
                if (string.IsNullOrEmpty(tbIngenieria.Text))
                {
                    tbIngenieria.Text = "0";
                }
                else
                {
                    todosCero = false;
                }

                if (string.IsNullOrEmpty(tbCalidad.Text))
                {
                    tbCalidad.Text = "0";
                }
                else
                {
                    todosCero = false;
                }

                if (string.IsNullOrEmpty(tbProduccion.Text))
                {
                    tbProduccion.Text = "0";
                }
                else
                {
                    todosCero = false;
                }
                if (todosCero)
                {
                    MessageBox.Show("No es válido realizar registros en cero.", "Información");
                    return;
                }

                RegistrarPiezasTomadas();

                grdPiezasTomadas.Visibility = Visibility.Collapsed;
                btnIniciar.Visibility = Visibility.Visible;
            }
        }
        private void BtnVolverInicio_Click(object sender, RoutedEventArgs e)
        {
            lblPiezasTomadas.Visibility = Visibility.Collapsed;
            grdPiezasTomadas.Visibility = Visibility.Collapsed;
            btnIniciar.Visibility = Visibility.Visible;
        } 
       
        private void CargarEstacionesDisponibles()
        {
            screens = System.Windows.Forms.Screen.AllScreens;

            foreach (var estacion in App.estaciones)
            {
                //Verifica si la estacion tiene su monitor conectado. 
                if (screens.Any(a => a.DeviceName == estacion.Monitor))
                {
                    estacion.isRunning = true;
                }
            }
            //Muestra estaciones disponibles
            estacionesItems.ItemsSource = App.estaciones.Where(a => a.isRunning);
          
        }
        private async Task AbrirEstaciones()
        {
            Loading.Procesando("Abriendo estaciones...");
            await Task.Delay(1000);
            try
            {
                foreach (var est in estacionesItems.SelectedItems)
                {
                    Estacion estacion = (Estacion)est;

                    //Verificar si la ventana de la estacion se encuentra activa.
                    Loading.Procesando($"Verificando estación {estacion.Nombre} ...");
                    await Task.Delay(100);

                    if (App.estacionesWindows.Any(a => a.Estacion.EstacionId == estacion.EstacionId))
                    {
                        //Cerrar y eliminar estación activa.
                        Loading.Procesando($"Cerrando estación {estacion.Nombre} ...");
                        await Task.Delay(100);
                        App.estacionesWindows.Where(a => a.Estacion.EstacionId == estacion.EstacionId).FirstOrDefault().Close();
                        App.estacionesWindows.Remove(App.estacionesWindows.Where(a => a.Estacion.EstacionId == estacion.EstacionId).FirstOrDefault());
                    }

                    //Buscar monitor indicado.
                    Loading.Procesando($"Buscando monitor {estacion.Monitor} ...");
                    await Task.Delay(100);

                    var screen = screens.Where(w => w.DeviceName == estacion.Monitor).FirstOrDefault();
                    estacion.PID = PID;

                    //Crear pantalla de monitoreo.
                    Loading.Procesando($"Abriendo estación {estacion.Nombre} ...");
                    await Task.Delay(100);

                    Monitoreo monitoreoWindow = new Monitoreo(estacion, modelo);

                    monitoreoWindow.Left = screen.WorkingArea.Left;
                    monitoreoWindow.Top = screen.WorkingArea.Top;
                    monitoreoWindow.Width = screen.Bounds.Width;
                    monitoreoWindow.Height = screen.Bounds.Height;
                    monitoreoWindow.WindowState = WindowState.Normal;
                    monitoreoWindow.Show();

                    //Crear pantalla de monitoreo.
                    Loading.Procesando($"Cargando ayudas visuales en estación {estacion.Nombre}...");
                    await Task.Delay(100);

                    monitoreoWindow.InitializeDocumentViewer();
                    App.estacionesWindows.Add(monitoreoWindow);

                    //Indicar que la estación esta corriendo el modelo seleccionado. 
                    App.estaciones.Where(w => w.EstacionId == estacion.EstacionId).FirstOrDefault().Modelo = modelo.NumeroModelo;
                    App.estaciones.Where(w => w.EstacionId == estacion.EstacionId).FirstOrDefault().Mensaje = "Modelo Actual:";

                    Loading.Procesando($"Listo!");
                    await Task.Delay(1000);

                    App.modelRunnings.Where(w => w.RunId == id).First().isRunning = true;
                    App.modelRunnings.Where(w => w.RunId == id).First().model = modelo;
                    App.modelRunnings.Where(w => w.RunId == id).First().PID = PID;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }
        private void RegistrarPiezasTomadas()
        {
            try
            {
                PiezasTomadasRepository piezasTomadasRepository = new PiezasTomadasRepository();
                int ingenieria = int.Parse(tbIngenieria.Text);
                int calidad = int.Parse(tbCalidad.Text);
                int produccion = int.Parse(tbProduccion.Text);

                PiezasTomadas piezasTomadas = new PiezasTomadas()
                {
                    Ingenieria = ingenieria,
                    Calidad = calidad,
                    Produccion = produccion,
                    ModeloId = modelo.ModeloId,
                    FechaHora = DateTime.Now
                };

                piezasTomadasRepository.InsertPiezasTomadas(piezasTomadas);

                MessageBox.Show("Piezas registradas con éxito.");

                tbIngenieria.Text = string.Empty;
                tbCalidad.Text = string.Empty;
                tbProduccion.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al registrar piezas. - Error:" + ex.Message);
            }
        }
       

    }
}
