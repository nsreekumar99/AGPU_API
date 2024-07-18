using AGPU_API.Data;
using AGPU_API.Models;
using AGPU_API.Models.DTO;
using AGPU_API.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AGPU_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AGPUAPIController : ControllerBase
    {

        private readonly ILogger<AGPUAPIController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public AGPUAPIController(ILogger<AGPUAPIController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        //Get all GPU's
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AGPUDTO>>> GetGPUs()
        {
            _logger.LogInformation("Getting all GPU's");
            var gpuList = await _unitOfWork.AGPUs.GetAllAsync(); //returns control to threadpool of .net runtime
            return Ok(gpuList);  // threadpool assigns another thread for handling response from previous database operation.
        }

        //Get GPU

        [HttpGet("id", Name ="GETAGPU")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AGPUDTO>> GetGPU(int id)
        {
            if(id == 0)
            {
                _logger.LogError("Enter a Valid Id." + " " + "Current id = " + id);
                return BadRequest();
            }

            var AGPUvar = await _unitOfWork.AGPUs.GetAsync(u=>u.Id == id);

            if(AGPUvar == null)
            {
                return NotFound();
            }
            return Ok(AGPUvar);
        }

        //Create GPU

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<AGPUDTO>> CreateAGPU([FromBody]AGPUCreateDTO agpuDTO)
        {
            if(agpuDTO == null)
            {
                return BadRequest(agpuDTO);
            }

            if(await _unitOfWork.AGPUs.GetAsync(u=>u.Name.ToLower() == agpuDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Name already exists.");
                return BadRequest(ModelState);
            }

            //agpuDTO.Id = _db.AGPUs.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;

            AGPU model = new()
            {
                Name = agpuDTO.Name,
                Brand = agpuDTO.Brand,
                Model = agpuDTO.Model,
                ReleaseDate = agpuDTO.ReleaseDate,
                Price = agpuDTO.Price,
                ValuePercentage = agpuDTO.ValuePercentage,
                AverageBenchPercentage = agpuDTO.AverageBenchPercentage,
                Images = agpuDTO.Images
            };


            await _unitOfWork.AGPUs.AddAsync(model);
            await _unitOfWork.SaveAsync();

            return CreatedAtRoute("GETAGPU", new { id = model.Id }, model); // call get, include id, give full object

        }

        //Delete GPU

        [HttpDelete("id", Name = "DELETEAGPU")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<AGPUDTO>> DeleteAGPU(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var ExtractedAGPU = await _unitOfWork.AGPUs.GetAsync(u => u.Id == id);

            if(ExtractedAGPU == null)
            {
                return NotFound();
            }

            _unitOfWork.AGPUs.Remove(ExtractedAGPU);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        //Update GPU

        [HttpPut("id",Name = "EditAGPU")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<AGPUDTO>> EditAGPU(int id , [FromBody] AGPUUpdateDTO agpuDTO)
        {
            if(agpuDTO == null || id != agpuDTO.Id) 
            {
                return BadRequest();
            }

            var ExtractedAGPU = await _unitOfWork.AGPUMain.GetAsync(u => u.Id == id, tracked:false);

            //if(ExtractedAGPU == null)
            //{
            //    return BadRequest();
            //}

            //ExtractedAGPU.Name = agpuDTO.Name;
            //ExtractedAGPU.Price = agpuDTO.Price;
            //ExtractedAGPU.ValuePercentage = agpuDTO.ValuePercentage;
            //ExtractedAGPU.AverageBenchPercentage = agpuDTO.AverageBenchPercentage;
            //ExtractedAGPU.Model = agpuDTO.Model;
            //ExtractedAGPU.Brand = agpuDTO.Brand; // removed agpu store

            AGPU model = new()
            {
                Id = agpuDTO.Id,
                Name = agpuDTO.Name,
                Brand = agpuDTO.Brand,
                Model = agpuDTO.Model,
                ReleaseDate = agpuDTO.ReleaseDate,
                Price = agpuDTO.Price,
                ValuePercentage = agpuDTO.ValuePercentage,
                AverageBenchPercentage = agpuDTO.AverageBenchPercentage,
                Images = agpuDTO.Images
            };
            await _unitOfWork.AGPUMain.UpdateAsync(model);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

    }
}
