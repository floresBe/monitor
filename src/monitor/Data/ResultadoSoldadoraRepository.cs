using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitor.Data
{
    public class ResultadoSoldadoraRepository
    {
        private MonitoreoEntities _monitoreoEntities;

        public ResultadoSoldadoraRepository()
        {
            _monitoreoEntities = new MonitoreoEntities();
        }

        public List<ResultadoSoldadora> GetResultadoSoldadoraes()
        {
            try
            {
                return _monitoreoEntities.ResultadoSoldadora.ToList();
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public bool InsertResultadoSoldadora(ResultadoSoldadora ResultadoSoldadora)
        {
            try
            {
                if (!_monitoreoEntities.ResultadoSoldadora.Any(a => a.CycleCount == ResultadoSoldadora.CycleCount && a.DateResult == ResultadoSoldadora.DateResult))
                {
                    _monitoreoEntities.ResultadoSoldadora.Add(ResultadoSoldadora);
                    _monitoreoEntities.SaveChanges(); 
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
