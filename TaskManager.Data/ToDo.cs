using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManager.Data
{
	public class ToDo
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ToDoStatus ToDoStatus { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
	}
}
