using static Villa_Utility.SD;

namespace Villa_Web.Models
{
    public class APIRequest
    {
        public ApiType ApiType { get; set; } = ApiType.Get;
        public string URL {  get; set; }
        public object Data { get; set; }
    }
}
