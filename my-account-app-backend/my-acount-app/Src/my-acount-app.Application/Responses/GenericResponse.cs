﻿namespace MyAccountApp.Application.Responses
{
    public class GenericResponse
    {
        public GenericResponse()
        {
            Resolution = false;
            Data = string.Empty;
            Message = string.Empty;
        }
        public bool Resolution { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
}