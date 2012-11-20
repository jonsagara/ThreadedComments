using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThreadedComments.Models.Board
{
    public class BoardIndex
    {
        public int BoardId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<PostInfo> Posts { get; set; }

        public class PostInfo
        {
            public int PostId { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
            public string AuthorName { get; set; }
            public string AuthorEmail { get; set; }
            public DateTime PostUtcDate { get; set; }
        }
    }
}