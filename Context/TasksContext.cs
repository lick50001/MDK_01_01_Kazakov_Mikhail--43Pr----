using System.Data.Entity;
using TaskManager_Kazakov.Models;

namespace TaskManager_Kazakov.Context
{
    public class TasksContext : DbContext
    {
        public TasksContext() : base("name=TaskManagerContext"){}

        public DbSet<Tasks> Tasks { get; set; }
    }
}