using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Transactions;

namespace ASPNETCore
{
    public class UnitOfWorkFilter : IAsyncActionFilter
    {
        private static UnitOfWorkAttribute? GetUoWAttr(ActionDescriptor actionDesc)
        {
            var caDesc = actionDesc as ControllerActionDescriptor;
            if (caDesc == null)
            {
                return null;
            }

            var uowAttr = caDesc.ControllerTypeInfo.GetCustomAttribute<UnitOfWorkAttribute>();
            if (uowAttr != null)
            {
                return uowAttr;
            }
            else
            {
                return caDesc.MethodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
            }

        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var uowAttr = GetUoWAttr(context.ActionDescriptor);
            if (uowAttr == null)
            {
                await next();
                return;
            }

            // 创建跨越多个数据库连接、甚至跨越不同数据存储的分布式事务
            using TransactionScope tsScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled); // 这个枚举表示在异步流中启用事务范围的传递
            List<DbContext> dbCtxs = new List<DbContext>();
            foreach (var dbCtxType in uowAttr.DbContextTypes)
            {
                var sp = context.HttpContext.RequestServices;
                DbContext dbCtx = (DbContext)sp.GetRequiredService(dbCtxType); // 获取当前请求的dbContext实例
                dbCtxs.Add(dbCtx);
            }
            var result = await next(); // 执行后面的逻辑，没有异常就提交事务
            if (result.Exception == null)
            {
                foreach (var dbCtx in dbCtxs)
                {
                    await dbCtx.SaveChangesAsync();
                }

                tsScope.Complete();
            }
        }
    }
}
