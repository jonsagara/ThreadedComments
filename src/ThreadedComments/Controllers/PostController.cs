using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

    }
}
