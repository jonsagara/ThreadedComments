using System.Web;
using System.Web.Mvc;
using ThreadedComments.Data.Entities;

namespace ThreadedComments.Controllers
{
    public class ThreadedCommentsBaseController : Controller
    {
        protected ThreadedCommentsEntities ThreadedCommentsContext { get; private set; }


        #region Override Methods

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ThreadedCommentsContext = (ThreadedCommentsEntities)HttpContext.Items[MvcApplication.CurrentRequestThreadedCommentsContext];
        }

        #endregion
    }
}
