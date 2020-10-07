using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nomina.Entities
{
    public partial class Puestos
    {
        public Puestos()
        {
            Empleados = new HashSet<Empleados>();
        }

        public int Id { get; set; }
        [Display(Name = "Puesto")]
        public string Nombre { get; set; }
        public string NivelRiesgo { get; set; }
        public decimal? NivelMinimoSalario { get; set; }
        public decimal? NivelMaximoSalario { get; set; }

        public virtual ICollection<Empleados> Empleados { get; set; }
    }
}
