using DevExpress.Mvvm;
using DevExpress.Xpf.Printing.Native;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitor.ViewModels
{
    public class PiezasTomadasViewModel : BindableBase
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

        public PiezasTomadasViewModel(DateTime desde, DateTime hasta, string modelo)
        {
            System.Data.DataTable dataSource = new System.Data.DataTable();
            DataSet ds = new DataSet();

            var test = System.Configuration.ConfigurationSettings.AppSettings.Get("MonitoreoEntities");

            using (var con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings.Get("MonitoreoEntities")))
            using (var cmd = new SqlCommand("ReportePiezasTomadas", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Desde", desde.ToString("yyyyMMdd"));
                cmd.Parameters.AddWithValue("@Hasta", hasta.ToString("yyyyMMdd"));
                cmd.Parameters.AddWithValue("@Modelo", modelo); 
                da.Fill(dataSource);
            }

            ds.Tables.Add(dataSource);

            var report = new Reports.PiezasTomadas(desde, hasta, modelo);

            report.DataSource = ds;

            Report = report;
        }

    }
}
