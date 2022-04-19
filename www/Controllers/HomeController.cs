using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using www.Models;

namespace www.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IDistributedCache _cache;

		public HomeController(ILogger<HomeController> logger, IDistributedCache cache)
		{
			_logger = logger;
			_cache = cache;
		}

		public IActionResult Index()
		{
			var key = "aspnet:{yuryi}:bio";
			var bio = _cache.GetString(key);

			if (string.IsNullOrEmpty(bio))
			{
				bio = "of 32 years old"; // here we imitate call of slow and thick service

				_cache.SetString(key, bio);
			}

			ViewData["AdditionalMessage"] = $"Yuryi, {bio}!";

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
