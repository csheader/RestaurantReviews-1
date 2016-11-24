using CMMI.Interfaces;
using CMMI.DataAccess;
using System;
using CMMI.Services.Repository;
using CMMI.Interfaces.Repository;

namespace CMMI.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;
        private CMMIContext context;
        public IRestaurantRepository Restaurants { get; private set; }
        public IReviewRepository Reviews { get; private set; }
        public IUserRepository Users { get; private set; }

        public UnitOfWork(CMMIContext context)
        {
            this.context = context;
            Restaurants = new RestaurantRepository(context);
            Reviews = new ReviewRepository(context);
            Users = new UserRepository(context);
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }
        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}