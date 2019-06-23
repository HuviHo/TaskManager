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
			AccountRepository repos = new AccountRepository(_connectionString);
			int userId = repos.GetUserByEmail(User.Identity.Name).Id;
			return View(userId);
		}	
	}
}
