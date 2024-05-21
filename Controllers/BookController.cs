using DBFirst.Models.db;
using DBFirst.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DBFirst.Controllers
{
    public class BookController : Controller
    {
        private readonly DemoDbContext _demoDbContext;

        public BookController(DemoDbContext demoDbContext)
        {
            _demoDbContext = demoDbContext;
        }
        public async Task<ActionResult> Index()
        {
            var bs = _demoDbContext.Books
                .Include(c => c.Category)
                .Include(p => p.Publish);
            if (bs == null) 
            {
                return NotFound();
            }
            return View(await bs.ToListAsync());
        }

        public async Task<ActionResult> IndexViewModel()
        {
            var bcp = from b in _demoDbContext.Books
                      from c in _demoDbContext.Categories
                      from p in _demoDbContext.Publishes
                      where (b.CategoryId == c.CategoryId)
                      && (b.PublishId == p.PublishId)
                      select new BookCategoryPublisherViewModel
                      {
                          BookId = b.BookId,
                          BookName = b.BookName,
                          Isbn = b.Isbn,
                          CategoryName = c.CategoryName,
                          PublishName = p.PublishName,
                          BookCost = b.BookCost,
                          BookPrice = b.BookPrice
                      };
            return View(await bcp.ToListAsync());
        }

        public ActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_demoDbContext.Categories, "CategoryId", "CategoryName");
            ViewData["PublishId"] = new SelectList(_demoDbContext.Publishes, "PublishId", "PublishName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("BookId, BookName, CategoryId, PublishId, Isbn, BookCost, BookPrice")] Book book,
            Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(_demoDbContext.Categories, "CategoryId", "CategoryName");
                ViewData["PublishId"] = new SelectList(_demoDbContext.Publishes, "PublishId", "PublishName");
                return View(book);
            }
            book.BookId = Guid.NewGuid().ToString();
            _demoDbContext.Books.Add(book);
            await _demoDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = await _demoDbContext.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_demoDbContext.Categories, "CategoryId", "CategoryName");
            ViewData["PublishId"] = new SelectList(_demoDbContext.Publishes, "PublishId", "PublishName");
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, [Bind("BookId, BookName, CategoryId, PublishId, Isbn, BookCost, BookPrice")] Book book,
            Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            if (id != book.BookId) { 
                return NotFound();
            }
            if (modelState.IsValid)
            {
                try
                {
                    _demoDbContext.Books.Update(book);
                    await _demoDbContext.SaveChangesAsync();
                } catch(DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
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
            ViewData["CategoryId"] = new SelectList(_demoDbContext.Categories, "CategoryId", "CategoryName");
            ViewData["PublishId"] = new SelectList(_demoDbContext.Publishes, "PublishId", "PublishName");
            return View(book);
        }

        private bool BookExists(string id)
        {
            return _demoDbContext.Books.Any(x => x.BookId == id);
        }

        public async Task<ActionResult> Details(string id)
        {
            if (id== null)
            {
                return NotFound();
            }

            var book = await _demoDbContext.Books
                .Include(c => c.Category)
                .Include(p => p.Publish)
                .FirstOrDefaultAsync(b=>b.BookId== id);
            if (book == null) 
            { 
                return NotFound();
            }
            return View(book);
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _demoDbContext.Books
                .Include(c => c.Category)
                .Include(p => p.Publish)
                .FirstOrDefaultAsync(b=>b.BookId== id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var book = await _demoDbContext.Books.FindAsync(id);
            _demoDbContext.Books.Remove(book);
            await _demoDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Search(string q = "")
        {
            var bcp = (from b in _demoDbContext.Books
                       from c in _demoDbContext.Categories
                       from p in _demoDbContext.Publishes
                       where (b.BookName.Contains(q))
                       && (b.CategoryId == c.CategoryId)
                       && (b.PublishId == p.PublishId)
                       select new BookCategoryPublisherViewModel
                       {
                           BookId = b.BookId,
                           BookName = b.BookName,
                           Isbn = b.Isbn,
                           CategoryName = c.CategoryName,
                           PublishName = p.PublishName,
                           BookCost = b.BookCost,
                           BookPrice = b.BookPrice
                       }).ToListAsync();
            return View(await bcp);
        }
    }
}
