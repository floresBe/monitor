using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace monitor.Reports
{
    public partial class PiezasPorModelo : DevExpress.XtraReports.UI.XtraReport
    {
        public PiezasPorModelo()
        {
            InitializeComponent();
        }
        public PiezasPorModelo(DateTime desde, DateTime hasta)
        {
            InitializeComponent();
            lblDesde.Text = desde.ToString("d/M/yyy");
            lblHasta.Text = hasta.ToString("d/M/yyy");
        }
    }
}
