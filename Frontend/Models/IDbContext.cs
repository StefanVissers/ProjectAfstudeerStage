using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    interface IDbContext<T>
    {
        T Post(T model);
        T Get(string id);
    }
}
