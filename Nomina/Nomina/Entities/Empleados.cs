using System;
using System.Collections.Generic;

namespace Nomina.Entities
{
    public partial class Empleados
    {
        public int Id { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public int IdDepartamento { get; set; }
        public int IdPuesto { get; set; }
        public decimal? SalarioMensual { get; set; }

        public virtual Departamentos IdDepartamentoNavigation { get; set; }
        public virtual Puestos IdPuestoNavigation { get; set; }
    }
}
