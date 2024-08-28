using AGPU_WEB.Models;
using AGPU_WEB.Models.DTO;
using AGPU_WEB.Services.IServices;
using AGPU_Utility;

namespace AGPU_WEB.Services
{
    public class AGPUService : BaseService, IAGPUService
    {
        private readonly IHttpClientFactory httpclient;
        private string AGPUUrl;

        public AGPUService(IHttpClientFactory ClientFactory, IConfiguration configurations) : base(ClientFactory)
        {
            httpClient = ClientFactory;
            AGPUUrl = configurations.GetValue<string>("ServiceUrl:AGPUAPI");
        }

        public Task<T> AddAsync<T>(AGPUCreateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = AGPUUrl + "/api/AGPUAPI",
                Data = dto,
                ApiType = SD.ApiType.POST
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = AGPUUrl + "/api/AGPUAPI",
                ApiType = SD.ApiType.GET
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = AGPUUrl + "/api/AGPUAPI/"+id,
                ApiType = SD.ApiType.GET
            });
        }

        public Task<T> RemoveAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = AGPUUrl + "/api/AGPUAPI/" + id,
                ApiType = SD.ApiType.DELETE
            });
        }

        public Task<T> UpdateAsync<T>(AGPUUpdateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = AGPUUrl + "/api/AGPUAPI/"+dto.Id,
                ApiType = SD.ApiType.PUT,
                Data = dto
            });
        }
    }
}
