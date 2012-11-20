using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThreadedComments.Models.Home
{
    public class HomeIndex
    {
        public List<BoardInfo> Boards { get; set; }

        public class BoardInfo
        {
            public int BoardId { get; set; }
            public string Name { get; set; }
        }
    }
}