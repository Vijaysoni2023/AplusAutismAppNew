using aplusautism.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Repository.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<T> GetAsync(long id);
        Task<int?> InsertAsync(T entity);
        //List<T> GetUserForLogin(string Email, string Password);

        int? Insert(T entity);
        Task<int?> UpdateAsync(T entity);
        Task<int?> DeleteAsync(T entity);
        void Remove(T entity);
        Task<int?> SaveChangesAsync();
        int? SaveChanges();
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        Task<int?> UpdateAsync(List<T> entity);
        Task<int?> InsertMultiAsync(List<T> entity);
        Task<T> ExecuteWithJsonResultAsync(string name, string parserString, params SqlParameter[] parameters);
        List<T> ExecuteWithJsonResult(string name, string parserString, params SqlParameter[] parameters);
        void ExecuteStoreProcedureWithoutReturnType(string procName, string entity, params SqlParameter[] parameters);
        T Find(params object[] keyValues);

      
    }
}
