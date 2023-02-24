using coreApi.Models;
using Microsoft.EntityFrameworkCore;


namespace CoreApi.Context
{
    public class CoreApiContext:DbContext
    {
        public CoreApiContext(DbContextOptions<CoreApiContext> options) : base(options){ }
        public DbSet<Tweet> Tweets { get; set; }
    }
}
