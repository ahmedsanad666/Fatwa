using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebOS.AuxiliaryClasses;
using WebOS.Data;
using WebOS.Models;

namespace WebOS.Controllers
{
    [Authorize]
    public class ScholarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _environment;

        public ScholarsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Scholars
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ViewBag.Scholars = _context.Scholar.OrderBy(a=>a.Indx);
            return View(await _context.Fatwa.ToListAsync());
        }

        [AllowAnonymous]
        public JsonResult IndexAsJson()
        {
            var Books = _context.Scholar.OrderBy(a=>a.Id);
            return Json(Books);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Search(string keyword)
        {
            var applicationDbContext = _context.Scholar.Where(a => a.Name.Contains(keyword) || a.Bio.Contains(keyword) || a.PositioName.Contains(keyword));
            return View(await applicationDbContext.ToListAsync());
       
        }

        // GET: Scholars/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scholar = await _context.Scholar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scholar == null)
            {
                return NotFound();
            }

            return View(scholar);
        }

        // GET: Scholars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Scholars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PositioName,Name,Bio,Image,Indx")] Scholar scholar, IFormFile myfile)
        {
            if (ModelState.IsValid)
            {
                scholar.Image = await UserFile.UploadeNewImageAsync(scholar.Image,
   myfile, _environment.WebRootPath, Properties.Resources.Images, 500, 500);

                _context.Add(scholar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scholar);
        }

        // GET: Scholars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scholar = await _context.Scholar.FindAsync(id);
            if (scholar == null)
            {
                return NotFound();
            }
            return View(scholar);
        }

        // POST: Scholars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PositioName,Name,Bio,Image,Indx")] Scholar scholar, IFormFile myfile)
        {
            if (id != scholar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    scholar.Image = await UserFile.UploadeNewImageAsync(scholar.Image,
 myfile, _environment.WebRootPath, Properties.Resources.Images, 500, 500);

                    _context.Update(scholar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScholarExists(scholar.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(scholar);
        }

        // GET: Scholars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scholar = await _context.Scholar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scholar == null)
            {
                return NotFound();
            }

            return View(scholar);
        }

        // POST: Scholars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scholar = await _context.Scholar.FindAsync(id);
            _context.Scholar.Remove(scholar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScholarExists(int id)
        {
            return _context.Scholar.Any(e => e.Id == id);
        }
    }
}
