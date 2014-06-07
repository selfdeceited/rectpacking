using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace RectPacking.UI.Models
{
    public class PackingContext: DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<COA> COAs { get; set; }
        public DbSet<ConcreteType> ConcreteTypes { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<Frame> Frames { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Placement> Placements { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSet> ProductSets { get; set; }
        public DbSet<User> Users { get; set; } 
        public PackingContext() : base("PackingContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        } 
    }
}