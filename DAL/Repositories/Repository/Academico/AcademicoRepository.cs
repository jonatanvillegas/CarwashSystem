using DAL.Entities;
using Loyola_ERP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Repository.Academico
{
    public class AcademicoRepository : Repository<Solicitud>, IAcademicoRepository
    {
        public AcademicoRepository(SchoolManagementContext context) : base(context) { }

        public async Task CrearSolicitud(Solicitud solicitud)
        {
            if (solicitud == null)
            {
                throw new ArgumentNullException(nameof(solicitud), "La solicitud no puede ser nula.");
            }

            await _context.Set<Solicitud>().AddAsync(solicitud);
            await _context.SaveChangesAsync();
        }
    }

}
