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
using static Devop.Infrastructure.Helper;

namespace Devop.Controllers
{
    public class CityController : BaseController
    {
        private readonly DevopContext _context;

        public CityController(DevopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.tblCity
                .Include(c => c.Suburb)
                .AsNoTracking());
        }

        [NoDirectAccess]
        [HttpPost]
        public async Task<IActionResult> AddOrEdit(string id = null)
        {
            if(id == null)
            {
                PopulateSuburbDropDownList();
                return View(new City());
            }
            else
            {
                PopulateSuburbDropDownList();
                var cities = await _context.tblCity.FindAsync(id);
                if(cities == null)
                {
                    return NotFound();
                }
                return View(cities);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit (string id, string CityName, [Bind("CityId, CityName, SuburbId")] City cities)
        {
            if(ModelState.IsValid)
            {
                if(id == null)
                {
                    try
                    {
                        var item = _context.tblCity.Where(p => p.CityName.Equals(CityName)).FirstOrDefault();
                        if (item == null)
                        {
                            _context.Add(cities);
                            await _context.SaveChangesAsync();
                            Notify(cities.CityName + " city was added successfully");

                        }
                        else
                        {
                            Notify(item.CityName + " already existing in the database", notificationType: NotificationType.error);
                            return View(item);
                        }
                    }
                    catch(Exception)
                    {

                    }
                }
                else
                {
                    try
                    {
                        PopulateSuburbDropDownList(cities.SuburbId);
                        _context.Update(cities);
                        await _context.SaveChangesAsync();
                        Notify(cities.CityName + " city was updated successfully");

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ModelExists(cities.CityId))
                        {
                            return NotFound();
                        }
                        else
                            throw;
                    }
                }
            }
            return RedirectToAction(nameof(Index), cities);

        }
        public async Task<IActionResult> Delete(string id)
        {
            var cities = await _context.tblCity.FindAsync(id);
            try
            {
                if (id != null)
                {
                    PopulateSuburbDropDownList(cities.SuburbId);
                    _context.tblCity.Remove(cities);
                    Notify(cities.CityName + " city was deleted permanently");
                }
            }
            catch (Exception)
            {
                Notify(cities.CityName + " is in use could not be deleted!", notificationType: NotificationType.error);
            }
            return RedirectToAction(nameof(Index));
        }

        //Method to Populate DropDown List
        private void PopulateSuburbDropDownList(object selectSuburbName = null )
        {
            var SuburbQuery = from d in _context.tblSuburb.Distinct().AsNoTracking()
                           orderby d.SuburbName
                           select d;

            ViewBag.SuburbId = new SelectList(SuburbQuery.AsNoTracking(), "SuburbId", "SuburbName", selectSuburbName);


        }
        private bool ModelExists(string id)
        {
            return _context.tblCity.Any(e => e.CityId == id);
        }


    }
}
