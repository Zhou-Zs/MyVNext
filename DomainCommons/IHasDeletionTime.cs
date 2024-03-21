namespace DomainCommons
{
    /// <summary>
    /// 删除时间
    /// </summary>
    public interface IHasDeletionTime
    {
        /// <summary>
        /// 删除时间
        /// </summary>
        DateTime? DeletionTime { get; }
    }
}
