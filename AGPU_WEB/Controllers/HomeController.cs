using AGPU_WEB.Models;
using AGPU_WEB.Models.DTO;
using AGPU_WEB.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AGPU_WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAGPUService _agpuService;

        public HomeController(ILogger<HomeController> logger, IAGPUService aGPUService)
        {
            _logger = logger;
            _agpuService = aGPUService;
        }

        public async Task<IActionResult> Index()
        {
			List<AGPUDTO> list = new(); // define a new list to store the list of agpu's.
			var response = await _agpuService.GetAllAsync<APIResponse>();
			if (response != null && response.IsSuccess == true)
			{
				list = JsonConvert.DeserializeObject<List<AGPUDTO>>(Convert.ToString(response.Result));
			}
			return View(list);
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
