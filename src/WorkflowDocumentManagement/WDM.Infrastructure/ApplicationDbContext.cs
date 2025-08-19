using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDM.Domain.Entities;

namespace WDM.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(x =>
            x.CreatedDate).HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<User>()
                .HasData(
                new User
                {
                    Id = new Guid("8C647159-9A27-43C9-AA21-115E9DDDEE9E"),
                    Email = "irfan@gmail.com",
                    Password = "admin12345",
                    UserName = "admin",
                    AccessLevel = "Read-Write",
                    CreatedBy = new Guid("8C647159-9A27-43C9-AA21-115E9DDDEE9E"),
                    CreatedDate = new DateTime(2025, 8, 17, 19, 31, 26, DateTimeKind.Utc),
                });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
    }
}
