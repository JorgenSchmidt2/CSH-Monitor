namespace CSH_Monitor.Core.Responses
{
    public class EntityResponse<T>
    {
        public T? Data { get; set; }
        public string? Message { get; set; }
        public bool Status { get; set; }
    }
}