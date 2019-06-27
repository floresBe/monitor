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
        int pages;
        int page;

        public Monitoreo()
        {
            InitializeComponent();

            InitializeDocumentViwer();  
        }

        private void InitializeDocumentViwer()
        {
            // to do: Buscar URL de archivo segun modelo y estación. 
            const string powerPointFile = @"C:\Users\Flores\Dropbox\Monitor\AyudaVisual.ppt";
            
            var xpsFile = System.IO.Path.GetTempPath() + Guid.NewGuid() + ".ppsx";
            var xpsDocument = ConvertPowerPointToXps(powerPointFile, xpsFile);

            DocumentviewPowerPoint.Document = xpsDocument.GetFixedDocumentSequence();
            //DocumentviewPowerPoint.Zoom = 75.00;
            page = 1;
            pages = DocumentviewPowerPoint.PageCount;

            InitializeTimerDocumentViewer();
        }

        private void InitializeTimerDocumentViewer()
        { 
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(NextPageTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start(); 
        }

        private void NextPageTimer_Tick(object sender, EventArgs e)
        { 
            if(page < pages)
            {
                DocumentviewPowerPoint.NextPage(); 
                page++;
                return;
            }

            DocumentviewPowerPoint.FirstPage();
            page = 1;
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
    }
}
