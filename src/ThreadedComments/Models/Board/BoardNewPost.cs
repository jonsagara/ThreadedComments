using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ThreadedComments.Models.Board
{
    public class BoardNewPost
    {
        public int BoardId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Body { get; set; }

        [Required]
        public string AuthorName { get; set; }

        [Required]
        public string AuthorEmail { get; set; }
    }
}