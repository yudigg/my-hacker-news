using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HackerRepo;
using MvcMyHackerNews.Models;
using System.Web.Security;
using LoggingRepo;

namespace MvcMyHackerNews.Controllers
{
    public class LoggingController : BaseController
    {

        public ActionResult NewAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewAccount(string username, string passwd)
        {
            AccountsManager mg = new AccountsManager(Properties.Settings.Default.Constr);
            mg.AddUser(username, passwd);
           return RedirectToAction("LogIn");//maybe set auth cookie now
        }
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(string username, string password)
        {
            AccountsManager mg = new AccountsManager(Properties.Settings.Default.Constr);
            User user = mg.GetUser(username, password);
            if (user == null)
            {
                return View();
            }
            FormsAuthentication.SetAuthCookie(user.Username, true);
            return RedirectToAction("News","homePage");
        }
        [Authorize]
        public ActionResult Private()
        {
           

            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("News", "homePage");
        }
    }
}
