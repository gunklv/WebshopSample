﻿namespace Catalog.Domain.Exceptions
{
    public class InvalidStateException : Exception
    {
        public InvalidStateException(string message) : base(message) { }
    }
}
