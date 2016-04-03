using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMyHackerNews.Models
{
    public class UpvoteModel
    {
        public bool IsAdded { get; set; }
        public double Points { get; set; }
    }
}