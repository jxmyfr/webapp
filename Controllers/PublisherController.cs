using DBFirst.Models.db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBFirst.Controllers
{
    public class PublisherController : Controller
    {
        private readonly DemoDbContext _dbcontext;

        public PublisherController(DemoDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

		public async Task<ActionResult> Index()
        {
            var allP = from publi in _dbcontext.Publishes select publi;
            if (allP == null)
            {
                return NotFound();
            }
            return View(await allP.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<ActionResult> Create(Publish publish)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _dbcontext.Publishes.Add(publish);
                    await _dbcontext.SaveChangesAsync();
                } catch(Exception)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(publish);
        }
        
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var publish = await _dbcontext.Publishes.FirstOrDefaultAsync(p => p.PublishId == id);
            if (publish == null)
            {
                return NotFound();
            }
            return View(publish);
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var publish = await _dbcontext.Publishes.FirstOrDefaultAsync(p => p.PublishId == id);
            if (publish == null) 
            { 
                return NotFound();
            }
            return View(publish);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Publish publish)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _dbcontext.Publishes.Update(publish);
                    await _dbcontext.SaveChangesAsync();
                    
                } catch(Exception)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(publish);
        }
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var publish = await _dbcontext.Publishes.FirstOrDefaultAsync(p => p.PublishId == id);
            if (publish == null)
            {
                return NotFound();
            }
            return View(publish);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfiremed(int id)
        {
            var publish = await _dbcontext.Publishes.FirstOrDefaultAsync(p => p.PublishId == id);
            _dbcontext.Publishes.Remove(publish);
            await _dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
