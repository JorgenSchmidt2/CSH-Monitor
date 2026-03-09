namespace CSH_Monitor.Core.Responses
{
    public class EnumDataResponse<T>
    {
        public List<T>? Data { get; set; }
        public string? Message { get; set; }
        public bool Status { get; set; }
    }
}