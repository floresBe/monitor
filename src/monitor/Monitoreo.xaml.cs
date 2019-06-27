using DocumentFormat.OpenXml.Packaging;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
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

namespace monitor
{
    /// <summary>
    /// Interaction logic for Monitoreo.xaml
    /// </summary>
    public partial class Monitoreo : Window
    {
        string Estacion;

        string Modelo;
        double? Routing;

        int piezasHoraActual;
        int piezasHoraAnterior;

        int piezasBuenas;
        int piezasMalas;

        int pages;
        int page;
        string URL;

        string sDateCycle;
        DateTime timeCycle;

        public Monitoreo()
        {
            InitializeComponent();
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

            // to Do: Recibir nombre de estacion por parametro.
            Estacion = "AyudaVisual" ;
            lblEstacion.Content = Estacion;  
        }
        private void InitializeDocumentViwer()
        {
            try
            {
                string powerPointFile = URL + Estacion + ".ppt";

                var xpsFile = System.IO.Path.GetTempPath() + Guid.NewGuid() + ".ppsx";
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

        private void AddPieza(int state)
        {
            InitializeTimeCycle();

            piezasHoraActual += 1;
            lblPiezasHoraActual.Content = piezasHoraActual;

            if(state == 1)
            {
                piezasBuenas += 1;
                lblBuenasHoraActual.Content = piezasBuenas;
                return;
            }

            piezasMalas += 1;
            lblMalasHoraActual.Content = piezasMalas;
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
    }
}
