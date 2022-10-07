using TaskTrackerAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Task = TaskTrackerAPI.Data.Models.Task;

namespace TaskTrackerAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Task> Tasks { get; set; }

    }
}
