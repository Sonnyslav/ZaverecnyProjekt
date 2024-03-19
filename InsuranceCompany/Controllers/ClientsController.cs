using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InsuranceCompany.Data;
using InsuranceCompany.Models;
using InsuranceCompany.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace InsuranceCompany.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Metoda Index. Základní pohled na přehled klientů.
        /// </summary>
        /// <returns>Pohled se seznamem klientů</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Client.ToListAsync());
        }
        /// <summary>
        /// Metoda Details. Zobrazení detailů klienta na základě předaného ID.
        /// </summary>
        /// <param name="id">Číselný identifikátor klienta</param>
        /// <returns>Pohled s detaily klienta v případě, že je nalezen</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var personView = new PersonDetailsViewModel();
            personView.Client = await _context.Client
                .SingleOrDefaultAsync(m => m.Id == id);
            personView.InsuranceContracts = await _context.Insurance
                .Include(i => i.InsuredPerson)
                .ToListAsync();
            personView.InsurerContracts = await _context.Insurance
                .Include(i => i.Insurer)
                .ToListAsync();
            personView.InstalmentsSum = personView.InsurerContracts
                .Sum(x => x.Instalment);
            return View(personView);
        }
        /// <summary>
        /// Metoda Create GET. Zobrazení prázdného formuláře k založení nového klienta.
        /// </summary>
        /// <returns>Základní pohled na formulář k vytvoření klienta bez modelu</returns>
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Metoda Create POST. Odeslání formuláře s vyplněnými údaji o osobě. Provedení kontrol vstupů.
        /// </summary>
        /// <param name="person"> Osoba přijatá z formuláře</param>
        /// <returns>V případě úspěchu návrat na seznam klientů, v případě neúspěchu pohled s modelem klienta.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,BirthNumber,Age,Street,HouseNumber,City,PostCode,Country,Phone")] Client person)
        {
            person.RepairBirthNumber();
            if (!person.PhoneNumberCheckAndFormat())
            {
                ViewBag.PhoneNumberError = "Zadejte platné telefonní číslo ve formátu 123 456 789 nebo 123456789";
                return View(person);
            }
            if (!person.PostCodeCheckAndFormat())
            {
                ViewBag.PostCodeError = "Zadejte platné PSČ ve formátu 123 56 nebo 12345!";
                return View(person);
            }
            if (ModelState.IsValid && person.CheckBirthNumber())
            {
                person.TrimTextInputs();
                person.GetGender();
                person.GetAge();
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else ViewBag.BirthNumberError = "Neplatné rodné číslo!";
            return View(person);
        }
        /// <summary>
        /// Metoda Edit GET. Zobrazení detailů klienta na základě předaného ID.
        /// </summary>
        /// <param name="id">Číselný identifikátor klienta</param>
        /// <param name="person">V případě úspěchu pohled s formulářem naplněným údaji o klientovi</param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Client.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }
        /// <summary>
        /// Metoda Edit POST. Odeslání formuláře s úpravami klienta.
        /// </summary>
        /// <param name="id">Číselný identifikátor klienta</param>
        /// <param name="person">Upravovaný klient</param>
        /// <returns>V případě úspěchu návrat na seznam klienta, v případě neúspěchu pohled s vyplněným formulářem</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,BirthNumber,Sex,Age,Street,HouseNumber,City,PostCode,Country,Phone")] Client person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }
            person.RepairBirthNumber();
            if (!person.PhoneNumberCheckAndFormat())
            {
                ViewBag.PhoneNumberError = "Zadejte platné telefonní číslo ve formátu 123 456 789 nebo 123456789";
                return View(person);
            }
            if (!person.PostCodeCheckAndFormat())
            {
                ViewBag.PostCodeError = "Zadejte platné PSČ ve formátu 123 56 nebo 123456!";
                return View(person);
            }
            if (ModelState.IsValid && person.CheckBirthNumber())
            {
                try
                {
                    person.TrimTextInputs();
                    person.GetGender();
                    person.GetAge();
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
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
            else ViewBag.BirthNumberError = "Neplatné rodné číslo!";
            return View(person);
        }
        /// <summary>
        /// Metoda Delete GET. Zobrazení formuláře s údaji o klientovi k vymazání.
        /// </summary>
        /// <param name="id">Číselný identifikátor klienta</param>
        /// <returns>V případě úspěchu pohled s modelem mazaného klienta.</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Client
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }
        /// <summary>
        /// Metoda Delete POST. Odeslání formuláře s potvrzením vymazání klienta.
        /// </summary>
        /// <param name="id">Číselný identifikátor klienta</param>
        /// <returns>V případě úspěchu a vymazání klienta návrat na přehled klienta. V případě neúspěchu pohled s modelem klienta.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Client
                .FindAsync(id);
            var contracts = await _context.Insurance
                .FirstAsync(m => m.InsuredPersonId == id);
            if (person != null && contracts == null)
            {
                _context.Client.Remove(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Error = "Uživatel má stále aktivní pojistku! Před vymazáním nutné odstranit.";
            return View(person);
        }
        /// <summary>
        /// Kontrola existence klienta.
        /// </summary>
        /// <param name="id">Číselný identifikátor klienta</param>
        /// <returns>True pokud je klient nalezen</returns>
        private bool PersonExists(int id)
        {
            return _context.Client.Any(e => e.Id == id);
        }
    }
}
