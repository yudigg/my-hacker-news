using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMyHackerNews.Models;
using HackerRepo;
using System.Web.Security;
namespace MvcMyHackerNews.Controllers
{
    public class BaseController : Controller
    {
        protected override ViewResult View(IView view, object model)
        {
            if (model == null)
            {
                model = new BaseViewModel();
            }
            if (User.Identity.IsAuthenticated)
            {
                var bvm = (BaseViewModel)model;
                AccountsManager mg = new AccountsManager(Properties.Settings.Default.Constr);
                var user = mg.GetUserByUserName(User.Identity.Name);
                bvm.Username = user.Username;
            }
            return base.View(view, model);
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {

            if (model == null)
            {
                model = new BaseViewModel();
            }
            if (User.Identity.IsAuthenticated)
            {
                var bvm = (BaseViewModel)model;
                AccountsManager mg = new AccountsManager(Properties.Settings.Default.Constr);
                var user = mg.GetUserByUserName(User.Identity.Name);
                bvm.Username = user.Username;
            }
            return base.View(viewName, masterName, model);
        }

    }
}
