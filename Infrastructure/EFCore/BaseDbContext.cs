using DomainCommons;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFCore
{
    public abstract class BaseDbContext :DbContext
    {
        private IMediator? _mediator;
        protected BaseDbContext(DbContextOptions options,IMediator? mediator):base(options) 
        {
            _mediator = mediator;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new NotImplementedException("Don not call SaveChanges, please call SaveChangesAsync instead.");
        }

        public async override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            // 在写数据库前触发领域事件，因为领域事件的Handle也都属于领域，所以通过聚合根获取数据，所以可以读取到还没有提取的数据。
            // 这样可以保证所有连接在同一个连接中。集成事件一定要在SaveChanges之后。
            // 当然，其他领域对象如果直接去读数据库，是读不到这次的修改，因此要通过EF Core的FindAsync()方法读取，因为默认是读取本地缓存
            if (_mediator != null)
            {
                await _mediator.DispatchDomainEventsAsync(this);
            }

            // 在提交到数据库之前，记录那些被“软删除”实体对象。一定要ToList()，否则会延迟到ForEach的时候才执行
            var softDeletedEntities = this.ChangeTracker.Entries<ISoftDelete>()
                                          .Where(e => e.State == EntityState.Modified && e.Entity.IsDeleted)
                                          .Select(e => e.Entity).ToList();

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        }
    }
}
