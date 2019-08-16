using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace monitor.Reports
{
    public partial class PiezasTomadas : DevExpress.XtraReports.UI.XtraReport
    {
        public PiezasTomadas()
        {
            InitializeComponent();
        }
        public PiezasTomadas(DateTime desde, DateTime hasta, string modelo)
        {
            InitializeComponent();
            lblDesde.Text = desde.ToString("d/M/yyy");
            lblHasta.Text = hasta.ToString("d/M/yyy");  
        }

    }
}
