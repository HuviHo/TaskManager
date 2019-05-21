using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TaskManager.Data;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Data
{
	public class ToDoRepository
	{
		private string _connectionString;

		public ToDoRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public void AddToDo (ToDo toDo)
		{
			using (var context = new TaskManagerContext(_connectionString))
			{
				AccountRepository accountRepos = new AccountRepository(_connectionString);
				context.ToDos.Add(toDo);
				context.SaveChanges();
			}
		}

		public void UpdateToDo(ToDo toDo)
		{
			using (var context = new TaskManagerContext(_connectionString))
			{
				AccountRepository accountRepos = new AccountRepository(_connectionString);
				context.ToDos.Update(toDo);
				context.SaveChanges();
			}
		}

		public IEnumerable<ToDo> GetIncompleteToDos()
		{
			using (var context = new TaskManagerContext(_connectionString))
			{
				return context.ToDos.Where(t=>t.ToDoStatus!=ToDoStatus.Completed).Include(t => t.User).ToList();
			}

		}

		public ToDo GetToDoForId(int id)
		{
			using (var context = new TaskManagerContext(_connectionString))
			{
				return context.ToDos.FirstOrDefault(t => t.Id == id);
			}
		}
	}
}
