using DocumentFormat.OpenXml.Packaging;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using monitor.Data;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;
using Application = Microsoft.Office.Interop.PowerPoint.Application;

namespace monitor.Views
{
    /// <summary>
    /// Interaction logic for Monitoreo.xaml
    /// </summary>
    public partial class Monitoreo : Window
    {
        PiezaRepository PiezaRepository = new PiezaRepository();

        Estacion Estacion;

        string Modelo;
        double? Routing;

        int piezasHoraActual;
        int piezasHoraAnterior;

        int piezasBuenas;
        int piezasMalas;

        int pages;
        int page;
        string URL;
        DateTime timeCycle;
          
        public Monitoreo()
        {
            InitializeComponent();
            Initialize();
        }

        public Monitoreo(Estacion estacion)
        {
            Estacion = estacion;

            InitializeComponent();
            Initialize();
        }
        private void Initialize()
        {
            PiezaRepository = new PiezaRepository(); 

            InitializeHeader();
            InitializeDocumentViwer();
            InitializeTimerCycle();
            InitializeTimerCurrentTime();
        }
        private void InitializeHeader()
        {
            Modelo = App.modelo.NumeroModelo;
            lblModelo.Content = Modelo;

            Routing = App.modelo.Routing;
            lblRouting.Content = Routing;

            URL = App.modelo.RutaAyudaVisual;

            lblEstacion.Content = Estacion.Nombre;
        }
        private void InitializeDocumentViwer()
        {
            try
            {
                string powerPointFile = URL + Estacion.Nombre + ".ppt";

                var xpsFile = System.IO.Path.GetTempPath() + Guid.NewGuid() + Estacion.Nombre + ".ppsx";
                var xpsDocument = ConvertPowerPointToXps(powerPointFile, xpsFile);

                DocumentviewPowerPoint.Document = xpsDocument.GetFixedDocumentSequence();
                page = 1;
                pages = DocumentviewPowerPoint.PageCount;

                InitializeTimerDocumentViewer();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar archivo ppt - Error: " + ex.Message);
            }
        }
        private static XpsDocument ConvertPowerPointToXps(string pptFilename, string xpsFilename)
        {
            //New Application Power Point 
            var powerPointApp = new Application();
            //Open the presentation (Invisible)
            var presentation = powerPointApp.Presentations.Open(pptFilename, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);

            try
            {
                presentation.ExportAsFixedFormat(xpsFilename, PpFixedFormatType.ppFixedFormatTypeXPS, PpFixedFormatIntent.ppFixedFormatIntentScreen, MsoTriState.msoCTrue);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to export to XPS format: " + ex);
            }
            finally
            {  //Close the presentation without saving changes and quit PowerPoint
                presentation.Close();
                powerPointApp.Quit();
            }

            return new XpsDocument(xpsFilename, FileAccess.Read);
        }
        private void InitializeTimerDocumentViewer()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(NextPageTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();
        }
        private void InitializeTimerCycle()
        {
            InitializeTimeCycle();

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(CycleTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
        private void InitializeTimeCycle()
        {
            timeCycle = DateTime.Today;
        }
        private void InitializeTimerCurrentTime()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(CurrentTimeTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 30);
            dispatcherTimer.Start();
        }


        private void InPLCData(string modelo, string state)
        {
            if(modelo != App.modelo.ModeloId)
            {
                WarningMessageGrid.Visibility = Visibility.Visible;
            }
            else
            {
                WarningMessageGrid.Visibility = Visibility.Collapsed;
            }

            AddPieza(int.Parse(state));
        }
        private void AddPieza(int state)
        {
            try
            {
                Pieza pieza = new Pieza()
                {
                    EstacionId = Estacion.EstacionId,
                    Estado = state,
                    ModeloId = App.modelo.ModeloId,
                    PID = int.Parse(App.PID),
                    TiempoCiclo = lblTiempoCiclo.Content.ToString(),
                    FechaHora = DateTime.Now
                };

                PiezaRepository.InsertPieza(pieza);

                InitializeTimeCycle();

                piezasHoraActual += 1;
                lblPiezasHoraActual.Content = piezasHoraActual;

                if (state == 1)
                {
                    piezasBuenas += 1;
                    lblBuenasHoraActual.Content = piezasBuenas;
                    return;
                }

                piezasMalas += 1;
                lblMalasHoraActual.Content = piezasMalas;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al capturar pieza. - Error: " + ex.Message);
            }
        }
        private void NextPageTimer_Tick(object sender, EventArgs e)
        {
            if (page < pages)
            {
                DocumentviewPowerPoint.NextPage();
                page++;
                return;
            }

            DocumentviewPowerPoint.FirstPage();
            page = 1;
        }
        private void CycleTimer_Tick(object sender, EventArgs e)
        {
            lblTiempoCiclo.Content = timeCycle.ToString("HH:mm:ss");
            timeCycle = timeCycle.AddSeconds(1);
        }
        private void CurrentTimeTimer_Tick(object sender, EventArgs e)
        {
            piezasHoraAnterior = piezasHoraActual;
            piezasHoraActual = 0;

            piezasBuenas = 0;
            lblBuenasHoraActual.Content = piezasBuenas;

            piezasMalas = 0;
            lblMalasHoraActual.Content = piezasMalas;

            lblPiezasHoraActual.Content = piezasHoraActual;
            lblPiezasHoraAnterior.Content = piezasHoraAnterior;
        } 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddPieza(1);

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddPieza(0);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            InPLCData("A","1");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            InPLCData(App.modelo.ModeloId, "0");
        }
    }
}
