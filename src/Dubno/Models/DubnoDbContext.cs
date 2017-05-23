using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dubno.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Dubno.Models
{
    public class DubnoDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Post> Posts { get; set; }
      

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //contects to our database
            options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Dubno;integrated security=True");
        }

        public DubnoDbContext()
        {
        }

        public DubnoDbContext(DbContextOptions<DubnoDbContext> options)
                : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
        }

    }
}