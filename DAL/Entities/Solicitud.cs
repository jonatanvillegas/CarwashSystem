using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Solicitud
    {
        public int IdSolicitud { get; set; }
        public string nombrePadre { get; set; }
        public string cedula { get; set; }
        public DateTime fechaSolicitud { get; set; }
    }
}
