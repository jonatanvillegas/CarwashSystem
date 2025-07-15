using DAL.Repositories.Repository.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public class UnidOfWork : IUnidOfWork
    {
        public UnidOfWork()
        {
            
        }
        public IAcademicoRepository academico {  get; set; }
        public IAcademicoRepository Academico { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
