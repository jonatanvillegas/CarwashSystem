using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Repository
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAll();
        public ValueTask<T?> GetById(int id);
        void RemoveId(int id);
    }
}
