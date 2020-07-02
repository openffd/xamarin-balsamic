using System;
namespace Natukaship.Exceptions
{
    public class InvalidUserCredentialsException : Exception
    {
        /// <summary>
        /// Initializes a news instance of the InvalidUserCredentialsException
        /// class.
        /// </summary>
        public InvalidUserCredentialsException()
        {
        }

        /// <summary>
        /// Initializes a news instance of the InvalidUserCredentialsException
        /// class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidUserCredentialsException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a news instance of the InvalidUserCredentialsException
        /// class with a specified error message and a reference to the
        /// inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public InvalidUserCredentialsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
