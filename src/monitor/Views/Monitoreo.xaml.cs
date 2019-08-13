using DocumentFormat.OpenXml.Packaging;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using monitor.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
        ResultadoSoldadoraRepository ResultadoSoldadoraRepository = new ResultadoSoldadoraRepository();

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

        Application powerPointApp = new Application();
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
            ;
        }

        private void Initialize()
        {
            InitializeHeader();
            InitializeTimerCycle();
            InitializeTimerCurrentTime();
            InitializeTimerSoldadora();
        }
        private void InitializeHeader()
        {
            lblPID.Content = Estacion.PID;
            lblModelo.Content = Modelo.NumeroModelo;

            lblRouting.Content = Modelo.Routing;
            ruta = Modelo.RutaAyudaVisual;

            lblEstacion.Content = Estacion.Nombre;
        }
        public async void InitializeDocumentViewer()
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
                        XpsDocument xpsDocument = await ConvertPowerPointToXps(powerPointFile, xpsFile);

                        xpsDocuments.Add(xpsDocument);
                    }

                    powerPointApp.Quit();

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
                //MessageBox.Show("No se pudo abrir el archivo: " + powerPointFile);
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
        private void InitializeTimerSoldadora()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(SoldadoraTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 15);
            dispatcherTimer.Start();
        }
        private async Task<XpsDocument> ConvertPowerPointToXps(string pptFilename, string xpsFilename)
        {
            Presentation presentation;
            try
            {

                presentation = powerPointApp.Presentations.Open(pptFilename, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
                presentation.ExportAsFixedFormat(xpsFilename, PpFixedFormatType.ppFixedFormatTypeXPS);
                presentation.Close();

            }
            catch (Exception ex)
            {
                await ConvertPowerPointToXps(pptFilename, xpsFilename);
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
        private async void MakePost()
        {
            try
            {
                string data = @"{""Sid"":0}";
                Dictionary<int, string> Properties = await Post($"http://{Estacion.IPSoldador}/Services/GetWeldResult", data);

                InSoldadoraData(Properties);
            }
            catch (Exception)
            {
                 
            }
        }

        public async Task<Dictionary<int, string>> Post(string URL, string data)
        {
            Dictionary<int, string> Properties = null;
            try
            {
                //Request
                HttpClient client = new HttpClient();
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = await client.PostAsync(URL, content);
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).", httpResponse.StatusCode, httpResponse.ReasonPhrase));
                }
                else
                {
                    var jsonstring = await httpResponse.Content.ReadAsStringAsync();

                    //Ejemplo de string de respuesta
                    // var jsonstring = @"{""ErrorCode"":0,""1"":832,""2"":""N / A"",""3"":""-- - "",""4"":0,""5"":0,""6"":""No"",""7"":""DEFAULT"",""8"":""DEFAULT"",""9"":""DEFAULT"",""10"":0.149,""11"":16.9,""12"":12.1,""14"":2.4,""15"":2.2937,""16"":2.2977,""17"":0.0231,""18"":0.0271,""19"":80,""22"":""-- - "",""23"":51,""24"":70,""25"":40160,""26"":40194,""27"":40176,""28"":40173,""29"":"" -3"",""30"":1.675,""31"":82,""32"":12,""33"":""XVH19050545E"",""34"":""19050085"",""35"":""10:51:10"",""36"":""08 - 07 - 19"",""37"":""Preset0 * ""}";
                    //Ejemplo de string de respuesta con error
                    //var jsonstring = @"{""ErrorCode"":23}";

                    //Regex para extraer la propiedad "ErrorCode" del string
                    Regex regexObject = new Regex(@"""ErrorCode"":\d+,");
                    var errorCode = regexObject.Match(jsonstring).ToString();

                    //Si no hay match significa que hubo un error
                    if (!string.IsNullOrEmpty(errorCode))
                    {
                        jsonstring = regexObject.Replace(jsonstring, "");
                        Properties = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<int, string>>(jsonstring);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Properties;
        }
        private void InSoldadoraData(Dictionary<int, string> Properties)
        {
            try
            {   
                if(Properties == null)
                {
                    return;
                }
                ResultadoSoldadora resultadoSoldadora = new ResultadoSoldadora()
                {
                    ModeloId = Modelo.ModeloId,
                    EstacionId = Estacion.EstacionId,
                    FechaHora = DateTime.Now,
                    CycleCount = int.Parse(Properties[1]),
                    Preset = Properties[4],
                    WeldTime = Properties[10],
                    PeakPower = Properties[11],
                    Energy = Properties[12],
                    Downspeed = Properties[14],
                    WeldAbsolute = Properties[15],
                    TotalAbsolute = Properties[16],
                    WeldColapse = Properties[17],
                    TotalColapse = Properties[18],
                    Pressure = Properties[24],
                    FrecuencyMin = Properties[25],
                    FrecuencyMax = Properties[26],
                    FrecuencyStart  = Properties[27],
                    FrecuencyEnd = Properties[28],
                    CycleTime = Properties[30],
                    HoldeForce = Properties[31],
                    TriggerForce = Properties[32],
                    TimeResult = Properties[35],
                    DateResult = Properties[36],
                    AlarmInfo = Properties.Keys.Any(a => a == 38) ? Properties[38] : "",
                };

                ResultadoSoldadoraRepository.InsertResultadoSoldadora(resultadoSoldadora);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error al registrar datos de soldadora. - Error: " + ex.Message);
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
        private void SoldadoraTimer_Tick(object sender, EventArgs e)
        {
                MakePost();
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
            MakePost();
        }
    }
}
