namespace Listening.Domain.ValueObjects
{
    public record Sentence(TimeSpan StatTime, TimeSpan EndTime, string Value);
}
