using MediatR;

namespace Listening.Domain.Evnets
{
    public record EpisodeDeletedEvent(Guid Id) : INotification;
}
