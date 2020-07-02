using System;
namespace Natukaship.Exceptions
{
    public class ExceededMaximumVerificationAttemptException : Exception
    {
        /// <summary>
        /// Initializes a news instance of the ExceededMaximumVerificationAttemptException
        /// class.
        /// </summary>
        public ExceededMaximumVerificationAttemptException()
        {
        }

        /// <summary>
        /// Initializes a news instance of the ExceededMaximumVerificationAttemptException
        /// class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public ExceededMaximumVerificationAttemptException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a news instance of the ExceededMaximumVerificationAttemptException
        /// class with a specified error message and a reference to the
        /// inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ExceededMaximumVerificationAttemptException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
