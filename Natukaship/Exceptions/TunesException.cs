using System;

namespace Natukaship.Exceptions
{
    public class TunesException : Exception
    {
        /// <summary>
        /// Initializes a news instance of the TunesException
        /// class.
        /// </summary>
        public TunesException()
        {
        }

        /// <summary>
        /// Initializes a news instance of the TunesException
        /// class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public TunesException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a news instance of the TunesException
        /// class with a specified error message and a reference to the
        /// inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public TunesException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
