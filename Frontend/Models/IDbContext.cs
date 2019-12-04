using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public interface IDbContext<T>
    {
        T Post(T model);
        T Put(string id, T model);
        T Get(string id);

        void Delete(string id);
    }
}
