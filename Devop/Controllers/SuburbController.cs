using Devop.Data;
using Devop.Models;
using Devop.Models.Content;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Devop.Controllers
{
    public class SuburbController : BaseController
    {
        private readonly DevopContext _context;

        public SuburbController(DevopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.tblSuburb              
                .AsNoTracking());
        }
        //[NoDirectAccess]
        [HttpGet]
        public async Task<IActionResult> AddOrEdit(string id = null)
        {
            if (id == null)
            {
                return View(new Suburb());
            }
            else
            {       
                var cities = await _context.tblCity.FindAsync(id);
                if (cities == null)
                {
                    return NotFound();
                }
                return View(cities);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(string id, string SuburbName, [Bind("SuburbId,SuburbName,CityId")] Suburb suburb)
        {
            if (ModelState.IsValid)
            {

                if (id == null)
                {
                    try
                    {
                        var item = _context.tblSuburb.Where(p => p.SuburbName.Equals(SuburbName)).FirstOrDefault();
                        if (item == null)
                        {
                            _context.Add(suburb);
                            await _context.SaveChangesAsync();
                            Notify(suburb.SuburbName + " Suburb was added successfully");
                        }
                        else
                        {
                            Notify(item.SuburbName + " already existing in the database", notificationType: NotificationType.error);
                            return View(item);
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    try
                    {
                       // PopulateCityDropDownList(suburb.City.c);
                        _context.Update(suburb);
                        await _context.SaveChangesAsync();
                        Notify(suburb.SuburbName + " Suburb was updated successfully");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ModelExists(suburb.SuburbId))
                        {
                            return NotFound();
                        }
                        else
                        { throw; }
                    }
                }
                return RedirectToAction(nameof(Index), suburb);
            }
            return View(suburb);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var cities = await _context.tblSuburb.FindAsync(id);
            try
            {
                if (id != null)
                {
                 //   PopulateCityDropDownList(cities.CityId);
                    _context.tblSuburb.Remove(cities);
                    Notify(cities.SuburbName + " city was deleted permanently");
                }
            }
            catch (Exception)
            {
                Notify(cities.SuburbName + " is in use could not be deleted!", notificationType: NotificationType.error);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ModelExists(string id)
        {
            return _context.tblSuburb.Any(e => e.SuburbId == id);
        }

      
    }
}
