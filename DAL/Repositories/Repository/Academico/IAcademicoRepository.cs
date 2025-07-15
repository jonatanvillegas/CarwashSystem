using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Repository.Academico
{
    public interface IAcademicoRepository : IRepository<Solicitud>
    {
        Task CrearSolicitud(Solicitud solicitud);
    }
}
