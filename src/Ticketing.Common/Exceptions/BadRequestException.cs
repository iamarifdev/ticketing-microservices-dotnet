using System.Net;
using System.Runtime.Serialization;

namespace Ticketing.Common.Exceptions
{
    [Serializable]
    public class BadRequestException : HttpException
    {
        private const int Status = (int)HttpStatusCode.BadRequest;

        public BadRequestException(string message) : base(message, Status)
        {
        }

        public BadRequestException(string message, Exception innerException) : base(message, Status, innerException)
        {
        }

        protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}