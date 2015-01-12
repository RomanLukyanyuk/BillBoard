using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BillBoard.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("DatabaseConnection")
        { 
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Type> Types { get; set; }
    }
}