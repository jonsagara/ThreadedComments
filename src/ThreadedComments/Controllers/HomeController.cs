using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThreadedComments.Models.Home;

namespace ThreadedComments.Controllers
{
    public class HomeController : ThreadedCommentsBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            var viewModel = new HomeIndex();

            viewModel.Boards = ThreadedCommentsContext.Boards
                .OrderBy(b => b.Name)
                .Take(10)
                .ToList()
                .Select(b => new HomeIndex.BoardInfo { BoardId = b.BoardId, Name = b.Name })
                .ToList();

            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
