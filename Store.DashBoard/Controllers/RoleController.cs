

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.DashBoard.Models;

namespace Store.DashBoard.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var Roles = await _roleManager.Roles.ToListAsync();

            return View(Roles);
        }


        [HttpPost]
        public async Task<IActionResult> Create(RoleFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var RoleExist = await _roleManager.RoleExistsAsync(model.Name);

                if (!RoleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));

                    return RedirectToAction("Index");
                }

                else
                {
                    var Roles = await _roleManager.Roles.ToListAsync();

                    ModelState.AddModelError("Name", "Role Is Already Exist");
                    return View("Index", Roles);
                }

            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(string id)
        {

            var Role = await _roleManager.FindByIdAsync(id);

            await _roleManager.DeleteAsync(Role);

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            var mappedRole = new RoleViewModel()
            {
                Name = role.Name
            };
            return View(mappedRole);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleExists = await _roleManager.RoleExistsAsync(model.Name);
                if (!roleExists)
                {

                    var role =await _roleManager.FindByIdAsync(model.Id);
                    role.Name=model.Name;

                   await _roleManager.UpdateAsync(role);

                    return RedirectToAction("Index");
                }
                else
                {
                    var Roles = await _roleManager.Roles.ToListAsync();

                    ModelState.AddModelError("Name", "Role Is Already Exist");
                    return View("Index", Roles);
                }

            }
            return RedirectToAction("Index");



        }
    }
}
