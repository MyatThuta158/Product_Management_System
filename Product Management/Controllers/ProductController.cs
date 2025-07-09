using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Product_Management.Data;
using Product_Management.Models;
using X.PagedList.Extensions;
using X.PagedList.Mvc.Core;

namespace Product_Management.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

       //-----------This is to show product list------------//
       public async Task<IActionResult> Index(string SearchProduct,int? CategoryID,string sortingProduct,int page = 1)
        {
            int pageSize = 6;

            var cats = await _context.Categories.Where(c => c.Active).OrderBy(c => c.CategoryName).ToListAsync(); 
            ViewBag.Categories = new SelectList(cats, "CategoryID", "CategoryName", CategoryID);

            //......This fetch for products including categories------//
            var product = _context.Products.Include(p => p.Category).AsQueryable();
          
           
            //.......Check if the search term and filter include in request list.......//
            if (!String.IsNullOrWhiteSpace(SearchProduct))
            {
               var sProduct = product.Where(p => p.StockName.Contains(SearchProduct)).OrderBy(p=>p.CreatedDate);

                if (!await sProduct.AnyAsync())
                {
                    product = product.Where(p => p.Category.CategoryName.Contains(SearchProduct)).OrderBy(p => p.CreatedDate); //search the product with category name
                }
                else
                {
                    product = sProduct; //---search the product with product name
                }
            }


            //------This is the sorting process------//
            if (!String.IsNullOrWhiteSpace(sortingProduct))
            {
                switch (sortingProduct)
                {
                    case "nameAsc":
                        product = product.OrderBy(p => p.StockName); break;
                     
                    case "nameDesc":
                        product = product.OrderByDescending(p => p.StockName);break;

                    case "priceAsc":
                        product = product.OrderBy(p => p.Price);
                        break;

                    case "priceDesc":
                        product = product.OrderByDescending(p => p.Price);
                        break;

                    default:
                        product = product.OrderBy(p => p.StockName);break;

                }
            }

            //-------this filter by category-----//
            if (CategoryID.HasValue)
            {
                product = product.Where(p => p.CategoryID ==CategoryID.Value);
            }

            //....This return as page list------//
            var pageList = product.ToPagedList(page,pageSize);

            ViewBag.sortProduct = sortingProduct ;
            ViewBag.SearchProduct = SearchProduct;
            return View(pageList);
        }

        ///......This show for product add form..........//
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var cats = _context.Categories
                               .Where(c => c.Active)
                               .OrderBy(c => c.CategoryName)
                               .ToList();

            ViewBag.Categories = cats;

            //-------This code log to debug purpose.Currently, comment.............// 
            // System.Diagnostics.Debug.WriteLine(
            // JsonSerializer.Serialize(cats, new JsonSerializerOptions { WriteIndented = true })
            // );

            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("StockNo,StockName,CategoryID,ProductImgFile,Price,Active")]
                Product product)
        {
       
            if (product.ProductImgFile != null && product.ProductImgFile.Length > 0)
            {
           
                var ext = Path.GetExtension(product.ProductImgFile.FileName);
            
                var fileName = $"{Guid.NewGuid()}{ext}";
           
                var savePath = Path.Combine(_env.WebRootPath, "images", fileName);

          
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await product.ProductImgFile.CopyToAsync(stream);
                }

      
                product.ProductImage = Path.Combine("images", fileName)
                                           .Replace("\\", "/");
            }
            else
            {

                ModelState.AddModelError("ProductImgFile", "Please upload an image.");
                ViewBag.Categories = await _context.Categories
                                                  .Where(c => c.Active)
                                                  .OrderBy(c => c.CategoryName)
                                                  .ToListAsync();
                return View("Create", product);
            }


            product.CreatedDate = DateTime.UtcNow;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Product created successfully!";
            return RedirectToAction(nameof(Create));
        }




        //.......This is to show product detail for edit....//
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ///....This find the product by id....;//
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(); 
            }

            var categories = await _context.Categories
                                           .Where(c => c.Active)
                                           .OrderBy(c => c.CategoryName)
                                           .ToListAsync();

            ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName", product.CategoryID);

            return View(product);
        }


        //.........This is to edit product ..........//
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product model)
        {
            
            if(id != model.StockID)
            {
                return NotFound();
            }

            //----this fetch the database product data.../
            var product = await _context.Products.FindAsync(id);

            if(null == product)
            {
                return NotFound(); 
            }

            if(model.ProductImgFile !=null && model.ProductImgFile.Length > 0)
            {
                //----this delete the old img-------//
                if (!String.IsNullOrEmpty(product.ProductImage))
                {
                    var oldImgPath = Path.Combine(_env.WebRootPath, product.ProductImage);
                    
                    //-----this delete the old img if exist.....//
                    if (System.IO.File.Exists(oldImgPath))
                    {
                        System.IO.File.Delete(oldImgPath);
                    }
                }

                //--------This save the new image....//
                var ext = Path.GetExtension(model.ProductImgFile.FileName);

                var fileName = $"{Guid.NewGuid()}{ext}";

                var savePath = Path.Combine(_env.WebRootPath, "images", fileName);


                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await model.ProductImgFile.CopyToAsync(stream);
                }


                product.ProductImage = Path.Combine("images", fileName)
                                           .Replace("\\", "/");

            }

            ///---This update other field if update in input----//
            product.StockNo = model.StockNo;
            product.StockName = model.StockName;
            product.CategoryID = model.CategoryID;
            product.Price = model.Price;
            product.Active = model.Active;


            //---This update the product in database----//
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
                TempData["UpdateSuccess"] = "Product Update Successfully!";
            }catch(Exception ex)
            {
                ///log the error   
                TempData["UpdateError"] = "An error occurred while updating the product";
            }

            return RedirectToAction(nameof(Index));

        }


        //----This is delete process-----//
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            //----This find the product first------//

            var product = await _context.Products.FindAsync(id);


            //-----Delete the product image----//
            if(!String.IsNullOrEmpty(product.ProductImage))
            {
                var oldImgPath = Path.Combine(_env.WebRootPath, product.ProductImage);

                if (System.IO.File.Exists(oldImgPath))
                {
                    System.IO.File.Delete(oldImgPath);
                }
            }

            //----This delete the product from database-----//
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }


}
