using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InsuranceCompany.Data;
using InsuranceCompany.Models;
using Microsoft.AspNetCore.Authorization;

namespace InsuranceCompany.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class InsurancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InsurancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Metoda Index. Základní pohled na přehled pojištění.
        /// </summary>
        /// <returns>Pohled se seznamem pojištění.</returns>
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Insurance.Include(i => i.InsuredPerson).Include(i => i.Insurer);
            return View(await applicationDbContext.ToListAsync());
        }
        /// <summary>
        /// Metoda Details. Zobrazení detailů pojištění na základě předaného ID.
        /// </summary>
        /// <param name="id">Číselný identifikátor pojistky</param>
        /// <returns>Pohled s detaily pojistky v případě, že je nalezena.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var insurance = await _context.Insurance
                .Include(i => i.InsuredPerson)
                .Include(i => i.Insurer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insurance == null)
            {
                return NotFound();
            }
            return View(insurance);
        }
        /// <summary>
        /// Metoda Create GET. Zobrazení prázdného formuláře k založení nové pojistky a dohledání možných klientů do výběrových seznamů ve formuláři.
        /// </summary>
        /// <returns>Základní pohled na formulář k vytvoření pojistky bez modelu</returns>
        public IActionResult Create()
        {
            ViewData["InsuredPersonId"] = new SelectList(from person in _context.Client select new { person.Id, person.BirthNumber }, "Id", "BirthNumber");
            ViewData["InsurerId"] = new SelectList(from person in _context.Client select new { person.Id, person.BirthNumber }, "Id", "BirthNumber");
            return View();
        }
        /// <summary>
        /// Metoda Create POST. Odeslání formuláře s vyplněnými údaji o pojistce.
        /// </summary>
        /// <param name="insurance">Pojistka přijatá z formuláře</param>
        /// <returns>V případě úspěchu návrat na seznam pojistek, v případě neúspěchu pohled s modelem pojistky.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,InsuredPersonId,InsurerId,InsuredAmount,Instalment,InsuranceStartDate,InsuranceEndDate")] Insurance insurance)
        {
            if (ModelState.IsValid && insurance.IsInstalmentSmaller())
            {
                _context.Add(insurance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Error = "Splátka nesmí být větší než celková suma.";
            ViewData["InsuredPersonId"] = new SelectList(from person in _context.Client select new { person.Id, person.BirthNumber }, "Id", "BirthNumber", insurance.InsuredPersonId);
            ViewData["InsurerId"] = new SelectList(from person in _context.Client select new { person.Id, person.BirthNumber }, "Id", "BirthNumber");
            return View(insurance);
        }
        /// <summary>
        /// Metoda Edit GET. Zobrazení detailů pojistky na základě předaného ID. 
        /// </summary>
        /// <param name="id">Číselný identifikátor pojistky</param>
        /// <returns>V případě úspěchu pohled s formulářem naplněným údaji o pojistce</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var insurance = await _context.Insurance.FindAsync(id);
            if (insurance == null)
            {
                return NotFound();
            }
            ViewData["InsuredPersonId"] = new SelectList(from person in _context.Client select new { person.Id, person.BirthNumber }, "Id", "BirthNumber", insurance.InsuredPersonId);
            ViewData["InsurerId"] = new SelectList(from person in _context.Client select new { person.Id, person.BirthNumber }, "Id", "BirthNumber");
            return View(insurance);
        }
        /// <summary>
        /// Metoda Edit POST. Odeslání formuláře s úpravami pojistky.
        /// </summary>
        /// <param name="id">Číselný identifikátor pojistky</param>
        /// <param name="insurance">Upravovaná pojistka</param>
        /// <returns>V případě úspěchu návrat na seznam pojistek, v případě neúspěchu pohled s vyplněným formulářem</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,InsuredPersonId,InsurerId,InsuredAmount,Instalment,InsuranceStartDate,InsuranceEndDate")] Insurance insurance)
        {
            if (id != insurance.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid && insurance.IsInstalmentSmaller())
            {
                try
                {
                    _context.Update(insurance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceExists(insurance.Id))
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
            else ViewBag.Error = "Splátka nesmí být větší než celková suma.";
            ViewData["InsuredPersonId"] = new SelectList(from person in _context.Client select new { person.Id, person.BirthNumber }, "Id", "BirthNumber", insurance.InsuredPersonId);
            ViewData["InsurerId"] = new SelectList(from person in _context.Client select new { person.Id, person.BirthNumber }, "Id", "BirthNumber");
            return View(insurance);
        }
        /// <summary>
        /// Metoda Delete GET. Zobrazení formuláře s údaji o pojistce k vymazání.
        /// </summary>
        /// <param name="id">Číselný identifikátor pojistky</param>
        /// <returns>V případě úspěchu pohled s modelem mazané pojistky.</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var insurance = await _context.Insurance
                .Include(i => i.InsuredPerson)
                .Include(i => i.Insurer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insurance == null)
            {
                return NotFound();
            }
            return View(insurance);
        }
        /// <summary>
        /// Metoda Delete POST. Odeslání formuláře s potvrzením vymazání pojistky.
        /// </summary>
        /// <param name="id">Číselný identifikátor pojistky</param>
        /// <returns>V případě úspěchu a vymazání pojistky návrat na přehled pojistek. V případě neúspěchu pohled s modelem pojistky.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insurance = await _context.Insurance.FindAsync(id);
            var events = await _context.InsuredEvent
                .FirstAsync(m => m.InsuranceId == id);
            if (insurance != null && events == null)
            {
                _context.Insurance.Remove(insurance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Error = "K této pojistce se váže pojistná událost, nejdříve je nutné ji odstranit.";
            return View(insurance);
        }
        /// <summary>
        /// Kontrola existence pojistky.
        /// </summary>
        /// <param name="id">Číselný identifikátor pojistky</param>
        /// <returns>True pokud je pojistka nalezena</returns>
        private bool InsuranceExists(int id)
        {
            return _context.Insurance.Any(e => e.Id == id);
        }
    }
}
