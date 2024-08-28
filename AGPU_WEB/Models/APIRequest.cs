using static AGPU_Utility.SD;

namespace AGPU_WEB.Models
{
    public class APIRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET; //setting GET as the default value for ApiType property.
        public string Url { get; set; }
        public object Data { get; set; }
    }
}
