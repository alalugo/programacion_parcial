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
        [Required(ErrorMessage = "El nombre del Departamento es obligatorio")]
        public string Nombre { get; set; }
        [Display(Name = "Direccion")]
        [Required(ErrorMessage = "Debe asignar una direccion a este Departamento")]
        public string UbicacionFisica { get; set; }

        public virtual ICollection<Empleados> Empleados { get; set; }
    }
}
