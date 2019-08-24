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
                List<Estacion> estaciones = _monitoreoEntities.Estacion.Where(usr => usr.Estatus == 1).ToList();
                
                return estaciones; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertEstacion(Estacion estacion)
        {
            try
            {
                if (_monitoreoEntities.Estacion.Any(a => a.Nombre == estacion.Nombre && a.Estatus ==1))
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

        public bool DeleteEstacion(Estacion estacion)
        {
            try
            {
                Estacion dbEstacion = _monitoreoEntities.Estacion.Where(w => w.EstacionId == estacion.EstacionId).FirstOrDefault();

                if (dbEstacion != null)
                {
                    dbEstacion.Estatus = 0;
                    dbEstacion.FechaHora = DateTime.Now;
                    _monitoreoEntities.SaveChanges();
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateEstacion(Estacion estacion)
        {
            try
            {
                List<Estacion> estaciones = GetEstaciones();
                Estacion estacion1 = estaciones.Where(a => a.Nombre == estacion.Nombre && a.EstacionId != estacion.EstacionId).FirstOrDefault();
                if (estacion1 != null)
                {
                    throw new Exception("Ya existe una estación con ese nombre.");
                }
                Estacion Estacion = _monitoreoEntities.Estacion.Where(w => w.EstacionId == estacion.EstacionId).FirstOrDefault();

                if (Estacion != null)
                {
                    Estacion.FechaHora = estacion.FechaHora;
                    Estacion.Nombre = estacion.Nombre;
                    Estacion.Monitor = estacion.Monitor;
                    Estacion.IPPLC = estacion.IPPLC;
                    Estacion.IPSoldador = estacion.IPSoldador;
                    Estacion.Soldador = estacion.Soldador;
                    Estacion.SegundosAyudaVisual = estacion.SegundosAyudaVisual;
                    
                    _monitoreoEntities.SaveChanges();
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
