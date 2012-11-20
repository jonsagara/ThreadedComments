using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ThreadedComments.Data.Entities;
using ThreadedComments.Models.Post;

namespace ThreadedComments.Controllers
{
    public class PostController : ThreadedCommentsBaseController
    {
        //
        // GET: /Post/

        public ActionResult Index(int? id)
        {
            var post = ThreadedCommentsContext.Posts
                .Include("Board")
                .SingleOrDefault(p => p.PostId == id);

            if (post == null)
            {
                throw new Exception("No Post with id = " + id);
            }

            var comments = ThreadedCommentsContext.Comments
                .Where(c => c.PostId == id && c.BoardId == post.BoardId)
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.CommentUtcDate)
                .Take(50)
                .ToList()
                .Select(c => new PostIndex.CommentInfo
                            {
                                CommentId = c.CommentId,
                                Body = c.Body,
                                AuthorName = c.AuthorName,
                                AuthorEmail = c.AuthorEmail,
                                CommentUtcDate = c.CommentUtcDate,
                                Indent = c.Indent
                            })
                .ToList();

            var viewModel = new PostIndex();
            viewModel.PostId = post.PostId;
            viewModel.Title = post.Title;
            viewModel.Body = post.Body;
            viewModel.AuthorName = post.AuthorName;
            viewModel.AuthorEmail = post.AuthorEmail;
            viewModel.PostUtcDate = post.PostUtcDate;
            viewModel.Comments = comments;

            return View(viewModel);
        }

        public ActionResult NewComment(int? id)
        {
            var post = ThreadedCommentsContext.Posts
                .Include("Board")
                .SingleOrDefault(p => p.PostId == id);

            if (post == null)
            {
                throw new Exception("No Post with id = " + id);
            }

            return View(new PostNewComment { PostId = post.PostId, BoardId = post.BoardId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewComment(PostNewComment viewModel)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment();
                comment.PostId = viewModel.PostId;
                comment.BoardId = viewModel.BoardId;
                comment.ParentCommentId = null;//viewModel.ParentCommentId;
                comment.Body = viewModel.Body.Trim();
                comment.AuthorName = viewModel.AuthorName.Trim();
                comment.AuthorEmail = viewModel.AuthorEmail.Trim();
                comment.CommentUtcDate = DateTime.UtcNow;

                comment.SortOrder = "0000001";
#warning hard coded for now
                comment.Indent = 0;

                ThreadedCommentsContext.Comments.Add(comment);

                return RedirectToAction("Index", "Post", new RouteValueDictionary {{"id", viewModel.PostId}});
            }

            return View(viewModel);
        }
    }
}
