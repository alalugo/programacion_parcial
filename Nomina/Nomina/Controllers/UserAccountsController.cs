using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Nomina.Models;

namespace Nomina.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserAccountsController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        
        public UserAccountsController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var roles = _roleManager.Roles.ToList().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Name,
                Selected = _userManager.IsInRoleAsync(user, x.Name).Result
            }).ToList();

            var identity = new EditIdentityModel
            {
                User = user,
                Roles = roles
            };

            return View(identity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("User, Roles")] EditIdentityModel identityModel)
        {
            if (id != identityModel.User.Id)
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound("User not found!");

            if (ModelState.IsValid)
            {
                var identityUpdateResult = new List<IdentityResult>();

                user.UserName = identityModel.User.UserName;

                // Update the user
                var updateUserResult = await _userManager.UpdateAsync(user);
                identityUpdateResult.Add(updateUserResult);

                // Add users to selected roles
                foreach (var item in identityModel.Roles.Where(x => x.Selected))
                {
                    if (await _userManager.IsInRoleAsync(user, item.Value))
                        continue;

                    var result = await _userManager.AddToRoleAsync(user, item.Value);
                    identityUpdateResult.Add(result);
                }

                // Remove user from roles
                foreach (var item in identityModel.Roles.Where(x => !x.Selected))
                {
                    if (!await _userManager.IsInRoleAsync(user, item.Value))
                        continue;

                    var result = await _userManager.RemoveFromRoleAsync(user, item.Value);
                    identityUpdateResult.Add(result);
                }

                if (identityUpdateResult.All(r => r.Succeeded))
                    return RedirectToAction(nameof(Index));
            }

            return View(identityModel);
        }
    }
}
