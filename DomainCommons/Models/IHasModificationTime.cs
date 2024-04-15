namespace DomainCommons
{
    public interface IHasModificationTime
    {
        DateTime? LastModificationTime { get; }
    }
}
