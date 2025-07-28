using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Product_Management.Models;
using Product_Management.Models.ViewModel;

namespace Product_Management.Controllers
{
    public class AccountController : Controller
    {


        private readonly UserManager<BaseUserClass> _userManager;
        private readonly SignInManager<BaseUserClass> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<BaseUserClass> userManager,SignInManager<BaseUserClass> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager= userManager;
            _signInManager=signInManager;
            _roleManager= roleManager;
        }


        
        public IActionResult UserRegister()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserRegister(CustomerViewModel customer)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var cus = new Customer
            {
                UserName = customer.FirstName + ""+ customer.LastName,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Address = customer.Address,
                City = customer.City,
            };

            var result = await _userManager.CreateAsync(cus, customer.Password);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError("", err.Description);
                return View(customer);
            }


            if (!await _roleManager.RoleExistsAsync("Customer"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Customer"));

                
            }

            await _userManager.AddToRoleAsync(cus, "Customer");
            await _signInManager.SignInAsync(cus,isPersistent: false);

            ModelState.Clear();

            return RedirectToAction("Index","Product");


        }


        public IActionResult AdminRegister() => View();


        [HttpPost]
        public async Task<IActionResult> AdminRegister(AdminViewModel admin)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            var adm = new Admin
            {
                UserName = admin.FirstName + "" + admin.LastName,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                Role = admin.Role,
                Department = admin.Department,
            };

            var result = await _userManager.CreateAsync(adm, admin.Password);

            if (!result.Succeeded)
            {
                foreach(var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View(admin);
            }

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            await _userManager.AddToRoleAsync(adm, "Admin");
            await _signInManager.SignInAsync(adm, isPersistent: false);

            ModelState.Clear();
            return RedirectToAction("Index","Product");
        }
    }
}
