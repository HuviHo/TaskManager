using System.IO;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TaskManager.Data
{
	public class TaskItemsContextFactory : IDesignTimeDbContextFactory<TaskManagerContext>
	{
		public TaskManagerContext CreateDbContext(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}TaskManager.Web"))
				.AddJsonFile("appsettings.json")
				.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

			return new TaskManagerContext(config.GetConnectionString("ConStr"));
		}
	}
}