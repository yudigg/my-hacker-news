using LoggingRepo;
using HackerRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMyHackerNews.Models;
using System.Web.Security;

namespace MvcMyHackerNews.Controllers
{
    public class HomePageController : BaseController
    {

        public ActionResult News()
        {
            HomePageViewModel hm;
            NewsItemsManager nm = new NewsItemsManager(Properties.Settings.Default.Constr);
            AccountsManager mg = new AccountsManager(Properties.Settings.Default.Constr);
           var all = nm.GetAllLinks();
           if (!User.Identity.IsAuthenticated)
           {
              hm= new HomePageViewModel{AllLinks = nm.GetAllLinks()};
           }else{
                var user = mg.GetUserByUserName(User.Identity.Name);
                hm = new HomePageViewModel { AllLinks = nm.GetAllLinks() };
            hm.IntLinkList = nm.GetallLinksThatAuthUserVoted(user);
            hm.UserId = user.Id;
           }
            return View(hm);
        }
        
        public ActionResult Submit()
        {
            UpvoteModel upvModel = new UpvoteModel();
            if (!User.Identity.IsAuthenticated)
            {
                //set temp data
                return RedirectToAction("logIn", "Logging");
            }
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Submit(string title, string url)
        {
            NewsItemsManager nm = new NewsItemsManager(Properties.Settings.Default.Constr);
            AccountsManager am = new AccountsManager(Properties.Settings.Default.Constr);
            string username = HttpContext.User.Identity.Name;
            User user = am.GetUserByUserName(username);
            nm.AddLink(url, user.Id, title);
            return View();
        }
      
        public ActionResult Upvote(int linkId, int userId)
        {
            UpvoteModel upvModel = new UpvoteModel();
            if (!User.Identity.IsAuthenticated)
            {
                //set temp data
                return RedirectToAction("logIn","Logging");
            }

            NewsItemsManager nm = new NewsItemsManager(Properties.Settings.Default.Constr);
            upvModel.IsAdded = nm.AddUpvotes(linkId, userId);
           upvModel.Points = nm.GetPointsForLink(linkId);
            return Json(upvModel);
        }
        
    }
}
