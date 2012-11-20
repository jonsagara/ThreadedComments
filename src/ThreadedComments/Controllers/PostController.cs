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

        public ActionResult NewComment(int? id, int? parentCommentId)
        {
            var post = ThreadedCommentsContext.Posts
                .Include("Board")
                .SingleOrDefault(p => p.PostId == id);

            if (post == null)
            {
                throw new Exception("No Post with id = " + id);
            }

            if (parentCommentId != null)
            {
                var parentComment = ThreadedCommentsContext.Comments.SingleOrDefault(c => c.CommentId == parentCommentId);
                if (parentComment == null)
                {
                    throw new Exception("No parent Comment with id = " + parentCommentId);
                }
            }

            return View(new PostNewComment { PostId = post.PostId, BoardId = post.BoardId, ParentCommentId = parentCommentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewComment(PostNewComment viewModel)
        {
            const string StartingParentLevelSortOrder = "9999999";
            const string StartingChildLevelSortOrder = "0000001";

            var post = ThreadedCommentsContext.Posts
                .Include("Board")
                .SingleOrDefault(p => p.PostId == viewModel.PostId);

            if (post == null)
            {
                throw new Exception("No Post with id = " + viewModel.PostId);
            }

            Comment parentComment = null;
            if (viewModel.ParentCommentId != null)
            {
                parentComment = ThreadedCommentsContext.Comments.SingleOrDefault(c => c.CommentId == viewModel.ParentCommentId);
                if (parentComment == null)
                {
                    throw new Exception("No parent Comment with id = " + viewModel.ParentCommentId);
                }
            }

            if (ModelState.IsValid)
            {
                var comment = new Comment();
                comment.PostId = viewModel.PostId;
                comment.BoardId = viewModel.BoardId;
                comment.ParentCommentId = viewModel.ParentCommentId;
                comment.Body = viewModel.Body.Trim();
                comment.AuthorName = viewModel.AuthorName.Trim();
                comment.AuthorEmail = viewModel.AuthorEmail.Trim();
                comment.CommentUtcDate = DateTime.UtcNow;

                if (parentComment != null)
                {
                    // See if this parent has other comments that are siblings of this new comment. 
                    //  If so, we'll use the latest one to set our sort order.
                    var otherChildComments = ThreadedCommentsContext.Comments
                        .Where(c => c.PostId == viewModel.PostId && c.BoardId == viewModel.BoardId && c.ParentCommentId == parentComment.CommentId)
                        .OrderByDescending(c => c.SortOrder)
                        .Take(1)
                        .ToList();

                    string sortOrder = StartingChildLevelSortOrder;
                    if (otherChildComments.Count > 0)
                    {
                        var lastChildComment = otherChildComments.Single();
                        var lastChildCommentSortOrderParts = lastChildComment.SortOrder.Split(new char[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
                        if (lastChildCommentSortOrderParts.Length == 0)
                        {
                            throw new Exception("Parent comment with id = " + lastChildComment.CommentId + " has an invalid SortOrder = " + lastChildComment.SortOrder);
                        }

                        sortOrder = (Convert.ToInt32(lastChildCommentSortOrderParts.Last()) + 1).ToString("0000000");
                    }

                    comment.SortOrder = parentComment.SortOrder + "." + sortOrder;
                    comment.Indent = parentComment.Indent + 1;
                }
                else
                {
                    // See if this Post has other top-level comments that are siblings of this new
                    //  comment. If so, we'll use the latest one to set our sort order.
                    var otherTopLevelComments = ThreadedCommentsContext.Comments
                        .Where(c => c.PostId == viewModel.PostId && c.BoardId == viewModel.BoardId && c.ParentCommentId == null)
                        .OrderBy(c => c.SortOrder)
                        .Take(1)
                        .ToList();

                    string sortOrder = StartingParentLevelSortOrder;
                    if (otherTopLevelComments.Count > 0)
                    {
                        var lastTopLevelComment = otherTopLevelComments.Single();
                        if (lastTopLevelComment.SortOrder.Length != 7)
                        {
                            throw new Exception("Top-level comment with id = " + lastTopLevelComment.CommentId + " has an invalid SortOrder = " + lastTopLevelComment.SortOrder + ". It should only consist of 7 integer characters.");
                        }

                        sortOrder = (Convert.ToInt32(lastTopLevelComment.SortOrder) - 1).ToString("0000000");
                    }

                    comment.SortOrder = sortOrder;
                    comment.Indent = 0;
                }

                ThreadedCommentsContext.Comments.Add(comment);

                return RedirectToAction("Index", "Post", new RouteValueDictionary {{"id", viewModel.PostId}});
            }

            return View(viewModel);
        }
    }
}
