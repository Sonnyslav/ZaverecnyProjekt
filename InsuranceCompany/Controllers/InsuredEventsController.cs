using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InsuranceCompany.Data;
using InsuranceCompany.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Protocol.Core.Types;
using Humanizer;
using Microsoft.AspNetCore.Authorization;

namespace InsuranceCompany.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class InsuredEventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InsuredEventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Metoda Index. Pohled na výpis událostí. 
        /// </summary>
        /// <returns>Pohled s listem pojistných událostí</returns>
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.InsuredEvent
                .Include(i => i.Insurance)
                .Include(i => i.Client);
            return View(await applicationDbContext.ToListAsync());
        }
        /// <summary>
        /// Metoda Create GET. Zobrazuje formulář k založení události s předvyplněnými údaji o pojistce, ke které se váže.
        /// </summary>
        /// <param name="id">Číselný identifikátor události</param>
        /// <returns>Pohled na formulář k založení události bez modelu</returns>
        public async Task<IActionResult> Create(int? id)
        {
            var insurance = await _context.Insurance
                .FirstOrDefaultAsync(m => m.Id == id);
            var person = await _context.Client
                .Where(x => x.Id == insurance.InsuredPersonId)
                .FirstOrDefaultAsync();
            ViewBag.Insurance = insurance;
            ViewBag.Client = person;
            return View();
        }
        /// <summary>
        /// Metoda Create POST. Odeslání formuláře k založení pojistné události.
        /// </summary>
        /// <param name="insuredEvent">Model pojistné události</param>
        /// <returns>V případě úspěchu přesměrování na výpis událostí.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InsuranceId,ClientId,Description,State")] InsuredEvent insuredEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(insuredEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(insuredEvent);
        }
        /// <summary>
        /// Metoda Edit GET. Zobrazení formuláře k editaci pojistné události.
        /// </summary>
        /// <param name="id">Číselný identifikátor pojistné události</param>
        /// <returns>Pohled s modelem události</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var insuredEvent = await _context.InsuredEvent.FindAsync(id);
            if (insuredEvent == null)
            {
                return NotFound();
            }
            return View(insuredEvent);
        }
        /// <summary>
        /// Metoda Edit POST. Odeslání formuláře k editaci pojistné události
        /// </summary>
        /// <param name="id">Číselný identifikátor pojistné události</param>
        /// <param name="insuredEvent">Model pojistné události</param>
        /// <returns>V případě úspěchu přesměrování na výpis událost, v případě neúspěchu pohled s modelem upravované události</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InsuranceId,ClientId,Description,State")] InsuredEvent insuredEvent)
        {
            if (id != insuredEvent.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insuredEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuredEventExists(insuredEvent.Id))
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
            return View(insuredEvent);
        }
        /// <summary>
        /// Metoda Delete GET. Zobrazení detailu události před smazáním
        /// </summary>
        /// <param name="id">Číselný identifikátor události</param>
        /// <returns>Pohled s modelem události</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var insuredEvent = await _context.InsuredEvent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuredEvent == null)
            {
                return NotFound();
            }
            return View(insuredEvent);
        }
        /// <summary>
        /// Metoda DeleteConfirmed POST. Odeslání potvrzení vymazání události.
        /// </summary>
        /// <param name="id">Číselný identifikátor pojistné události.</param>
        /// <returns>V případě úspěchu přesměrování na seznam událostí.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insuredEvent = await _context.InsuredEvent.FindAsync(id);
            if (insuredEvent != null)
            {
                _context.InsuredEvent.Remove(insuredEvent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Kontrola existence konkrétní události.
        /// </summary>
        /// <param name="id">Číselný identifikátor události</param>
        /// <returns>True v případě existence události.</returns>
        private bool InsuredEventExists(int id)
        {
            return _context.InsuredEvent.Any(e => e.Id == id);
        }
    }
}
