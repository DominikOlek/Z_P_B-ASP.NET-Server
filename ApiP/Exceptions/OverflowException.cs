using System;


namespace ApiP.Exceptions
{
    public class OverflowException : Exception
    {
        public OverflowException(string message) : base(message)
        {

        }
    }
}

