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
                return _monitoreoEntities.Modelo.Where(usr => usr.Estatus == 1).ToList();
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
    }
}
