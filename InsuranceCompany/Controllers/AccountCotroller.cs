using InsuranceCompany.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using InsuranceCompany.ViewModels;

namespace InsuranceCompany.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController
        (
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
        )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        /// <summary>
        /// Metoda RedirectToLocal. Navrácení na předchozí stránku po přihlášení.
        /// </summary>
        /// <param name="returnUrl">Návratová URL adresa</param>
        /// <returns>Konkrétní adresa k přesměrování, případně domácí adresa</returns>
        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        /// <summary>
        /// Metoda Login. Přechod na stránku s loginem.
        /// </summary>
        /// <param name="returnUrl">Návratová URL adresa</param>
        /// <returns>Prázdný ViewModel pro login</returns>
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        /// <summary>
        /// Metoda Login. Pokus o přihlášení po odeslání formuláře s vyplněnými údaji.
        /// </summary>
        /// <param name="model">ModelView s údaji k přihlášení</param>
        /// <param name="returnUrl">Návratová URL adresa</param>
        /// <returns>V případě úspěchu pomocí RedirectToLocal přesměrování na předchozí adresu, jinak vrací zpět ViewModel s loginem</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result =
                    await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                    return RedirectToLocal(returnUrl);
                ModelState.AddModelError("Login error", "Neplatné přihlašovací údaje.");
                return View(model);
            }
            return View(model);
        }
        /// <summary>
        /// Metoda Register. Přechod na stránku s registrací
        /// </summary>
        /// <param name="returnUrl">Návratová URL adresa</param>
        /// <returns>Prázdný ViewModel pro registraci</returns>
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        /// <summary>
        /// Metoda Register. Odeslání formuláře s registračními údaji. Defaultní přiřazení role user všem nově registrovaným.
        /// </summary>
        /// <param name="model">ModelView s údaji k registraci</param>
        /// <param name="returnUrl">Návratová URL adresa</param>
        /// <returns>V případě úspěchu pomocí RedirectToLocal přesměrování na předchozí adresu, jinak vrací registrační ViewModel</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            model.RepairBirthNumber();
            if (!model.CheckBirthNumber())
            {
                ViewBag.Error = "Neplatné rodné číslo!";
                return View(model);
            }
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser { UserName = model.Email, Email = model.Email, Id = model.BirthNumber };
                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    await userManager.AddToRoleAsync(user, UserRoles.User);
                    return RedirectToLocal(returnUrl);
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            return View(model);
        }
        /// <summary>
        /// Metoda Logout. Odhlášení současně přihlášeného uživatele.
        /// </summary>
        /// <returns>Pomocí RedirectToLocal přesměrování na domovskou stránku</returns>
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToLocal(null);
        }
    }
}