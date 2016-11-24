using CMMI.Models;
using System.Data.Entity;

namespace CMMI.DataAccess
{
    public class CMMIContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Address> Address { get; set; }
    }
}