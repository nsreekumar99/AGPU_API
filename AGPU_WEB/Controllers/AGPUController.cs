using AGPU_WEB.Models;
using AGPU_WEB.Models.DTO;
using AGPU_WEB.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AGPU_WEB.Controllers
{
    public class AGPUController : Controller
    {
        private readonly IAGPUService _agpuService;
        private readonly IMapper _mapper;
        private readonly ILogger<AGPUController> _logger;
        public AGPUController(IAGPUService aGPUService, IMapper mapper, ILogger<AGPUController> logger)
        {
            _agpuService = aGPUService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AGPUCreateDTO dto)
        {
            if (ModelState.IsValid)
            {
                var response = await _agpuService.AddAsync<APIResponse>(dto);
                if (response != null && response.IsSuccess == true)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(dto);
        }
        
        public async Task<IActionResult> Edit (int id)
        {
            var response = await _agpuService.GetAsync<APIResponse>(id); // get the agpu based on id

            if(response != null && response.IsSuccess == true)
            {
                var result = JsonConvert.DeserializeObject<AGPUDTO>(Convert.ToString(response.Result)); //the result is of form agpudto
                var mappedResult = _mapper.Map<AGPUUpdateDTO>(result);
                return View(mappedResult);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (AGPUUpdateDTO dto)
        {
            if (ModelState.IsValid)
            {
                var response = await _agpuService.UpdateAsync<APIResponse>(dto);
                if(response!=null && response.IsSuccess == true)
                {
					return RedirectToAction(nameof(Index));
				}
                return NotFound();
            }
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            // Log the anti-forgery token from the request headers
            //var token = Request.Headers["RequestVerificationToken"].ToString();
            //_logger.LogInformation("Anti-Forgery Token from request: {Token}", token);

            var response = _agpuService.GetAsync<APIResponse>(id);

            if(response != null)
            {
                var delete =_agpuService.RemoveAsync<APIResponse>(id);

                if(delete != null)
                {
                    return RedirectToAction(nameof(Index));
				}
            }
            return NotFound();
		}

	}
}
