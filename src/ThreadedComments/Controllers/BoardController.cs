using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ThreadedComments.Data.Entities;
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
            viewModel.BoardId = board.BoardId;
            viewModel.Name = board.Name;
            viewModel.Description = board.Description;

            viewModel.Posts = ThreadedCommentsContext.Posts
                .Where(p => p.BoardId == id)
                .OrderByDescending(p => p.PostUtcDate)
                .Take(50)
                .ToList()
                .Select(p => new BoardIndex.PostInfo { PostId = p.PostId, Title = p.Title, Body = p.Body, AuthorName = p.AuthorName, AuthorEmail = p.AuthorEmail, PostUtcDate = p.PostUtcDate })
                .ToList();

            return View(viewModel);
        }

        public ActionResult NewPost(int? id)
        {
            var board = ThreadedCommentsContext.Boards.SingleOrDefault(b => b.BoardId == id);
            if (board == null)
            {
                throw new Exception("Invalid Board id = " + id);
            }

            return View(new BoardNewPost { BoardId = board.BoardId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewPost(BoardNewPost viewModel)
        {
            var board = ThreadedCommentsContext.Boards.SingleOrDefault(b => b.BoardId == viewModel.BoardId);
            if (board == null)
            {
                throw new Exception("Invalid Board id = " + viewModel.BoardId);
            }

            if (ModelState.IsValid)
            {
                var post = new Post();
                post.BoardId = board.BoardId;
                post.Title = viewModel.Title.Trim();
                post.Body = (viewModel.Body ?? string.Empty).Trim();
                post.AuthorName = viewModel.AuthorName.Trim();
                post.AuthorEmail = viewModel.AuthorEmail.Trim();
                post.PostUtcDate = DateTime.UtcNow;

                ThreadedCommentsContext.Posts.Add(post);

                return RedirectToAction("Index", "Board", new RouteValueDictionary { { "id", board.BoardId } });
            }

            return View(viewModel);
        }
    }
}
