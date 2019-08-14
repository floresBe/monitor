using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace monitor.Reports
{
    public partial class SoldadoraResultado : DevExpress.XtraReports.UI.XtraReport
    {
        public SoldadoraResultado()
        {
            InitializeComponent();
        }
        public SoldadoraResultado(DateTime desde, DateTime hasta, string modelo)
        {
            InitializeComponent();
            lblDesde.Text = desde.ToString("d/M/yyy");
            lblHasta.Text = hasta.ToString("d/M/yyy");
            lblModelo.Text = modelo;

        }

    }
}
