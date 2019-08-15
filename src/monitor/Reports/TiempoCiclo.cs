using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;

namespace monitor.Reports
{
    public partial class TiempoCiclo : DevExpress.XtraReports.UI.XtraReport
    {
        public TiempoCiclo()
        {
            InitializeComponent();
        }

        public TiempoCiclo(DateTime desde, DateTime hasta, System.Data.DataTable dataSource)
        {
            InitializeComponent();
            lblDesde.Text = desde.ToString("d/M/yyy");
            lblHasta.Text = hasta.ToString("d/M/yyy");

            Series series = new Series("Minutos", ViewType.Bar);
            foreach (System.Data.DataRow item in dataSource.Rows)
            {
                series.Points.Add(new SeriesPoint(item["Estacion"].ToString(), double.Parse(item["TiempoCiclo"].ToString())));
            }
            xrChart1.Series.Add(series);
        }


    }
}
