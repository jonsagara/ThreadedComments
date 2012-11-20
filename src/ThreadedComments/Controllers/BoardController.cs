using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThreadedComments.Models.Board;

namespace ThreadedComments.Controllers
{
    public class BoardController : ThreadedCommentsBaseController
    {
        //
        // GET: /Board/

        public ActionResult Index(int? id)
        {
            if ((id ?? 0) == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var board = ThreadedCommentsContext.Boards.SingleOrDefault(b => b.BoardId == id);
            if (board == null)
            {
                throw new Exception("No board with id = " + id);
            }

            var viewModel = new BoardIndex();
            viewModel.Name = board.Name;
            viewModel.Description = board.Description;

            viewModel.Posts = ThreadedCommentsContext.Posts
                .Where(p => p.BoardId == id)
                .OrderByDescending(p => p.PostUtcDate)
                .Take(50)
                .ToList()
                .Select(p => new BoardIndex.PostInfo { PostId = p.PostId, Title = p.Title, Body = p.Body })
                .ToList();

            return View(viewModel);
        }

    }
}
