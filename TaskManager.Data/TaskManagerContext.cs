using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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

		public DbSet<User> Users { get; set; }
		public DbSet<ToDo> ToDos { get; set; }
	}
}
