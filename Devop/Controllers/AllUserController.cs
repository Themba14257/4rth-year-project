using Devop.Enums;
using Devop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Devop.Controllers
{
    public class AllUserController : Controller
    {

        private UserManager<ApplicationUser> userManager;
        public AllUserController(UserManager<ApplicationUser> userMgr)
        {
            userManager = userMgr;
        }

        [Authorize]
        public IActionResult Index()
            => View(GetData(nameof(Index)));

        [Authorize(Roles = "Patient")]
        public IActionResult OtherAction() 
            => View("Index", GetData(nameof(OtherAction)));

        private Dictionary<string, object> GetData(string actionName)
            => new Dictionary<string, object>
            {
                ["Action"] = actionName,
                ["User"] = HttpContext.User.Identity.Name,
                ["Authenticated"] = HttpContext.User.Identity.IsAuthenticated,
                ["Auth Type"] = HttpContext.User.Identity.AuthenticationType,
                ["In Users Role"] = HttpContext.User.IsInRole("Patient"),
               // ["City"] = CurrentUser.Result.City,
            };

        //[Authorize]
        //public async Task<IActionResult> UserProps()
        //{
        //    return View(await CurrentUser);
        //}
        ////[Authorize]
        //[HttpPost]
        //public async Task<IActionResult> UserProps( [Required] Cities city)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //More can be added
        //        ApplicationUser user = await CurrentUser;
        //        user.City = city;
      
        //        await userManager.UpdateAsync(user);
        //        return RedirectToAction("Index");
        //    }
        //    return View(await CurrentUser);
        //}
        //private Task<ApplicationUser> CurrentUser =>
        //userManager.FindByNameAsync(HttpContext.User.Identity.Name);


    }

    
}
