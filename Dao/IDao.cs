using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace taskcore.Dao
{
    public interface IDao<T>
    {
        public Task<Boolean> Insert(Object obj);
     //   public Task<List<T>> Read(int id);
        public Task<Boolean> Modify(Object obj);
        public Task<Boolean> Erase(int id);

        public DatabaseContext GetContext();

    }
}