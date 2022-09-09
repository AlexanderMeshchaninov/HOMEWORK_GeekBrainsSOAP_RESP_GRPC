using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryService.Core
{
    public interface IRepository<T, TId> 
        where T : class
        where TId : class
    {
        TId Add(T item);
        int Update(T item);
        int Delete(T item);
        IList<T> GetAll();
        T GetById(TId id);
    }
}