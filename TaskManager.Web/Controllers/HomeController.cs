using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TaskManager.Data;
using TaskManager.Web.Models;

namespace TaskManager.Web.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private string _connectionString;

		public HomeController(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("ConStr");
		}

		public IActionResult Index()
		{
			ToDoRepository repos = new ToDoRepository(_connectionString);
			return View(repos.GetIncompleteToDos());
		}

		[HttpPost]
		public IActionResult AddToDo(String name)
		{
			ToDoRepository toDoRepos = new ToDoRepository(_connectionString);
			AccountRepository accountRepos = new AccountRepository(_connectionString);
			ToDo toDo = new ToDo()
			{
				Name = name,
				ToDoStatus = ToDoStatus.Unclaimed,
				UserId = accountRepos.GetUserByEmail(User.Identity.Name).Id,
			};
			toDoRepos.AddToDo(toDo);
			toDo.User = accountRepos.GetUserForId(toDo.UserId);
			return Json(toDo);
		}

		[HttpPost]
		public IActionResult UpdateToDo(int id, ToDoStatus toDoStatus)
		{
			ToDoRepository toDoRepos = new ToDoRepository(_connectionString);
			AccountRepository accountRepos = new AccountRepository(_connectionString);
			ToDo toDo = toDoRepos.GetToDoForId(id);
			toDo.ToDoStatus = toDoStatus;
			toDoRepos.UpdateToDo(toDo);

			return Redirect("/");
		}

		public IActionResult GetIncompleteToDos()
		{
			ToDoRepository repos = new ToDoRepository(_connectionString);
			IEnumerable<ToDo> toDos = repos.GetIncompleteToDos();
			return Json(toDos);
		}
	}
}
