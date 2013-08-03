using System;
using System.Transactions;
using System.Web.Mvc;

namespace DevAgenda.Infrastructure
{
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
  public class TransactionAttribute : ActionFilterAttribute
  {
    private TransactionScope _currentTransaction;

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      _currentTransaction = new TransactionScope();
    }

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
      if (filterContext.Exception == null)
      {
        _currentTransaction.Complete();
      }

      _currentTransaction.Dispose();
    }
  }
}
