using System.Runtime.Serialization;
using Ticketing.Common.Exceptions;

namespace Ticketing.Tickets.Application.Exceptions;

[Serializable]
public class TicketAlreadyReservedException : BadRequestException
{
    public TicketAlreadyReservedException() : base("Cannot edit a reserved Ticket")
    {
    }

    protected TicketAlreadyReservedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

[Serializable]
public class UnauthorizedTicketEditException : UnauthorizedException
{
    public UnauthorizedTicketEditException() : base("Cannot edit a ticket of different user")
    {
    }

    protected UnauthorizedTicketEditException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}