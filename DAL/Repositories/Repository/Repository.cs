using Loyola_ERP.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly SchoolManagementContext _context;

        public Repository(SchoolManagementContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public ValueTask<T?> GetById(int id)
        {
            return _context.Set<T>().FindAsync(id);
        }

        public void RemoveId(int id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
        }

    }
}
