using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyHealthPassAuth.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Entities { get; }

        void Add(T entity);

        void Remove(T entity);
    }
}
