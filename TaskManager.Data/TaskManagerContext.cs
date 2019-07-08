using Microsoft.EntityFrameworkCore;

namespace TaskManager.Data
{
	public class TaskManagerContext : DbContext
	{
		private string _connectionString;

		public TaskManagerContext(string connectionString)
		{
			_connectionString = connectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(_connectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ToDo>()
				.HasOne(t => t.User)
				.WithMany(u => u.ToDos)
				.HasForeignKey(t => t.HandledBy);
		}

		public DbSet<User> Users { get; set; }
		public DbSet<ToDo> ToDos { get; set; }
	}
}
