using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebOS.Data;
using WebOS.Models;

namespace WebOS.Controllers
{
    
    public class FatwasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FatwasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Fatwas
      
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Fatwa.Include(f => f.Category).Include(f => f.Scholar).OrderByDescending(a=>a.Id).Take(20);
            return View(await applicationDbContext.ToListAsync());
        }

     
        public async Task<IActionResult> IndexAsJson()
        {
            var applicationDbContext = _context.Fatwa.Include(f => f.Category).Include(f => f.Scholar).OrderByDescending(a=>a.Id).Take(20);
            return Json(await applicationDbContext.ToListAsync());
        }

     
        public async Task<IActionResult> Search(string keyword)
        {
            var applicationDbContext = _context.Fatwa.Where(a => a.Title.Contains(keyword) || a.Question.Contains(keyword) || a.Answer.Contains(keyword) || a.Tags.Contains(keyword) || a.Scholar.Name.Contains(keyword)).Include(f => f.Category).Include(f => f.Scholar);
            return View(await applicationDbContext.ToListAsync());
        }

        public JsonResult SearchAsJson(string keyword)
        {
            var applicationDbContext = _context.Fatwa.Where(a => a.Title.Contains(keyword) || a.Question.Contains(keyword) || a.Answer.Contains(keyword) || a.Tags.Contains(keyword) || a.Scholar.Name.Contains(keyword)).Include(f => f.Category).Include(f => f.Scholar);
            return Json(applicationDbContext);
        }

        public async Task<IActionResult> IndexByCategory(int Id)
        {
            var applicationDbContext = _context.Fatwa.Where(a => a.CategoryId == Id).Include(f => f.Category).Include(f => f.Scholar).OrderBy(a=>a.Id);
            ViewData["Name"] = _context.Category.SingleOrDefault(Category => Category.Id == Id).Name;    
            return View(await applicationDbContext.ToListAsync());
        }

        public JsonResult IndexByCategoryAsJson(int Id)
        {
            var applicationDbContext = _context.Fatwa.Where(a => a.CategoryId == Id).Include(f => f.Category).Include(f => f.Scholar);
            return Json(applicationDbContext);
        }

        public async Task<IActionResult> IndexByScholars(int Id)
        {
            var applicationDbContext = _context.Fatwa.Where(a => a.ScholarId == Id).Include(f => f.Category).Include(f => f.Scholar).OrderBy(a => a.Id);
            ViewData["Name"] = _context.Scholar.SingleOrDefault(a=>a.Id== Id).Name;
            return View(await applicationDbContext.ToListAsync());
        }

        public JsonResult IndexByScholarsAsJson(int Id)
        {
            var applicationDbContext = _context.Fatwa.Where(a => a.ScholarId == Id).Include(f => f.Category).Include(f => f.Scholar);
            return Json(applicationDbContext);
        }

        // GET: Fatwas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fatwa = await _context.Fatwa
                .Include(f => f.Category)
                .Include(f => f.Scholar)
                .FirstOrDefaultAsync(m => m.Id == id);



            if (fatwa == null)
            {
                return NotFound();
            }
            fatwa.Reads++;
            await _context.SaveChangesAsync();
            return View(fatwa);
        }

        public JsonResult DetailsAsJson(int? id)
        {
            var fatwa =  _context.Fatwa
               .Include(f => f.Category)
               .Include(f => f.Scholar)
               .FirstOrDefaultAsync(m => m.Id == id);

            return Json(fatwa);
        }

        // GET: Fatwas/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            ViewData["ScholarId"] = new SelectList(_context.Scholar, "Id", "Name");
            return View();
        }

        // POST: Fatwas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,ScholarId,Title,Question,Answer,Tags,Reads")] Fatwa fatwa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fatwa);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Edit), new { id = fatwa.Id });
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", fatwa.CategoryId);
            ViewData["ScholarId"] = new SelectList(_context.Scholar, "Id", "Name", fatwa.ScholarId);
           
            return View(fatwa);
        }

        // GET: Fatwas/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fatwa = await _context.Fatwa.FindAsync(id);
            if (fatwa == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", fatwa.CategoryId);
            ViewData["ScholarId"] = new SelectList(_context.Scholar, "Id", "Name", fatwa.ScholarId);
            return View(fatwa);
        }

        // POST: Fatwas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,ScholarId,Title,Question,Answer,Tags,Reads")] Fatwa fatwa)
        {
            if (id != fatwa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fatwa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FatwaExists(fatwa.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", fatwa.CategoryId);
            ViewData["ScholarId"] = new SelectList(_context.Scholar, "Id", "Name", fatwa.ScholarId);
            return View(fatwa);
        }

        // GET: Fatwas/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fatwa = await _context.Fatwa
                .Include(f => f.Category)
                .Include(f => f.Scholar)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fatwa == null)
            {
                return NotFound();
            }

            return View(fatwa);
        }

        // POST: Fatwas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fatwa = await _context.Fatwa.FindAsync(id);
            _context.Fatwa.Remove(fatwa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FatwaExists(int id)
        {
            return _context.Fatwa.Any(e => e.Id == id);
        }
    }
}
