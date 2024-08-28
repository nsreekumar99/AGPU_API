using AGPU_WEB.Models;
using AGPU_WEB.Models.DTO;
using AGPU_WEB.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AGPU_WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AGPUControlPanelDataTableAPIController : ControllerBase
    {
        private readonly IAGPUService _agpuService;
        private readonly IMapper _mapper;
        public AGPUControlPanelDataTableAPIController(IAGPUService agpuService, IMapper mapper)
        {
            _agpuService = agpuService;
            _mapper = mapper;
        }

        [HttpGet("TransferData")]
        public async Task<IActionResult> TransferData()
        {
            List<AGPUDTO> list = new(); // define a new list to store the list of agpu's.
            var response = await _agpuService.GetAllAsync<APIResponse>();
            if(response != null && response.IsSuccess == true)
            {
                list = JsonConvert.DeserializeObject<List<AGPUDTO>>(Convert.ToString(response.Result));
            }
            return Ok(list);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _agpuService.GetAsync<APIResponse>(id);

            if (response != null && response.IsSuccess == true)
            {
                var delete = await _agpuService.RemoveAsync<APIResponse>(id);

                if (delete != null && delete.IsSuccess == true)
                {
                    return Ok(new { message = "Item deleted successfully." });
                }
            }
            return NotFound(new { message = "Item not found or deletion failed." });
        }

    }
}
