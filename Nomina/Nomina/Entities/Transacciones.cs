using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nomina.Entities
{
    public partial class Transacciones
    {
        public int Id { get; set; }
        public int IdCustomer { get; set; }
        public string Descripcion { get; set; }
        [Display(Name = "Salario Neto")]
        public decimal Monto { get; set; }
        public DateTime FechaTransaccion { get; set; }

        public virtual Empleados IdCustomerNavigation { get; set; }
    }
}
