using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nomina.Data;
using Nomina.Entities;

namespace Nomina.Controllers
{
    public class DepartamentosController : Controller
    {
        private readonly PayrollSystemDbContext _payrollSystemDbContext;
        public DepartamentosController(PayrollSystemDbContext payrollSystemDbContext)
        {
            _payrollSystemDbContext = payrollSystemDbContext;
        }

        public IActionResult Index()
        {
            var departamentosList = _payrollSystemDbContext.Departamentos.ToList();

            return View(departamentosList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Nombre, UbicacionFisica")] Departamentos departamento)
        {
            if (ModelState.IsValid)
            {
                _payrollSystemDbContext.Departamentos.Add(departamento);
                await _payrollSystemDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(departamento);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var departamento = await _payrollSystemDbContext.Departamentos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (departamento == null)
                return NotFound();

            return View(departamento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departamento = await _payrollSystemDbContext.Departamentos.FindAsync(id);

            _payrollSystemDbContext.Departamentos.Remove(departamento);
            await _payrollSystemDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var departamento = await _payrollSystemDbContext.Departamentos
                                                            .Where(d => d.Id == id)
                                                            .FirstOrDefaultAsync();

            if (departamento == null)
                return NotFound();

            return View(departamento);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var departamento = await _payrollSystemDbContext.Departamentos.FindAsync(id);

            if (departamento == null)
                return NotFound();

            return View(departamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id, Nombre, UbicacionFisica")] Departamentos departamento)
        {
            if (id != departamento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _payrollSystemDbContext.Update(departamento);
                    await _payrollSystemDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartamentoExists(departamento.Id))
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
            return View(departamento);
        }
        private bool DepartamentoExists(int id)
        {
            return _payrollSystemDbContext.Departamentos.Any(e => e.Id == id);
        }
    }
}
