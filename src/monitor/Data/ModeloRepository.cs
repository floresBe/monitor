using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitor.Data
{
    public class ModeloRepository
    {
        private MonitoreoEntities _monitoreoEntities;

        public ModeloRepository()
        {
            _monitoreoEntities = new MonitoreoEntities();
        }

        public List<Modelo> GetModelos()
        {
            try
            {
                return _monitoreoEntities.Modelo.Where(w => w.Estatus == 1).ToList();
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public Modelo GetLastModelo()
        {
            try
            {
                Modelo modelo = _monitoreoEntities.Modelo.ToList().OrderBy(o => o.FechaHora).Last(); 
                return modelo;
            }
            catch (Exception ex)
            { 
                return null;
            }
        }

        public bool InsertModelo(Modelo model)
        {
            try
            {
                if (_monitoreoEntities.Modelo.Any(a => a.NumeroModelo == model.NumeroModelo))
                {
                    throw new Exception("Ya existe un Modelo con este número.");
                }
                _monitoreoEntities.Modelo.Add(model);
                _monitoreoEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteModelo(Modelo model)
        {
            try
            {
                Modelo modelo = _monitoreoEntities.Modelo.Where(w => w.ModeloId == model.ModeloId).FirstOrDefault();

                if (modelo != null)
                {
                    modelo.Estatus = 0;
                    modelo.FechaHora = DateTime.Now;
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

        public bool UpdateModelo(Modelo model)
        {
            try
            {
                if (_monitoreoEntities.Modelo.Any(a => a.NumeroModelo == model.NumeroModelo && a.ModeloId != model.ModeloId))
                {
                    throw new Exception("Ya existe un modelo con ese número.");
                }
                Modelo modelo = _monitoreoEntities.Modelo.Where(w => w.ModeloId == model.ModeloId).FirstOrDefault();

                if (modelo != null)
                { 
                    modelo.FechaHora = model.FechaHora;
                    modelo.NumeroModelo = model.NumeroModelo;
                    modelo.Routing = model.Routing;
                    modelo.RutaAyudaVisual = model.RutaAyudaVisual;
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
