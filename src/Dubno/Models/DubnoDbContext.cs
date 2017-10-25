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
        public virtual DbSet<Subscriber> Subscribers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //contects to our database
            options.UseSqlServer("Server=63863a8b-be28-48ba-ab7f-a816014864c7.sqlserver.sequelizer.com;Database=db63863a8bbe2848baab7fa816014864c7;User ID=svjmljogqvclebob;Password=uHkfAkwGhVSrWRuAta73U4gxGf8nVQHnuHP338tGBWvLMqz4EKikePryAUFvkiTz;Persist Security Info=False;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30; Integrated Security = false;");
            

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