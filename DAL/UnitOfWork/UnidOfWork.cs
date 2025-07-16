using DAL.Repositories.Repository.Academico;
using Loyola_ERP.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public class UnidOfWork : IUnidOfWork
    {

        protected readonly SchoolManagementContext _context;
        private readonly IHttpContextAccessor _accessor;    


        private  IAcademicoRepository _academicoRepository;


        public UnidOfWork(SchoolManagementContext context)
        {
           _context = context;
        }
        public IAcademicoRepository Academico => _academicoRepository ??= new AcademicoRepository(_context) ;

        public void Dispose()
        {
           _context.Dispose();
        }
    }
}
