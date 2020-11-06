using System;
using System.Collections.Generic;
using System.Text;
using CafeBites.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CafeBites.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<CafeBitesUser> CafeBitesUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
    }
}
