using Listening.Domain.Entities;
using MediatR;

namespace Listening.Domain.Evnets
{
    public record EpisodeCreatedEvent(Episode Episode) : INotification;
}
