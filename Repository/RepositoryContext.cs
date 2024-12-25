using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
   : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //  modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            // modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }

        public DbSet<Brand>? Brands { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<Complaint>? Complaints { get; set; }
        public DbSet<Like>? Likes { get; set; }
        public DbSet<Notification>? Notifications { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<Report>? Reports { get; set; }
        public DbSet<User>? Users { get; set; }

    }
}