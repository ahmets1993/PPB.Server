using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PPB.BL
{
    public interface IRepository<T> : IDisposable where T : class
    {
        T GetByID(string Id);
        IEnumerable<T> Select(Expression<Func<T, bool>> Filter = null);
        IEnumerable<T> WhereDynamicLinq(string query);
        IEnumerable<T> WhereDynamicLinqString(string query, string columnName, List<string> filterList, Func<T, bool> where);
        IEnumerable<T> WhereAndDynamicLinqString(Expression<Func<T, bool>> where, string query);
        void Insert(T Entity);
        void Update(T Entity);

        void Delete(T Entity);
        void DeleteListEntity(List<T> Entity);
        void InsertListEntity(List<T> Entity);
        ICollection<T> GetAll();
        T Find(Expression<Func<T, bool>> where);
        T SingleOrDefault(Expression<Func<T, bool>> where);
        T FirstOrDefault(Expression<Func<T, bool>> where);

        T LastOrDefault(Expression<Func<T, bool>> where);
        T Single();
        T First();

        bool Any(Expression<Func<T, bool>> where);
        List<T> GetList();
        List<T> GetRandom(int take);

        List<T> Where(Expression<Func<T, bool>> where);

        void UpdateRange(List<T> Entities);
        void AddRange(IEnumerable<T> Entities);
        void RemoveRange(IEnumerable<T> Entities);

        int Count(Expression<Func<T, bool>> where);

        List<T> AsQueryable(Expression<Func<T, bool>> predicate);

        List<T> Contains(string MongoColumnName, string MongoValue, Expression<Func<T, bool>> Sqlpredicate);

    }
}
