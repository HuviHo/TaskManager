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
			User user = repos.GetUserByEmail(User.Identity.Name);
			return View(new IndexViewModel { UserId = user.Id });
		}	

		[AllowAnonymous]
		public ActionResult GetTask (int id)
		{
			ToDoRepository repos = new ToDoRepository(_connectionString);
			return Json(repos.GetById(id));
		}
	}
}
