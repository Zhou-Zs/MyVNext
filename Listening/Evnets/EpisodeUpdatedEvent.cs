using Listening.Domain.Entities;
using MediatR;

namespace Listening.Domain.Evnets
{
    public record EpisodeUpdatedEvent(Episode Episode) : INotification;
}
