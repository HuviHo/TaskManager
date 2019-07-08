using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TaskManager.Data
{
	public class ToDo
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public bool IsCompleted { get; set; }
		public int? HandledBy { get; set; }

		public User User { get; set; }
	}
}
