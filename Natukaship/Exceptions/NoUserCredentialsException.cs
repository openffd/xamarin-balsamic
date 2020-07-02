using System;
namespace Natukaship.Exceptions
{
    public class NoUserCredentialsException : Exception
    {
        /// <summary>
        /// Initializes a news instance of the NoUserCredentialsException
        /// class.
        /// </summary>
        public NoUserCredentialsException()
        {
        }

        /// <summary>
        /// Initializes a news instance of the NoUserCredentialsException
        /// class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public NoUserCredentialsException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a news instance of the NoUserCredentialsException
        /// class with a specified error message and a reference to the
        /// inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public NoUserCredentialsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
