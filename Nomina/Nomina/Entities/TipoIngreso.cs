using System;
using System.Collections.Generic;

namespace Nomina.Entities
{
    public partial class TipoIngreso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string DependeSalario { get; set; }
        public string Estado { get; set; }
    }
}
