using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TaskManager.Data
{
	public class AccountRepository
	{
		private string _connectionString;

		public AccountRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public void AddUser(User user)
		{
			user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

			using (var context = new TaskManagerContext(_connectionString))
			{
				context.Users.Add(user);
				context.SaveChanges();
			}
		}

		public User LogIn(string email, string passwordTyped)
		{
			User user = GetUserByEmail(email);
			if (user == null)
			{
				return null;
			}

			bool IsCorrect = BCrypt.Net.BCrypt.Verify(passwordTyped, user.Password);

			if (IsCorrect)
			{
				return user;
			}
			return null;
		}

		public User GetUserByEmail(string email)
		{
			using (var context = new TaskManagerContext(_connectionString))
			{
				return context.Users.FirstOrDefault(u => u.Email == email);
			}
		}


		public User GetUserForId(int id)
		{
			using (var context = new TaskManagerContext(_connectionString))
			{
				return context.Users.FirstOrDefault(u => u.Id == id);
			}
		}
	}
}
