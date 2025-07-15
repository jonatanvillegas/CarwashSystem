using DAL.Repositories.Repository.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public interface IUnidOfWork:IDisposable
    {
        IAcademicoRepository Academico { get;set;}
    }
}
