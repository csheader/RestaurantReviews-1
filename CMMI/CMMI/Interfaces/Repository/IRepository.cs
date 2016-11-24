namespace CMMI.Interfaces.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity value);
        int Save();
    }
}
