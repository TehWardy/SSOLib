using Newtonsoft.Json;

namespace SharedObjects.Dtos
{
    public class ODataResult<T>
    {
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        [JsonProperty("value")]
        public T Value { get; set; }
    }
}
