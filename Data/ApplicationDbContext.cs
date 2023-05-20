using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebOS.Models;

namespace WebOS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<WebOS.Models.Country> Country { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<WebOS.Models.City> City { get; set; }
        public DbSet<WebOS.Models.Category> Category { get; set; }
        public DbSet<WebOS.Models.Scholar> Scholar { get; set; }
        public DbSet<WebOS.Models.Fatwa> Fatwa { get; set; }
        public DbSet<WebOS.Models.Article> Article { get; set; }
    




    }
}
