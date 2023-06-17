using System.Runtime.Serialization;
using Ticketing.Common.Exceptions;

namespace Ticketing.Tickets.Application.Exceptions
{
    [Serializable]
    public class TicketNotFoundException : NotFoundException
    {
        public TicketNotFoundException(int id) : base($"Ticket is not found by ID: {id}")
        {
        }

        protected TicketNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}