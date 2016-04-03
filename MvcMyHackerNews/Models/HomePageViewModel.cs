using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HackerRepo;
using LoggingRepo;

namespace MvcMyHackerNews.Models
{
    public class HomePageViewModel : BaseViewModel
    {
        public IEnumerable<LinkAndRatings> AllLinks { get; set; }
        public IEnumerable<int> IntLinkList { get; set; }
        public int UserId { get; set; }
    }
}