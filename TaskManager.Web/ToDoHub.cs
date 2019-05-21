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

		public void AddToDoSignalR(ToDo toDo)
		{
			Clients.All.SendAsync("NewToDo", toDo);
		}
	}
}
