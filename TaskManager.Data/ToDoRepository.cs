using System.Collections.Generic;
using System.Linq;
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
					  "UPDATE ToDos SET HandledBy = @userId WHERE Id = @toDoId",
					new SqlParameter("@userId", userId),
					new SqlParameter("@toDoId", toDoId));
			}
		}

		public void SetCompleted (int toDoId)
		{
			using (var context = new TaskManagerContext(_connectionString))
			{
				context.Database.ExecuteSqlCommand(
					"UPDATE ToDos SET IsCompleted = 1 WHERE Id = @toDoId",
					new SqlParameter("@toDoId", toDoId));
			}
		}

		public ToDo GetById(int id)
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
