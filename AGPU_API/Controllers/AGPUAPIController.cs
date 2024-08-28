using AGPU_API.Data;
using AGPU_API.Models;
using AGPU_API.Models.DTO;
using AGPU_API.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AGPU_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AGPUAPIController : ControllerBase
    {

        private readonly ILogger<AGPUAPIController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        protected APIResponse _response;
        private readonly IMapper _mapper;

        public AGPUAPIController(ILogger<AGPUAPIController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            this._response = new APIResponse(); //initializes the _response var with the new instance of APIResponse class.
            _mapper = mapper;
        }

        //Get all GPU's
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetGPUs()
        {
            try
            {
                _logger.LogInformation("Getting all GPU's");
                var gpuList = await _unitOfWork.AGPUs.GetAllAsync(); //returns control to threadpool of .net runtime
                _response.Result = _mapper.Map<List<AGPUDTO>>(gpuList); // map<destination>(objectSource);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);  // threadpool assigns another thread for handling response from previous database operation.
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while getting GPUs:{ex.Message}");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { "An error occurred while getting all GPU's." };
                return StatusCode((int)_response.StatusCode, _response);
            }
        }

        //Get GPU

        [HttpGet("{id}", Name = "GETAGPU")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetGPU(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _logger.LogError("Enter a Valid Id." + " " + "Current id = " + id);
                    return BadRequest(_response);
                }

                var AGPUvar = await _unitOfWork.AGPUs.GetAsync(u => u.Id == id);

                if (AGPUvar == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<AGPUDTO>(AGPUvar);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while getting the GPU with ID {id}:{ex.Message}");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { "An error occurred while processing your request." };
                return StatusCode((int)_response.StatusCode, _response);
            }
        }

        //Create GPU

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<APIResponse>> CreateAGPU([FromBody] AGPUCreateDTO agpuDTO)
        {
            try
            {
                if (agpuDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                if (await _unitOfWork.AGPUs.GetAsync(u => u.Name.ToLower() == agpuDTO.Name.ToLower()) != null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    //ModelState.AddModelError("CustomError", "Name already exists.");
                    _response.ErrorMessages = new List<string> { "Name already Exists." };
                    return BadRequest(_response);
                }

                //agpuDTO.Id = _db.AGPUs.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;

                //AGPU model = new()
                //{
                //    Name = agpuDTO.Name,
                //    Brand = agpuDTO.Brand,
                //    Model = agpuDTO.Model,
                //    ReleaseDate = agpuDTO.ReleaseDate,
                //    Price = agpuDTO.Price,
                //    ValuePercentage = agpuDTO.ValuePercentage,
                //    AverageBenchPercentage = agpuDTO.AverageBenchPercentage,
                //    Images = agpuDTO.Images
                //};

                var model = _mapper.Map<AGPU>(agpuDTO);
                await _unitOfWork.AGPUs.AddAsync(model);
                await _unitOfWork.SaveAsync();
                _response.Result = _mapper.Map<AGPUCreateDTO>(model);
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                return CreatedAtRoute("GETAGPU", new { id = model.Id }, _response); // call get, include id, give full object
            }
            catch (Exception ex) 
            {
                _logger.LogError($"An error occured while creating the GPU:{ex.Message}");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { "An error occurred while creating the GPU." };
                return StatusCode((int)_response.StatusCode, _response);
            }
        }

        //Delete GPU

        [HttpDelete("{id}", Name = "DELETEAGPU")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<APIResponse>> DeleteAGPU(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                var ExtractedAGPU = await _unitOfWork.AGPUs.GetAsync(u => u.Id == id);

                if (ExtractedAGPU == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                _unitOfWork.AGPUs.Remove(ExtractedAGPU);
                await _unitOfWork.SaveAsync();
                //_response.Result = _mapper.Map<AGPUDTO>(ExtractedAGPU);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok();
            } catch (Exception ex)
            {
                _logger.LogError($"An error occured while deleting the GPU with ID {id} :{ex.Message}");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { "An error occurred while deleting the GPU." };
                return StatusCode((int)_response.StatusCode, _response);
            }
        }

        //Update GPU

        [HttpPut("{id}", Name = "EditAGPU")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<AGPUDTO>> EditAGPU(int id, [FromBody] AGPUUpdateDTO agpuDTO)
        {
            try
            {
                if (agpuDTO == null || id != agpuDTO.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                var ExtractedAGPU = await _unitOfWork.AGPUMain.GetAsync(u => u.Id == id, tracked: false);

                if (ExtractedAGPU == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                //optional 

                _mapper.Map(agpuDTO, ExtractedAGPU); // modifies data currently in extracted gpu to the data input from enduser.

                //var model = _mapper.Map<AGPU>(agpuDTO); 
                await _unitOfWork.AGPUMain.UpdateAsync(ExtractedAGPU); // extracted gpu still of type agpumain so doesnt need mapper
                await _unitOfWork.SaveAsync();
                _response.Result = _mapper.Map<AGPUUpdateDTO>(ExtractedAGPU);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }catch (Exception ex)
            {
                _logger.LogError($"An error occured while upading the GPU with ID {id} :{ex.Message}");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { "An error occurred while updating the GPU." };
                return StatusCode((int)_response.StatusCode, _response);
            }
        }

    }
}
