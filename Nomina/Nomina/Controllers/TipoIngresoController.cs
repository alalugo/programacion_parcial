using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nomina.Data;
using Nomina.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nomina.Controllers
{
    [Authorize(Roles = "Recruiter")]
    public class TipoIngresoController: Controller
    {
        private readonly PayrollSystemDbContext _payrollSystemDbContext;

        public TipoIngresoController(PayrollSystemDbContext payrollSystemDbContext)
        {
            _payrollSystemDbContext = payrollSystemDbContext;
        }

        public IActionResult Index()
        {
            var tiposIngresos = _payrollSystemDbContext.TipoIngreso.ToList();

            return View(tiposIngresos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Nombre, DependeSalario, Estado")] TipoIngreso tipoIngreso)
        {
            if (ModelState.IsValid)
            {
                _payrollSystemDbContext.TipoIngreso.Add(tipoIngreso);
                await _payrollSystemDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(tipoIngreso);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var tipoIngreso = await _payrollSystemDbContext.TipoIngreso
                                                            .FirstOrDefaultAsync(m => m.Id == id);

            if (tipoIngreso == null)
                return NotFound();

            return View(tipoIngreso);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoIngreso = await _payrollSystemDbContext.TipoIngreso.FindAsync(id);

            _payrollSystemDbContext.TipoIngreso.Remove(tipoIngreso);
            await _payrollSystemDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var tipoIngreso = await _payrollSystemDbContext.TipoIngreso
                                                            .Where(d => d.Id == id)
                                                            .FirstOrDefaultAsync();

            if (tipoIngreso == null)
                return NotFound();

            return View(tipoIngreso);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var tipoIngreso = await _payrollSystemDbContext.TipoIngreso.FindAsync(id);

            if (tipoIngreso == null)
                return NotFound();

            return View(tipoIngreso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id, Nombre, DependeSalario, Estado")] TipoIngreso tipoIngreso)
        {
            if (id != tipoIngreso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _payrollSystemDbContext.Update(tipoIngreso);
                    await _payrollSystemDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoIngresoExists(tipoIngreso.Id))
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
            return View(tipoIngreso);
        }
        private bool TipoIngresoExists(int id)
        {
            return _payrollSystemDbContext.TipoIngreso.Any(e => e.Id == id);
        }
    }
}
