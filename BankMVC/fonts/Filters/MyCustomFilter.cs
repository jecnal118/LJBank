using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankMVC.Filters
{
    public class MyCustomFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];
            var message = $"[OnActionExecuting] - {controllerName}.{actionName}";
            //Change to log later
            Debug.WriteLine(message);

            base.OnActionExecuting(filterContext);
        }
    }
}