using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using www.Models;

namespace www.Controllers
{
	public class HomeController : Controller
	{
		private const string key = "aspnet:{yuryi}:bio";

		private readonly ILogger<HomeController> _logger;
		private readonly IDistributedCache _cache;

		public HomeController(ILogger<HomeController> logger, IDistributedCache cache)
		{
			_logger = logger;
			_cache = cache;
		}

		private string GetSetUserBio()
		{
			var bio = _cache.GetString(key);

			if (string.IsNullOrEmpty(bio))
			{
				bio = "of 32 years old"; // here we imitate call of slow and thick service

				_cache.SetString(key, bio);
			}

			return bio;
		}

		public IActionResult Index()
		{
			ViewData["AdditionalMessage"] = $"Yuryi, {GetSetUserBio()}!";
			HttpContext.Session.Set("YuryiSessionKeyTest", Encoding.UTF8.GetBytes("Lorem ipsum..."));

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
