using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RibbonApp
{
    // Custom výjimky. Skutečná chyba je zabalena do parametru innerException, tzn.
    // nastane výjimka DeserializationFailException, která je způsobena interně  např. chybou FileNotFound.

    #region DeserializationFailException

    public class DeserializationFailException : Exception
    {
        private static string _message = "Deserialization failed. Check inner exception for more details.";

        public DeserializationFailException() : base(_message)
        {

        }

        public DeserializationFailException(string message) : base(message)
        {

        }

        public DeserializationFailException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public DeserializationFailException(Exception innerException) : base(_message, innerException)
        {

        }
    }
    #endregion

    #region SerializationFailException
    public class SerializationFailException : Exception
    {
        private static string _message = "Serialization failed. Check inner exception for more details.";

        public SerializationFailException() : base(_message)
        {

        }

        public SerializationFailException(string message) : base(message)
        {

        }

        public SerializationFailException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public SerializationFailException(Exception innerException) : base(_message, innerException)
        {

        }
    }
    #endregion
}
