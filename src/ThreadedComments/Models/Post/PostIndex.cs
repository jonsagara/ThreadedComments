using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThreadedComments.Models.Post
{
    public class PostIndex
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public DateTime PostUtcDate { get; set; }

        public List<CommentInfo> Comments { get; set; }

        public class CommentInfo
        {
            public int CommentId { get; set; }
            public string Body { get; set; }
            public string AuthorName { get; set; }
            public string AuthorEmail { get; set; }
            public DateTime CommentUtcDate { get; set; }
            public int Indent { get; set; }
        }
    }
}