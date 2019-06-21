using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitor.Data
{
    public class UsuarioRepository
    {
        private MonitoreoEntities _monitoreoEntities;

        public UsuarioRepository()
        {
            _monitoreoEntities = new MonitoreoEntities();
        }

        public List<Usuarios> GetUsuarios()
        {
            return _monitoreoEntities.Usuarios.Where(usr => usr.Estatus == 1).ToList();
        }

        public List<Usuarios> GetUsuariosLogin()
        {
            return _monitoreoEntities.Usuarios.Where(usr => usr.Estatus == 1 && usr.Activo == 1).ToList();
        }


        public bool InsertUsuario(Usuarios user)
        {
            try
            {
                if(_monitoreoEntities.Usuarios.Any(a => a.NumeroEmpleado == user.NumeroEmpleado)){
                    throw new Exception("Ya existe un usuario con ese número de empleado.");
                }
                _monitoreoEntities.Usuarios.Add(user);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
