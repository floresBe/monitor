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

        public List<Usuario> GetUsuarios()
        {
            return _monitoreoEntities.Usuario.Where(usr => usr.Estatus == 1).ToList();
        }

        public List<Usuario> GetUsuariosLogin()
        {
            return _monitoreoEntities.Usuario.Where(usr => usr.Estatus == 1 && usr.Activo == 1).ToList();
        }


        public bool InsertUsuario(Usuario user)
        {
            try
            {
                if(_monitoreoEntities.Usuario.Any(a => a.NumeroEmpleado == user.NumeroEmpleado)){
                    throw new Exception("Ya existe un usuario con ese número de empleado.");
                }
                _monitoreoEntities.Usuario.Add(user);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
