using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TaskManager.Data;

namespace TaskManager.Web.Controllers
{
    public class AccountController : Controller
    {
		private string _connectionString;

		public AccountController(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("ConStr");
		}

		public IActionResult SignUp()
		{
			return View();
		}

		[HttpPost]
		public IActionResult SignUp(User user)
		{
			AccountRepository repos = new AccountRepository(_connectionString);
			repos.AddUser(user);
			return RedirectToAction("LogIn");
		}

		public IActionResult LogIn()
		{
			return View();
		}

		[HttpPost]
		public IActionResult LogIn (string email, string passwordTyped)
		{
			AccountRepository repos = new AccountRepository(_connectionString);
			User user = repos.LogIn(email, passwordTyped);
			if (user==null)
			{
				return RedirectToAction("LogIn");
			}

			var claims = new List<Claim>
			{
				new Claim ("user", email)
			};

			HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
						return Redirect("/");
		}

		public IActionResult LogOut()
		{
			HttpContext.SignOutAsync().Wait();
			return Redirect("/");
		}
    }
}