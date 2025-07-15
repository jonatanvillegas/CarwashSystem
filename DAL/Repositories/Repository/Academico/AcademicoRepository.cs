using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Repository.Academico
{
    public class AcademicoRepository : Repository<Solicitud>, IAcademicoRepository
    {
        public Task CrearSolicitud(Solicitud solicitud)
        {
            throw new NotImplementedException();
        }
    }
}
