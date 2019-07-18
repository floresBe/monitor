using DevExpress.Mvvm;
using DevExpress.Xpf.Printing.Native;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitor.ViewModels
{
    public class PiezasPorModeloViewModel : BindableBase
    {
        public XtraReport Report
        {
            get
            {
                return GetProperty(() => Report);
            }
            set
            {
                SetProperty(() => Report, value);
                if (Report != null)
                {
                    Report.PrintingSystem.ReplaceService(typeof(BackgroundPageBuildEngineStrategy), new DispatcherPageBuildStrategy());
                    Report.CreateDocument(true);
                }
            }
        }

        public PiezasPorModeloViewModel()
        {
            Report = new Reports.PiezasPorModelo();
        }
    }
}
