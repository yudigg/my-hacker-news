using LoggingRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerRepo
{
   public class NewsItemsManager
    {
       private readonly string _connectionString;
       public NewsItemsManager(string connectionString)
       {
           _connectionString = connectionString;
       }
     
       public void AddLink(string url,int userId,string title)
       {           
           Link link = new Link();
           link.Url = url;
           link.Title = title;
           link.UserId = userId;
           string slug = title.GenerateSlug();
           link.Slug = slug;
           link.DateTime = DateTime.Now;
           using(var dc = new HackerNewsDataContext(_connectionString))
           {
               dc.Links.InsertOnSubmit(link);
               dc.SubmitChanges();
           }
       }
       public IEnumerable<LinkAndRatings> GetAllLinks()
       {
           using(var dc = new HackerNewsDataContext(_connectionString))
           {
               var loadOptions = new System.Data.Linq.DataLoadOptions();
               loadOptions.LoadWith<Link>(l => l.User);
               loadOptions.LoadWith<Link>(l => l.Upvotes);
               dc.LoadOptions = loadOptions;
               IEnumerable<Link> allLinks = dc.Links.Where(l =>l.DateTime > DateTime.Now.AddDays(-1)).ToList();
              
               //var isAdded = dc.Upvotes.Any(v => v.UserId == userId && v.LinkId == linkId);
               return allLinks.Select(l => new LinkAndRatings { Points = GetPointsForLink(l.Id), Score = GetScore(l),
               DateTime = l.DateTime,Slug =l.Slug,UserId = l.UserId,Title = l.Title,Id =l.Id,Url = l.Url,Upvotes = l.Upvotes,User=l.User}).OrderByDescending(l => l.Score);
           }
       }
       public IEnumerable<Link> GetByAuthor(int userId)//slug?/partial
       {
           using (var dc = new HackerNewsDataContext(_connectionString))
           {
               var loadOptions = new System.Data.Linq.DataLoadOptions();
               loadOptions.LoadWith<Link>(l => l.User);
               loadOptions.LoadWith<Link>(l => l.Upvotes);
               dc.LoadOptions = loadOptions;
               return dc.Links.Where(l => l.UserId == userId ).ToList();
           }
       }
       public bool AddUpvotes(int linkId, int userId)
       {
           using (var dc = new HackerNewsDataContext(_connectionString))
           {
               if(IsAlreadyUpvoted(linkId,userId))
               {
                   return false;
               }
               Upvote upvote = new Upvote { LinkId = linkId, UserId = userId };
               dc.Upvotes.InsertOnSubmit(upvote);
               dc.SubmitChanges();
               return true;
           }
       }
       public double GetPointsForLink(int linkId)
       {
           using(var dc = new HackerNewsDataContext(_connectionString))
           {
              return dc.Upvotes.Count(v => v.LinkId == linkId);
           }
       }
       private bool IsAlreadyUpvoted(int linkId,int userId)
       {
           using (var dc = new HackerNewsDataContext(_connectionString))
           {
               return dc.Upvotes.Any(v => v.UserId == userId && v.LinkId == linkId);
           }
       }
       public double GetScore(Link link)
       {
           var p = GetPointsForLink(link.Id);
           var d = (DateTime.Now.Hour - link.DateTime.Hour) + 2;///turn in to hour after compute
         var g = Math.Pow(d,1.8);
         var x = (p - 1) / g;
         return x;
       }
     public IEnumerable<int> GetallLinksThatAuthUserVoted(User user)
       {
           List<Link> result = new List<Link>();
           using (var dc = new HackerNewsDataContext(_connectionString))
           {
               var x = dc.Upvotes.Where(v => v.UserId == user.Id).Select(v => v.LinkId).ToList();
               return x;
           } 
       }
    }
}