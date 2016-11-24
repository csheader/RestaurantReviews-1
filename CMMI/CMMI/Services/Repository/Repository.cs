using CMMI.DataAccess;
using CMMI.Interfaces.Repository;
using System;

namespace CMMI.Services.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly CMMIContext context;

        public Repository(CMMIContext context)
        {
            this.context = context;
        }

        public virtual void Add(TEntity value)
        {
            context.Set<TEntity>().Add(value);
        }

        public int Save()
        {
            return context.SaveChanges();
        }
    }
}