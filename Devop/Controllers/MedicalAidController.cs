using Devop.Data;
using Devop.Models;
using Devop.Models.Content;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Devop.Infrastructure.Helper;

namespace Devop.Controllers
{
    public class MedicalAidController : BaseController
    {
        private readonly DevopContext _context;

        public MedicalAidController(DevopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.tblMedicalAid
                .AsNoTracking());
        }
        [NoDirectAccess]
        [HttpGet]
        public async Task<IActionResult> AddOrEdit(string id = null)
        {
            if (id == null)
            {
                return View(new MedicalAid());
            }
            else
            {
                var medicalAid = await _context.tblMedicalAid.FindAsync(id);
                if (medicalAid == null)
                {
                    return NotFound();
                }
                return View(medicalAid);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(string id, string MedicalName, [Bind("MedId,MedicalName ")] MedicalAid medicalAid)
        {
            if (ModelState.IsValid)
            {

                if (id == null)
                {
                    try
                    {
                        var item = _context.tblMedicalAid.Where(p => p.MedicalName.Equals(MedicalName)).FirstOrDefault();
                        if (item == null)
                        {
                            _context.Add(medicalAid);
                            await _context.SaveChangesAsync();
                            Notify(medicalAid.MedicalName + " Medical Aid was added successfully");
                        }
                        else
                        {
                            Notify(item.MedicalName + " already existing in the database", notificationType: NotificationType.error);
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
                        _context.Update(medicalAid);
                        await _context.SaveChangesAsync();
                        Notify(medicalAid.MedicalName + " Suburb was updated successfully");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ModelExists(medicalAid.MedId))
                        {
                            return NotFound();
                        }
                        else
                        { throw; }
                    }
                }
                return RedirectToAction(nameof(Index), medicalAid);
            }
            return View(medicalAid);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var medical = await _context.tblMedicalAid.FindAsync(id);
            try
            {
                if (id != null)
                {
                    _context.tblMedicalAid.Remove(medical);
                    Notify(medical.MedicalName + "  was deleted permanently");
                }
            }
            catch (Exception)
            {
                Notify(medical.MedicalName + " is in use could not be deleted!", notificationType: NotificationType.error);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ModelExists(string id)
        {
            return _context.tblMedicalAid.Any(e => e.MedId == id);
        }

    }
}
