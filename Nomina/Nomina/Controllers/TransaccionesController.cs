using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nomina.Data;
using Nomina.Entities;

namespace Nomina.Controllers
{
    public class TransaccionesController : Controller
    {
        PayrollSystemDbContext _payrollSystemDbContext;
        public TransaccionesController(PayrollSystemDbContext payrollSystemDbContext)
        {
            _payrollSystemDbContext = payrollSystemDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var transactions = await _payrollSystemDbContext.Transacciones.Include(t => t.IdCustomerNavigation)
                                                                    .OrderByDescending(t => t.FechaTransaccion)
                                                                    .ToListAsync();

            return View(transactions);
        }

        public async Task<IActionResult> PagarNomina()
        {
            var employes = await _payrollSystemDbContext.Empleados.ToListAsync();
            decimal? deduction = 0;

            foreach (var item in employes)
            {
                var transaction = new Transacciones();
                var MonthlySalary = item.SalarioMensual;

                transaction.IdCustomer = item.Id;
                transaction.Descripcion = "PAGO NOMINA";

                foreach (var deductions in _payrollSystemDbContext.TiposDeducciones.ToList())
                {
                    deduction = MonthlySalary - (MonthlySalary * (deductions.Porcentaje / 100));
                }

                transaction.Monto =  Convert.ToDecimal(deduction);
                transaction.FechaTransaccion = DateTime.Now;

                _payrollSystemDbContext.Transacciones.Add(transaction);
                _payrollSystemDbContext.SaveChanges();

            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ImportExcel()
        {
            var transactions = await _payrollSystemDbContext.Transacciones.Include(t => t.IdCustomerNavigation)
                                                                    .OrderByDescending(t => t.FechaTransaccion)
                                                                    .ToListAsync();

            var fileBuilder = new StringBuilder();

            fileBuilder.AppendLine("CODIGO,EMPLEADO,DESCRIPCION,SALARIO MENSUAL,SALARIO NETO,FECHA");

            foreach (var item in transactions)
            {
                fileBuilder.AppendLine($"{item.Id},\"{item.IdCustomerNavigation.Nombre}\",{item.Descripcion},\"{item.IdCustomerNavigation.SalarioMensual}\",\"{item.Monto}\",\"{item.FechaTransaccion}\"");
            }

            return File(Encoding.UTF8.GetBytes(fileBuilder.ToString()), "text/csv", "Transacciones.csv");
        }
    }
}
