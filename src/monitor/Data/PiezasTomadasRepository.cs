using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitor.Data
{
    public class PiezasTomadasRepository
    {
        private MonitoreoEntities _monitoreoEntities;

        public PiezasTomadasRepository()
        {
            _monitoreoEntities = new MonitoreoEntities();
        }

        public List<PiezasTomadas> GetPiezasTomadases()
        {
            try
            {
                return _monitoreoEntities.PiezasTomadas.ToList();
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public bool InsertPiezasTomadas(PiezasTomadas PiezasTomadas)
        {
            try
            { 
                _monitoreoEntities.PiezasTomadas.Add(PiezasTomadas);
                _monitoreoEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
