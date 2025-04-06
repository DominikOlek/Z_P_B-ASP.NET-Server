using System;


namespace ApiP.Exceptions
{
    public class NotFoundExceptions : Exception
    {
        public NotFoundExceptions(string message) : base(message)
        {

        }
    }
}

