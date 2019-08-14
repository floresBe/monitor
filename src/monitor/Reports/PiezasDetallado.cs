using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace monitor.Reports
{
    public partial class PiezasDetallado : DevExpress.XtraReports.UI.XtraReport
    {
        public PiezasDetallado()
        {
            InitializeComponent();
        }
        public PiezasDetallado(DateTime desde, DateTime hasta, int descontadosIng, int calidad, int produccion, string modelo)
        {
            InitializeComponent();
            lblDesde.Text = desde.ToString("d/M/yyy");
            lblHasta.Text = hasta.ToString("d/M/yyy");
            lblDescontadosIngenieria.Text = descontadosIng.ToString();
            lblCalidad.Text = calidad.ToString();
            lblProduccion.Text = produccion.ToString();
            lblModelo.Text = modelo;
        }

    }
}
