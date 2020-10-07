using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nomina.Data;
using Nomina.Entities;
using Nomina.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Nomina.Controllers
{
    public class EmpleadosController: Controller
    {
        private readonly PayrollSystemDbContext _payrollSystemDbContext;
        public EmpleadosController(PayrollSystemDbContext payrollSystemDbContext)
        {
            _payrollSystemDbContext = payrollSystemDbContext;
        }

        public IActionResult Index()
        {
            var empleados = _payrollSystemDbContext.Empleados.Include(d => d.IdDepartamentoNavigation)
                                                             .Include(p => p.IdPuestoNavigation)
                                                             .ToList();

            return View(empleados);
        }

        public async Task<IActionResult> Create()
        {
            var createEmpleadoModel = new CreateEmpleadoModel();

            createEmpleadoModel.Departamentos = await _payrollSystemDbContext.Departamentos.ToListAsync();
            createEmpleadoModel.Puestos = await _payrollSystemDbContext.Puestos.ToListAsync();

            return View(createEmpleadoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Cedula, Nombre, IdDepartamento, IdPuesto, SalarioMensual")] CreateEmpleadoModel createEmpleadoModel)
        {
            var empleado = new Empleados();

            empleado.Cedula = createEmpleadoModel.Cedula;
            empleado.Nombre = createEmpleadoModel.Nombre;
            empleado.IdDepartamento = createEmpleadoModel.IdDepartamento;
            empleado.IdPuesto = createEmpleadoModel.IdPuesto;
            empleado.SalarioMensual = createEmpleadoModel.SalarioMensual;

            if (ModelState.IsValid)
            {
                _payrollSystemDbContext.Empleados.Add(empleado);
                await _payrollSystemDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(createEmpleadoModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var empleado = await _payrollSystemDbContext.Empleados
                                                        .Include(d => d.IdDepartamentoNavigation)
                                                        .Include(d => d.IdPuestoNavigation)
                                                        .Where(d => d.Id == id)
                                                        .FirstOrDefaultAsync();

            if (empleado == null)
                return NotFound();

            return View(empleado);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleado = await _payrollSystemDbContext.Empleados.FindAsync(id);

            _payrollSystemDbContext.Empleados.Remove(empleado);
            await _payrollSystemDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var empleado = await _payrollSystemDbContext.Empleados
                                                        .Include(d => d.IdDepartamentoNavigation)
                                                        .Include(d => d.IdPuestoNavigation)
                                                        .Where(d => d.Id == id)
                                                        .FirstOrDefaultAsync();

            if (empleado == null)
                return NotFound();

            return View(empleado);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var empleado = await _payrollSystemDbContext.Empleados.FindAsync(id);

            if (empleado == null)
                return NotFound();

            var editEmpleadoModel = new EditEmpleadoModel();

            editEmpleadoModel.Id = empleado.Id;
            editEmpleadoModel.Cedula = empleado.Cedula;
            editEmpleadoModel.Nombre = empleado.Nombre;
            editEmpleadoModel.IdDepartamento = empleado.IdDepartamento;
            editEmpleadoModel.IdPuesto = empleado.IdPuesto;
            editEmpleadoModel.SalarioMensual = empleado.SalarioMensual;
            editEmpleadoModel.Departamentos = _payrollSystemDbContext.Departamentos.ToList();
            editEmpleadoModel.Puestos = _payrollSystemDbContext.Puestos.ToList();

            return View(editEmpleadoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id, Cedula, Nombre, IdDepartamento, IdPuesto, SalarioMensual")] EditEmpleadoModel editEmpleadoModel )
        {
            if (id != editEmpleadoModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var empleado = new Empleados();

                    empleado.Id = editEmpleadoModel.Id;
                    empleado.Cedula = editEmpleadoModel.Cedula;
                    empleado.Nombre = editEmpleadoModel.Nombre;
                    empleado.IdDepartamento = editEmpleadoModel.IdDepartamento;
                    empleado.IdPuesto = editEmpleadoModel.IdPuesto;
                    empleado.SalarioMensual = editEmpleadoModel.SalarioMensual;


                    _payrollSystemDbContext.Update(empleado);
                    await _payrollSystemDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(editEmpleadoModel.Id))
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
            return View(editEmpleadoModel);
        }
        private bool EmpleadoExists(int id)
        {
            return _payrollSystemDbContext.Empleados.Any(e => e.Id == id);
        }
    }
}
