using Devop.Data;
using Devop.Enums;
using Devop.Models;
using Devop.Models.ViewModel.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Devop.Controllers
{
    public class AllUserController : BaseController
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly DevopContext _context;
        private readonly IWebHostEnvironment hostEnvironment;
        public AllUserController(RoleManager<IdentityRole> roleMgr, UserManager<ApplicationUser> userMgr, SignInManager<ApplicationUser> signInManager, DevopContext context, IWebHostEnvironment hostEnvironment)
        {
            roleManager = roleMgr;
            userManager = userMgr;
            this.signInManager = signInManager;
            _context = context;
            this.hostEnvironment = hostEnvironment;
        }

        public ViewResult Index()
            => View(userManager.Users);

        public async Task<IActionResult> AllNurse()
        {
            List<ViewModelUserRole> nurse = new List<ViewModelUserRole>();
            var n = await userManager.GetUsersInRoleAsync("Nurse");

            foreach(var x in n)
            {
                nurse.Add(new ViewModelUserRole
                {
                    Title = x.Title,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    AddressLine1 = x.AddressLine1,
                    AddressLine2 = x.AddressLine2
                });
            }
            return View(nurse);
        }
        public async Task<IActionResult> AllPatient()
        {
            List<ViewModelUserRole> patient = new List<ViewModelUserRole>();
            var n = await userManager.GetUsersInRoleAsync("Patient");

            foreach (var x in patient)
            {
                patient.Add(new ViewModelUserRole
                {
                    Title = x.Title,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    AddressLine1 = x.AddressLine1,
                    AddressLine2 = x.AddressLine2
                });
            }
            return View(patient);
        }
        public async Task<IActionResult> LabUser()
        {
            List<ViewModelUserRole> labUser = new List<ViewModelUserRole>();
            var n = await userManager.GetUsersInRoleAsync("LabUser");

            foreach (var x in labUser)
            {
                labUser.Add(new ViewModelUserRole
                {
                    Title = x.Title,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    AddressLine1 = x.AddressLine1,
                    AddressLine2 = x.AddressLine2
                });
            }
            return View(labUser);
        }
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                Notify("User can't be deleted most activities depends on", notificationType: NotificationType.error);
                return View(nameof(Index));
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("Index");
            }

        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            PopulateCityDropDownList();
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                Notify("User Not Found", notificationType: NotificationType.error);
                return View("Index");
            }

            // GetClaimsAsync retunrs the list of user Claims
            var userClaims = await userManager.GetClaimsAsync(user);
            // GetRolesAsync returns the list of user Roles
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUserModel
            {

               
                Dob = user.Dob,
                Email = user.Email,
                Title = user.Title,
                Gender = user.Gender,
                CityId = user.CityId,
                Idnumber = user.Idnumber,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,            
                AddressLine1 = user.AddressLine1,
                AddressLine2 = user.AddressLine2,
              
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserModel model)
        {
          
            PopulateCityDropDownList();
           
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Dob = model.Dob;                
                user.Title = model.Title;
                user.Gender = model.Gender;
                user.CityId = model.CityId;
                user.Idnumber = model.Idnumber;             
                user.FirstName = model.FirstName;             
                user.AddressLine1 = model.AddressLine1;
                user.AddressLine2 = model.AddressLine2;
               

                PopulateCityDropDownList(model.CityId);
               
                await userManager.UpdateAsync(user);

                //await signInManager.RefreshSignInAsync(user);
                Notify("Your profile was updated successfully");


                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(AllUserController.Index), "AllUser");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
          
        }
       
        private void PopulateCityDropDownList(object selectedCity = null)
        {
            var CityQuery = from d in _context.tblCity
                            orderby d.CityName
                            select d;
            ViewBag.CityId = new SelectList(CityQuery.AsNoTracking(), "CityId", "CityName",
            selectedCity);
        }






        //[Authorize]
        //public IActionResult Index()
        //    => View(GetData(nameof(Index)));

        //[Authorize(Roles = "Patient")]
        //public IActionResult OtherAction() 
        //    => View("Index", GetData(nameof(OtherAction)));

        //private Dictionary<string, object> GetData(string actionName)
        //    => new Dictionary<string, object>
        //    {
        //        ["Action"] = actionName,
        //        ["User"] = HttpContext.User.Identity.Name,
        //        ["Authenticated"] = HttpContext.User.Identity.IsAuthenticated,
        //        ["Auth Type"] = HttpContext.User.Identity.AuthenticationType,
        //        ["In Users Role"] = HttpContext.User.IsInRole("Patient"),
        //       // ["City"] = CurrentUser.Result.City,
        //    };

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
