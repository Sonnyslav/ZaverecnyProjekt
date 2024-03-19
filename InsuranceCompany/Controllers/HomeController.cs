using InsuranceCompany.Data;
using InsuranceCompany.Models;
using InsuranceCompany.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace InsuranceCompany.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController
        (
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager
        )
        {
            _context = context;
            _userManager = userManager;
        }
        /// <summary>
        /// Metoda Index. Úvodní "prázdný" pohled.
        /// </summary>
        /// <returns>View bez pøedaného modelu.</returns>
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Metoda MyInsurance. Naplnìní ViewModelu daty o klientovi a jeho pojistkách.
        /// </summary>
        /// <returns>Pohled s ViewModelem naplnìným daty o klientovi v pøípadì, že byl nalezen.</returns>
        public async Task<IActionResult> MyInsurance()
        {
            var personView = new PersonDetailsViewModel();
            try
            {
                personView.Client = await _context.Client
                    .SingleOrDefaultAsync(m => m.BirthNumber == _userManager.GetUserId(User));
                personView.InsuranceContracts = await _context.Insurance
                    .Include(i => i.InsuredPerson)
                    .Where(i => i.InsuredPersonId == personView.Client.Id)
                    .ToListAsync();
                personView.InsurerContracts = await _context.Insurance
                    .Include(i => i.Insurer)
                    .Where(i => i.InsurerId == personView.Client.Id)
                    .ToListAsync();
                personView.InsuredEvents = await _context.InsuredEvent
                    .Include(i => i.Client)
                    .Where(i => i.ClientId == personView.Client.Id)
                    .ToListAsync();
                personView.InstalmentsSum = personView.InsurerContracts
                    .Sum(x => x.Instalment);
                return View(personView);
            }
            catch
            {
                ViewBag.NotAUser = "Zatím nejste naším klientem. Pokud si to pøejete zmìnit, kontaktuje našeho zástupce.";
                return View(personView);
            }
        }
        /// <summary>
        /// Metoda AboutUs. Pohled na stránku "O Nás" bez modelu.
        /// </summary>
        /// <returns>Prázdný pohled bez pøedaného modelu.</returns>
        public IActionResult AboutUs()
        {
            return View();
        }
        /// <summary>
        /// Metoda Contacts. Pohled na stránku "Kontakty".
        /// </summary>
        /// <returns>Prázdný pohled bez pøedaného modelu.</returns>
        public IActionResult Contacts()
        {
            return View();
        }
        /// <summary>
        /// Metoda AccessDenied. Pohled na stránku "Pøístup odepøen".
        /// </summary>
        /// <returns>Prázdný pohled bez pøedaného modelu.</returns>
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
