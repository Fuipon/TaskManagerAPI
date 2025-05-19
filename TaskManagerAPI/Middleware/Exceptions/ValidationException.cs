using System;

namespace TaskManagerAPI.Middleware.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}