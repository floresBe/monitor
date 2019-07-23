using DocumentFormat.OpenXml.Packaging;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using monitor.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
        PiezaRepository PiezaRepository;
        ResultadoSoldadoraRepository ResultadoSoldadoraRepository;

        public Estacion Estacion { get; set; }
        public Modelo Modelo { get; set; }

        int piezasHoraActual;
        int piezasHoraAnterior;

        int piezasBuenas;
        int piezasMalas;

        DateTime timeCycle;

        List<XpsDocument> xpsDocuments;
        string ruta;
        int documents;
        int document;
        int pages;
        int page;

        public Monitoreo()
        {
            InitializeComponent();
            Initialize();
        }
        public Monitoreo(Estacion estacion, Modelo modelo)
        {
            Estacion = estacion;
            Modelo = modelo;

            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            PiezaRepository = new PiezaRepository();
            ResultadoSoldadoraRepository = new ResultadoSoldadoraRepository();

            InitializeHeader();
            InitializeDocumentViwer();
            InitializeTimerCycle();
            InitializeTimerCurrentTime();
        }
        public void InitializeHeader()
        {
            lblPID.Content = Estacion.PID;
            lblModelo.Content = Modelo.NumeroModelo;

            lblRouting.Content = Modelo.Routing;
            ruta = Modelo.RutaAyudaVisual;

            lblEstacion.Content = Estacion.Nombre;
        }
        private void InitializeDocumentViwer()
        {
            string powerPointFile = "";
            xpsDocuments = new List<XpsDocument>();
            try
            {
                DirectoryInfo d = new DirectoryInfo(ruta + @"\" + Estacion.Nombre); // Se abre directorio
                FileInfo[] Files = d.GetFiles("*.ppt"); // Se obtienen los documentos  

                if (Files.Count() > 0)
                {
                    foreach (FileInfo file in Files)
                    {
                        powerPointFile = ruta + Estacion.Nombre + @"\" + file;

                        string xpsFile = System.IO.Path.GetTempPath() + Guid.NewGuid() + Estacion.Nombre + file + ".ppsx";
                        XpsDocument xpsDocument = ConvertPowerPointToXps(powerPointFile, xpsFile);

                        xpsDocuments.Add(xpsDocument);
                    } 

                    documents = xpsDocuments.Count();
                    document = 1;

                    ShowNewDocument();
                    InitializeTimerDocumentViewer();
                    return;
                }

                throw new FormatException("No se encontro ningún archivo .ppt en la ruta: " + ruta + @"\" + Estacion.Nombre);
            }
            catch (FormatException fe)
            {
                MessageBox.Show(fe.Message);
            }
            catch (Exception ez)
            {
                MessageBox.Show("No se pudo abrir el archivo: " + powerPointFile);
            }
        }
        private void InitializeTimerDocumentViewer()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(NextPageTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, Estacion.SegundosAyudaVisual.Value);
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
        private static XpsDocument ConvertPowerPointToXps(string pptFilename, string xpsFilename)
        {
            //New Application Power Point
            Application powerPointApp;
            Presentation presentation;
            try
            {
                powerPointApp = new Application();

                presentation = powerPointApp.Presentations.Open(pptFilename, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
                presentation.ExportAsFixedFormat(xpsFilename, PpFixedFormatType.ppFixedFormatTypeXPS, PpFixedFormatIntent.ppFixedFormatIntentScreen, MsoTriState.msoCTrue);

                presentation.Close();
                powerPointApp.Quit();
            }
            catch (Exception ex)
            {  
                ConvertPowerPointToXps(pptFilename, xpsFilename);
            } 

            return new XpsDocument(xpsFilename, FileAccess.Read);
        }
        private void InPLCData(string modelo, string state)
        {
            AddPieza(int.Parse(state));

            if (modelo != Modelo.ModeloId)
            {
                WarningMessageGrid.Visibility = Visibility.Visible;
                return;
            }

            WarningMessageGrid.Visibility = Visibility.Collapsed;
        }
        private void InSoldadoraData(int cycle, double pkpwr, double totalAbs, double energy, double weldForce)
        {
            try
            {
                ResultadoSoldadora resultadoSoldadora = new ResultadoSoldadora()
                {
                    ModeloId = Modelo.ModeloId,
                    EstacionId = Estacion.EstacionId,
                    Cycle = cycle,
                    PkPwr = pkpwr,
                    TotalAbs = totalAbs,
                    Energy = energy,
                    WeldForce = weldForce,
                    FechaHora = DateTime.Now
                };

                ResultadoSoldadoraRepository.InsertResultadoSoldadora(resultadoSoldadora);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar datos de soldadora. - Error: " + ex.Message);
            }
        }
        private void AddPieza(int state)
        {
            try
            {
                Pieza pieza = new Pieza()
                {
                    EstacionId = Estacion.EstacionId,
                    Estado = state,
                    ModeloId = Modelo.ModeloId,
                    PID = int.Parse(Estacion.PID),
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
                MessageBox.Show("Error al registrar pieza. - Error: " + ex.Message);
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

            if (document < documents)
            {
                document++;
                ShowNewDocument();
                return;
            }

            document = 1;
            ShowNewDocument();
        }
        private void ShowNewDocument()
        {
            DocumentviewPowerPoint.Document = xpsDocuments[document - 1].GetFixedDocumentSequence();
            page = 1;
            pages = DocumentviewPowerPoint.PageCount;
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

        //Botones auxiliares
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
            InPLCData("#", "1");
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            InPLCData(Modelo.ModeloId, "0");
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            InSoldadoraData(1, 1.2, 14.1, 12, 23.2);
        }
    }
}
