using CMMI.Interfaces.Repository;
using System;

namespace CMMI.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRestaurantRepository Restaurants { get; }
        IReviewRepository Reviews { get; }
        IUserRepository Users { get; }
        int Save();
    }
}
