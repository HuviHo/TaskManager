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

		public void AddToDo(string name, int userId)
		{
			ToDoRepository toDoRepos = new ToDoRepository(_connectionString);
			AccountRepository accountRepos = new AccountRepository(_connectionString);
			ToDo toDo = new ToDo()
			{
				Name = name,
				ToDoStatus = ToDoStatus.Unclaimed,
				UserId= userId
			};
			toDoRepos.AddToDo(toDo);
			GetAllToDos();
		}

		public void UpdateToDo(int id, ToDoStatus toDoStatus)
		{
			ToDoRepository toDoRepos = new ToDoRepository(_connectionString);
			AccountRepository accountRepos = new AccountRepository(_connectionString);
			ToDo toDo = toDoRepos.GetToDoForId(id);
			toDo.ToDoStatus = toDoStatus;
			toDoRepos.UpdateToDo(toDo);
			GetAllToDos();
		}

		public void GetAllToDos()
		{
			ToDoRepository repos = new ToDoRepository(_connectionString);
			IEnumerable<ToDo> toDos = repos.GetIncompleteToDos();
			Clients.All.SendAsync("RenderToDos", toDos);
		}
	}
}
