using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace CreateApi_NancyFk
{
    public class NancyContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=Nancy_ApiV2;Integrated Security=True; MultipleActiveResultSets=True");
        }
    }
}
