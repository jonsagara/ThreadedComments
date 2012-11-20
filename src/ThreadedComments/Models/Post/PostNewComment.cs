using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ThreadedComments.Models.Post
{
    public class PostNewComment
    {
        public int PostId { get; set; }
        public int BoardId { get; set; }
        public int? ParentCommentId { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public string AuthorName { get; set; }

        [Required]
        public string AuthorEmail { get; set; }
    }
}