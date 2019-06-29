using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitor.Data
{
    public class PiezaRepository
    {
        private MonitoreoEntities _monitoreoEntities;

        public PiezaRepository()
        {
            _monitoreoEntities = new MonitoreoEntities();
        }

        public List<Pieza> GetPiezas()
        {
            try
            {
                return _monitoreoEntities.Pieza.ToList();
            }
            catch (Exception)
            { 
                return null;
            }
        }

        public bool InsertPieza(Pieza pieza)
        {
            try
            { 
                _monitoreoEntities.Pieza.Add(pieza);
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
