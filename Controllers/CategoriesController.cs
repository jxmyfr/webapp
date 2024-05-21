using DBFirst.Models.db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBFirst.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DemoDbContext _demoDbContext;
        public CategoriesController(DemoDbContext demoDbContext)
        {
            _demoDbContext = demoDbContext;
        }

        // GET: CategoriesController
        public async Task<ActionResult> Index()
        {
            var category = from cat in _demoDbContext.Categories select cat;
            return View(await category.ToListAsync());
        }

        // GET: CategoriesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var category = await _demoDbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _demoDbContext.Categories.Add(obj);
                    await _demoDbContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));

            }
            return View(obj);
        }

        
        

        // GET: CategoriesController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var category = _demoDbContext.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _demoDbContext.Categories.Update(obj);
                    await _demoDbContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
            
        }

        // GET: CategoriesController/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var category = _demoDbContext.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: CategoriesController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfiremed(int id)
        {
            var category = await _demoDbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            _demoDbContext.Categories.Remove(category);
            await _demoDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
