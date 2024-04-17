using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace ASPNETCore
{
    class UnitOfWorkFilter : IAsyncActionFilter
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
              return  caDesc.MethodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
            }

        }


        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var uowAttr = GetUoWAttr(context.ActionDescriptor);

        }
    }
}
