using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nomina.Entities
{
    public partial class Departamentos
    {
        public Departamentos()
        {
            Empleados = new HashSet<Empleados>();
        }

        public int Id { get; set; }
        [Display(Name = "Departamento")]
        public string Nombre { get; set; }
        public string UbicacionFisica { get; set; }

        public virtual ICollection<Empleados> Empleados { get; set; }
    }
}
