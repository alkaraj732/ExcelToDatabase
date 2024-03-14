using ExcelToDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelToDatabase.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<CustomerData> customersdata { get; set; }
       



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // You can set schema for each entity separately if required
            modelBuilder.Entity<CustomerData>().ToTable("WHATSAPPCUSTOMER2702", schema: "ZXNWVR");
       


        }
    }
}

