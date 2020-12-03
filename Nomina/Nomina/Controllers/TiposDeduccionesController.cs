using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nomina.Data;
using Nomina.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Nomina.Controllers
{
    [Authorize(Roles = "Recruiter")]
    public class TiposDeduccionesController: Controller
    {
        private readonly PayrollSystemDbContext _payrollSystemDbContext;

        public TiposDeduccionesController(PayrollSystemDbContext payrollSystemDbContext)
        {
            _payrollSystemDbContext = payrollSystemDbContext;
        }

        public IActionResult Index()
        {
            var tiposDeducciones = _payrollSystemDbContext.TiposDeducciones.ToList();

            return View(tiposDeducciones);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Nombre, DependeSalario, Porcentaje, Estado")] TiposDeducciones tipoDeduccion)
        {
            if (ModelState.IsValid)
            {
                _payrollSystemDbContext.TiposDeducciones.Add(tipoDeduccion);
                await _payrollSystemDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(tipoDeduccion);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var tipoDeduccion = await _payrollSystemDbContext.TiposDeducciones
                                                            .FirstOrDefaultAsync(m => m.Id == id);

            if (tipoDeduccion == null)
                return NotFound();

            return View(tipoDeduccion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoDeduccion = await _payrollSystemDbContext.TiposDeducciones.FindAsync(id);

            _payrollSystemDbContext.TiposDeducciones.Remove(tipoDeduccion);
            await _payrollSystemDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var tipoDeduccion = await _payrollSystemDbContext.TiposDeducciones
                                                            .Where(d => d.Id == id)
                                                            .FirstOrDefaultAsync();

            if (tipoDeduccion == null)
                return NotFound();

            return View(tipoDeduccion);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var tipoDeduccion = await _payrollSystemDbContext.TiposDeducciones.FindAsync(id);

            if (tipoDeduccion == null)
                return NotFound();

            return View(tipoDeduccion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id, Nombre, DependeSalario, Porcentaje, Estado")] TiposDeducciones tipoDeduccion)
        {
            if (id != tipoDeduccion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _payrollSystemDbContext.Update(tipoDeduccion);
                    await _payrollSystemDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TiposDeduccionesExists(tipoDeduccion.Id))
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
            return View(tipoDeduccion);
        }
        private bool TiposDeduccionesExists(int id)
        {
            return _payrollSystemDbContext.TiposDeducciones.Any(e => e.Id == id);
        }
    }
}
