using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Usuarios
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
    }
}
