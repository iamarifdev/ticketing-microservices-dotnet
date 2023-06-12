using System.Net;
using System.Runtime.Serialization;

namespace Ticketing.Common.Exceptions
{
    [Serializable]
    public class NotFoundException : HttpException
    {
        private const int Status = (int)HttpStatusCode.NotFound;

        public NotFoundException(string message) : base(message, Status)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, Status, innerException)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}