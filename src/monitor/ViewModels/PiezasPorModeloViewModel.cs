using DevExpress.DataAccess.Native.Data;
using DevExpress.Mvvm;
using DevExpress.Xpf.Printing.Native;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

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
            System.Data.DataTable dataSource = new System.Data.DataTable();
            DataSet ds = new DataSet();

            var test = System.Configuration.ConfigurationSettings.AppSettings.Get("MonitoreoEntities");

            using (var con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings.Get("MonitoreoEntities")))
            using (var cmd = new SqlCommand("ReportePiezasPorModelo", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Desde", "20190101");
                cmd.Parameters.AddWithValue("@Hasta", "20191010");
                cmd.Parameters.AddWithValue("@Modelo", "A");
                cmd.Parameters.AddWithValue("@Estacion", "1");
                da.Fill(dataSource);
            }

            ds.Tables.Add(dataSource);

            var report = new Reports.PiezasPorModelo(DateTime.Now, DateTime.Now,23,7,11);

            report.DataSource = ds;

            Report = report;
        }
    }
}