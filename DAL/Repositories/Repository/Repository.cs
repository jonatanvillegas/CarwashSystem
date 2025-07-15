using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public List<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
