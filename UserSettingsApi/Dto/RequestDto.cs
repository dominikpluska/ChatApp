using MongoDB.Bson;

namespace UserSettingsApi.Dto
{
    public class RequestDto
    {
        public string RequestId { get; set; }
        public string RequestorId { get; set; }
        public string UserName { get; set; }
        public string RequestType { get; set; }
        public bool IsAccepted { get; set; }
    }
}
