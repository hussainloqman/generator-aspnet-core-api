using Framework.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace <%=baseName%>.Utilities.TransactionalActions
{
    public class TransactionalActionFilter : ActionFilterAttribute
    {
        private IUnitOfWork _unitOfWork;
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            IUnitOfWorkFactory unitOfWorkFactory = (IUnitOfWorkFactory)actionContext.Request.GetDependencyScope().GetService(typeof(IUnitOfWorkFactory));
            _unitOfWork = unitOfWorkFactory.Create();
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            _unitOfWork.Dispose();
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
