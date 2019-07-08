using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Data;

namespace TaskManager.Web
{
	public class ToDoHub : Hub
	{
		private string _connectionString;

		public ToDoHub(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("ConStr");
		}

		public void NewToDo(string title)
		{
			ToDoRepository toDoRepos = new ToDoRepository(_connectionString);
			ToDo toDo = new ToDo {Title = title, IsCompleted = false,
							};
			toDoRepos.AddToDo(toDo);
			SendTasks();
		}

		private void SendTasks()
		{
			var repos = new ToDoRepository(_connectionString);
			var tasks = repos.GetActiveToDos();
			Clients.All.SendAsync("RenderTasks", tasks.Select(t => new
			{
				Id = t.Id,
				Title = t.Title,
				HandledBy = t.HandledBy,
				UserDoingIt = t.User != null ? $"{t.User.FirstName} {t.User.LastName}" : null,
			}));
		}

		public void GetAll()
		{
			SendTasks();
		}


		public void SetDoing(int toDoId)
		{
			ToDoRepository toDoRepos = new ToDoRepository(_connectionString);
			AccountRepository accountRepos = new AccountRepository(_connectionString);
			User user = accountRepos.GetUserByEmail(Context.User.Identity.Name);
			toDoRepos.SetDoing(toDoId, user.Id);
			SendTasks();
		}

		public void SetDone (int toDoId)
		{
			ToDoRepository repos = new ToDoRepository(_connectionString);
			repos.SetCompleted(toDoId);
			SendTasks();
		}
	}
}
