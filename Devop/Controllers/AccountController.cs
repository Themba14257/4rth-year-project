using Devop.Data;
using Devop.Models;
using Devop.Models.ViewModel.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Devop.Controllers
{

   
    public class AccountController : BaseController
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private readonly DevopContext _context;
        public AccountController(DevopContext context, UserManager<ApplicationUser> userMgr, SignInManager<ApplicationUser> signinMgr)
        {
            _context = context;
            userManager = userMgr;
            signInManager = signinMgr;
        }
        //------------------------------
        public ViewResult Create() => View();
        [HttpPost]

        public async Task<IActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.FirstName,

                };

                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var isSave = await userManager.AddToRoleAsync(user, role: "Patient");
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(AccountController.Login), "Account");

                   // return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
        /// <summary>
        /// -----------------------------------
        /// </summary>
        /// <returns></returns>

        [AllowAnonymous]
        public IActionResult Login(/*string returnUrl*/)
        {
            //ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details/*string returnUrl*/)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(
                    user, details.Password, false, false);
                    if (result.Succeeded)
                    {
                        //  return Redirect(returnUrl ?? "/");
                        return RedirectToAction(nameof(AdminController.Create), "RoleAdmin");
                        
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.Email),  "Invalid user or password");
            }
            return View(details);
        }
        [HttpGet]
        public async Task<IActionResult> EditUser()
        {
            var user = await userManager.GetUserAsync(User);
            if(User == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }
           

            var model = new EditUserModel
            {
               // Id = user.Id,
                Dob = user.Dob,
                Email = user.Email,
                Idnumber = user.Idnumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                AddressLine1 = user.AddressLine1,
                AddressLine2 = user.AddressLine2,
                CityId = user.CityId,
                PostalCode = user.PostalCode,
                SuburbId = user.SuburbId

            };
            PopulateSuburbDropDownList(model.SuburbId);
            PopulateCityDropDownList(model.CityId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserModel model)
        {
            PopulateCityDropDownList();
            PopulateSuburbDropDownList();

            if(!ModelState.IsValid)
            {
                Notify("All field are required", notificationType: NotificationType.warning);
                return View(model);
            }
            var user = await userManager.GetUserAsync(User);

            if(user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }
            var email = user.Email;
            if(model.Email != email)
            {
                var setEmailResult = await userManager.SetEmailAsync(user, model.Email);
                if(setEmailResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }
            var phoneNumber = user.PhoneNumber;
            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
                }
            }

            user.Dob = model.Dob;
            user.Email = model.Email;
            user.Idnumber = model.Idnumber;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Gender = model.Gender;
            user.AddressLine1 = model.AddressLine1;
            user.AddressLine2 = model.AddressLine2;
            user.CityId = model.CityId;
            user.PostalCode = model.PostalCode;
            user.SuburbId = model.SuburbId;

            PopulateCityDropDownList(model.CityId);
            PopulateSuburbDropDownList(model.SuburbId);

            await userManager.UpdateAsync(user);

            await signInManager.RefreshSignInAsync(user);
            Notify("Your profile was updated successfully");
            return RedirectToAction(nameof(UserDetails));


        }
        [HttpGet]
        public IActionResult UserDetails()
        {
            var userId = userManager.GetUserId(HttpContext.User);

            if (userId == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                ApplicationUser user = userManager.FindByIdAsync(userId).Result;
                return View(user);
            }

        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
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
        private void PopulateCityDropDownList(object selectCityName = null)
        {
            var CityQuery = from d in _context.tblCity
                              orderby d.CityName
                              select d;
            ViewBag.CityId = new SelectList(CityQuery.AsNoTracking(), "CityId", "CityName", selectCityName);

        }

        private void PopulateSuburbDropDownList(object selectSuburbName = null)
        {
            var SuburbQuery = from d in _context.tblSuburb
                              orderby d.SuburbName
                              select d;

            ViewBag.SuburbId = new SelectList(SuburbQuery.AsNoTracking(), "SuburbId", "SuburbName", selectSuburbName);


        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
