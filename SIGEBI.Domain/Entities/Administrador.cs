using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities
{
    public class Administrador : User
    {
        public string? AreaResponsabilidad { get; set; }
        public bool EsSuperAdmin { get; set; }
        public void ActivarUsuario(User usuario)
        {
            if (usuario != null)
                usuario.IsActive = true;
        }
        public void DesactivarUsuario(User usuario)
        {
            if (usuario != null)
                usuario.IsActive = false;
        }
        public void CambiarRolUsuario(User usuario, Role nuevoRol)
        {
            if (usuario != null && nuevoRol != null)
            {
                usuario.ID = nuevoRol.ID;
                usuario.Role = nuevoRol;
            }
        }
        public void RegistrarHistorial(UserLogin login, ICollection<UserLogin> historial)
        {
            if (login == null) return;
            if (historial == null) return;
            historial.Add(login);
        }
    }
}