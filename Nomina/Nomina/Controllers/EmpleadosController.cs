using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nomina.Data;
using Nomina.Entities;
using Nomina.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Nomina.Controllers
{
    [Authorize(Roles = "Recruiter")]
    public class EmpleadosController: Controller
    {
        private readonly PayrollSystemDbContext _payrollSystemDbContext;
        public EmpleadosController(PayrollSystemDbContext payrollSystemDbContext)
        {
            _payrollSystemDbContext = payrollSystemDbContext;
        }

        public IActionResult Index([Bind("SelectedFilter, TextFilter")] IndexEmpleadosModel indexEmpleadosModel)
        {
            var empleados = new List<Empleados>();

            if (!string.IsNullOrEmpty(indexEmpleadosModel.TextFilter))
            {
                switch (indexEmpleadosModel.SelectedFilter)
                {
                    case "name":
                        empleados = _payrollSystemDbContext.Empleados.Where(x => x.Nombre.Contains(indexEmpleadosModel.TextFilter))
                                                                     .Include(d => d.IdDepartamentoNavigation)
                                                                     .Include(p => p.IdPuestoNavigation)
                                                                     .ToList();
                        break;
                    case "identification":
                        empleados = _payrollSystemDbContext.Empleados.Where(x => x.Cedula.StartsWith(indexEmpleadosModel.TextFilter))
                                                                     .Include(d => d.IdDepartamentoNavigation)
                                                                     .Include(p => p.IdPuestoNavigation)
                                                                     .ToList();
                        break;
                    default:
                        empleados = _payrollSystemDbContext.Empleados.Include(d => d.IdDepartamentoNavigation)
                                                                     .Include(p => p.IdPuestoNavigation)
                                                                     .ToList();
                        break;
                } 
            }
            else
            {
                empleados = _payrollSystemDbContext.Empleados.Include(d => d.IdDepartamentoNavigation)
                                                                     .Include(p => p.IdPuestoNavigation)
                                                                     .ToList();
            }

            var indexEmpleados = new IndexEmpleadosModel
            {
                Empleados = empleados,
                Filter = new List<SelectListItem> 
                { 
                    new SelectListItem { Value = "name", Text = "Nombre" },
                    new SelectListItem { Value = "identification", Text = "Cedula" } 
                }
            };

            return View(indexEmpleados);
        }

        [HttpGet]
        public async Task<IActionResult> ImportExcel()
        {
            var data = await _payrollSystemDbContext.Empleados.Include(d => d.IdDepartamentoNavigation)
                                                              .Include(p => p.IdPuestoNavigation)
                                                              .ToListAsync();
            var fileBuilder = new StringBuilder();

            fileBuilder.AppendLine("CODIGO,CEDULA,NOMBRE,DEPARTAMENTO,PUESTO,SALARIO");

            foreach (var item in data)
            {
                fileBuilder.AppendLine($"{item.Id},\"{item.Cedula}\",{item.Nombre},{item.IdDepartamentoNavigation.Nombre},{item.IdPuestoNavigation.Nombre},\"{item.SalarioMensual}\"");
            }

            return File(Encoding.UTF8.GetBytes(fileBuilder.ToString()), "text/csv", "Empleados.csv");
        }

        public async Task<IActionResult> Create()
        {
            var createEmpleadoModel = new CreateEmpleadoModel
            {
                Departamentos = await _payrollSystemDbContext.Departamentos.ToListAsync(),
                Puestos = await _payrollSystemDbContext.Puestos.ToListAsync()
            };

            return View(createEmpleadoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Cedula, Nombre, IdDepartamento, IdPuesto, SalarioMensual")] CreateEmpleadoModel createEmpleadoModel)
        {
            var empleado = new Empleados
            {
                Cedula = createEmpleadoModel.Cedula,
                Nombre = createEmpleadoModel.Nombre,
                IdDepartamento = createEmpleadoModel.IdDepartamento,
                IdPuesto = createEmpleadoModel.IdPuesto,
                SalarioMensual = createEmpleadoModel.SalarioMensual
            };

            if (ModelState.IsValid)
            {
                _payrollSystemDbContext.Empleados.Add(empleado);
                await _payrollSystemDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            createEmpleadoModel.Departamentos = await _payrollSystemDbContext.Departamentos.ToListAsync();
            createEmpleadoModel.Puestos = await _payrollSystemDbContext.Puestos.ToListAsync();

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

            var editEmpleadoModel = new EditEmpleadoModel
            {
                Id = empleado.Id,
                Cedula = empleado.Cedula,
                Nombre = empleado.Nombre,
                IdDepartamento = empleado.IdDepartamento,
                IdPuesto = empleado.IdPuesto,
                SalarioMensual = empleado.SalarioMensual,
                Departamentos = _payrollSystemDbContext.Departamentos.ToList(),
                Puestos = _payrollSystemDbContext.Puestos.ToList()
            };

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
                    var empleado = new Empleados
                    {
                        Id = editEmpleadoModel.Id,
                        Cedula = editEmpleadoModel.Cedula,
                        Nombre = editEmpleadoModel.Nombre,
                        IdDepartamento = editEmpleadoModel.IdDepartamento,
                        IdPuesto = editEmpleadoModel.IdPuesto,
                        SalarioMensual = editEmpleadoModel.SalarioMensual
                    };


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
