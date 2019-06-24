using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TaskManager.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

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

		public IEnumerable<ToDo> GetActiveToDos()
		{
			using (var context = new TaskManagerContext(_connectionString))
			{
				return context.ToDos.Include(t=>t.User)
					.Where(t=> ! t.IsCompleted).ToList();
			}
		}

		public void SetDoing (int toDoId, int userId)
		{
			using (var context = new TaskManagerContext(_connectionString))
			{
				context.Database.ExecuteSqlCommand(
					  "UPDATE ToDos SET HandledBy = @userId WHERE Id = @taskId",
					new SqlParameter("@userId", userId),
					new SqlParameter("@taskId", toDoId));
			}
		}

		public void SetCompleted (int toDoId)
		{
			using (var context = new TaskManagerContext(_connectionString))
			{
				context.Database.ExecuteSqlCommand(
					"UPDATE TaskItems SET IsCompleted = 1 WHERE Id = @taskId",
					new SqlParameter("@taskId", toDoId));
			}
		}

		public ToDo GetToDoForId(int id)
		{
			using (var context = new TaskManagerContext(_connectionString))
			{
				return context.ToDos.Include(t => t.User).FirstOrDefault(t => t.Id == id);
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
	}
}
