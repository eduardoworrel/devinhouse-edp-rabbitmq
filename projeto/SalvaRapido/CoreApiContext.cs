using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;



    public class CoreApiContext : DbContext
    {


        public DbSet<Tweet> Tweets { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=165.22.10.47;Database=TwitterFriends;User Id=sa;Password=Helio@123@Sql;TrustServerCertificate=true;");
        }
    }

