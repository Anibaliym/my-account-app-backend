<<<<<<< HEAD
﻿namespace MyAccountApp.Application.Responses
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
=======
﻿namespace MyAccountApp.Application.Responses
{
    public class GenericResponse
    {
        public GenericResponse()
        {
            Resolution = false;
            Data = new object();
            Message = string.Empty;
        }
        public bool Resolution { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }

    public class GenericResponse<T>
    {
        public bool Resolution { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ErrorCode { get; set; }
        public string[]? Errors { get; set; }
    }
}
>>>>>>> 6fb48a0 (feat(api): estandariza las respuestas de todos los endpoints GET y agrega pruebas)
