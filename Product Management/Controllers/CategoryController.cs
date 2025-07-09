using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Product_Management.Data;
using Product_Management.Models;

namespace Product_Management.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
  

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
         
        }


        // ........... This return all categories ............ //
        private const int PageSize = 10;  

        public async Task<IActionResult> Index(int page = 1)
        {
            var cats = _context.Categories.OrderBy(c => c.CategoryName).AsQueryable();

            ///-----This getting totalcount for category.........//
            var totalCount = await cats.CountAsync();

            //-----This fetching only ten items for current page----//
            var categories = await cats.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();


            //--------Sending categories and pagination items with view bags-----//
            ViewBag.Categories = categories;
            ViewBag.CurrentPage = page;
            ViewBag.TotalCount = totalCount;
            ViewBag.PageSize = PageSize;

            return View();
        }



        //----------This return detail category................//
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (category == null) return NotFound();

            return View(category);
        }


        //...........This is to show for category add form.........//
        [HttpGet]
        public IActionResult Create()
        {
           
            return View();
        }


        // ..............This create category to store in database.........//
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryNo,CategoryName,Active")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    category.CreatedDate = DateTime.Now;
                    _context.Add(category); //---This make add process----//
                    await _context.SaveChangesAsync();

                  
                    TempData["SuccessMessage"] = "Category “" + category.CategoryName + "” was created successfully.";

                    return RedirectToAction(nameof(Create));
              
                }
                catch (Exception ex)
                {
                   
                    TempData["ErrorMessage"] = "An error occurred while creating the category. Please try again.";
                 
                }
            }
           

            return View();
        }


        // ..................This process is to show information of category to make edit process....//
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // ..........This makes update process..........//
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryID,CategoryNo,CategoryName,Active")] Category cat)
        {
            if (id != cat.CategoryID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    //.....--this finidng category----..//
                    var category = await _context.Categories.FindAsync(id);

                    if (category ==null)
                    {
                        return NotFound();
                    }

                    category.Active = cat.Active;
                    category.CategoryNo = cat.CategoryNo;
                    category.CategoryName = cat.CategoryName;

                    await _context.SaveChangesAsync();
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Categories.AnyAsync(e => e.CategoryID == id))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cat);
        }



        //.............This makes delete process....----//
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}