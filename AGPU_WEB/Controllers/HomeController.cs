using AGPU_WEB.Models;
using AGPU_WEB.Models.DTO;
using AGPU_WEB.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AGPU_WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAGPUService _agpuService;
		private readonly ICompositeViewEngine _viewEngine;

        public HomeController(ILogger<HomeController> logger, IAGPUService aGPUService, ICompositeViewEngine viewEngine)
        {
            _logger = logger;
            _agpuService = aGPUService;
			_viewEngine = viewEngine;
        }

		public async Task<List<AGPUDTO>> GetAllAGPUsAsync()
		{
			List<AGPUDTO> list = new();
			var response = await _agpuService.GetAllAsync<APIResponse>();
			if (response != null && response.IsSuccess == true)
			{
				list = JsonConvert.DeserializeObject<List<AGPUDTO>>(Convert.ToString(response.Result)) ?? new List<AGPUDTO>();
			}
			return list;
		}

        public async Task<IActionResult> Index()
        {
			//List<AGPUDTO> list = new(); // define a new list to store the list of agpu's.
			//var response = await _agpuService.GetAllAsync<APIResponse>();
			//if (response != null && response.IsSuccess == true)
			//{
			//	list = JsonConvert.DeserializeObject<List<AGPUDTO>>(Convert.ToString(response.Result));
			//}
			//return View(list);

			var list = await GetAllAGPUsAsync();
			return View(list);
		}

        public async Task<IActionResult> ValueSort(string brand = null, int minValue = 0, int maxValue = 0) // for value percentage
        {
			var list = await GetAllAGPUsAsync();

			// Apply filtering if filterBrand is provided
			if (!string.IsNullOrEmpty(brand))
			{
				list = list.Where(u => u.Brand == brand).ToList();
			}

			if (minValue != 0 || maxValue != 0)
			{
				list = list.Where(u => u.ValuePercentage >= minValue && u.ValuePercentage <= maxValue).ToList();
			}

			var sortedList = list.OrderByDescending(u=>u.ValuePercentage).ToList();

            return PartialView("_GPUsPartial", sortedList);
		}

		public async Task<IActionResult> GetSortedGPUs(string brand = null, string sortby = null, int minValue = 0, int maxValue = 0, int benchSliderLeftValue = 0, int benchSliderRightValue = 0) // for bench percentage descending.
        {
			var list = await GetAllAGPUsAsync();

			// Apply filtering if filterBrand is provided
			if (!string.IsNullOrEmpty(brand))
			{
				list = list.Where(u => u.Brand == brand).ToList();
			}

			if(minValue != 0 || maxValue != 0)
			{
				list = list.Where(u => u.ValuePercentage >= minValue && u.ValuePercentage <= maxValue).ToList();
			}

			if (benchSliderLeftValue != 0 || benchSliderRightValue != 0)
			{
				list = list.Where(u => u.AverageBenchPercentage >= benchSliderLeftValue && u.AverageBenchPercentage <= benchSliderRightValue).ToList();
			}

			//var sortedList = list.OrderByDescending(u => u.AverageBenchPercentage).ToList();

			switch (sortby)
            {
				case "Benchmark":
					list = list.OrderByDescending(u => u.AverageBenchPercentage).ToList();
					break;
				case "AgeAsc":
					list = list.OrderBy(u => u.ReleaseDate).ToList();
					break;
				case "AgeDesc":
					list = list.OrderByDescending(u => u.ReleaseDate).ToList();
					break;
				case "PriceAsc":
					list = list.OrderBy(u => u.Price).ToList();
					break;
				case "PriceDesc":
					list = list.OrderByDescending(u => u.Price).ToList();
					break;
				default:
					break;
			}

			var gpuCount = list.Count();
			var renderedHtml = RenderPartialViewToString("_GPUsPartialBench", list);

			//return PartialView("_GPUsPartialBench", list);
			return Json(new
			{
				html = renderedHtml,
				count = gpuCount
			});
        }

		public async Task<IActionResult> GetPriceRange(string brand = null)
		{
			var list = await GetAllAGPUsAsync();

			// Apply filtering if filterBrand is provided
			if (!string.IsNullOrEmpty(brand))
			{
				list = list.Where(u => u.Brand == brand).ToList();
			}

			var minValue = list.Min(u => u.ValuePercentage);
			var maxValue = list.Max(u => u.ValuePercentage);

			var minBenchValue = list.Min(u=>u.AverageBenchPercentage);
			var maxBenchValue = list.Max(u => u.AverageBenchPercentage);

			return Json(new {minValue, maxValue,minBenchValue,maxBenchValue});
		}

		// Helper method to convert partial view to string
		private string RenderPartialViewToString(string viewName, object model)
		{
			ViewData.Model = model;

			using (var writer = new StringWriter())
			{
				var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);
				if (viewResult.View == null)
				{
					throw new ArgumentNullException($"View {viewName} was not found.");
				}

				var viewContext = new ViewContext(
					ControllerContext,
					viewResult.View,
					ViewData,
					TempData,
					writer,
					new HtmlHelperOptions()
				);

				viewResult.View.RenderAsync(viewContext).Wait();
				return writer.GetStringBuilder().ToString();
			}
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
