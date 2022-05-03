using System;
using Microsoft.EntityFrameworkCore;
using Team1_Website.Models;

namespace Team1_Website.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
        }

        // maps to our tables in the database
        public DbSet<ActivationCode> ActivationCodes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchaseHistory> PurchaseHistory { get; set; }
        public DbSet<PurchaseList> PurchaseList { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rate> Rates { get; set; }
    }
}
