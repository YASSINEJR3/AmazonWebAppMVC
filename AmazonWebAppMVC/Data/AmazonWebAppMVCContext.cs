using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AmazonWebAppMVC.Models;

namespace AmazonWebAppMVC.Data
{
    public class AmazonWebAppMVCContext : DbContext
    {
        public AmazonWebAppMVCContext (DbContextOptions<AmazonWebAppMVCContext> options)
            : base(options)
        {
        }

        public DbSet<AmazonWebAppMVC.Models.Category> Category { get; set; } = default!;

        public DbSet<AmazonWebAppMVC.Models.Product> Product { get; set; } = default!;
    }
}
