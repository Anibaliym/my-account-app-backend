namespace MyAccountApp.Application.Responses
{
    public class GenericResponse
    {
        public GenericResponse()
        {
            Resolution = false;
            Data = new Object{};
            Message = string.Empty;
            Errors = null; 
            ErrorCode = null; 
        }

        public bool Resolution { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
        public string? ErrorCode { get; set; }
        public string[]? Errors { get; set; }
    }
}
