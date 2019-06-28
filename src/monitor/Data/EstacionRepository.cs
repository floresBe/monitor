using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitor.Data
{
    public class EstacionRepository
    {
        private MonitoreoEntities _monitoreoEntities;

        public EstacionRepository()
        {
            _monitoreoEntities = new MonitoreoEntities();
        }

        public List<Estacion> GetEstaciones()
        {
            try
            {
                return _monitoreoEntities.Estacion.Where(usr => usr.Estatus == 1).ToList();
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public bool InsertEstacion(Estacion estacion)
        {
            try
            {
                if (_monitoreoEntities.Estacion.Any(a => a.Nombre == estacion.Nombre))
                {
                    throw new Exception("Ya existe una Estación con este nombre.");
                }
                _monitoreoEntities.Estacion.Add(estacion);
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
