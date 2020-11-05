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
        [Display(Name = "Nivel de Riesgo")]
        [Required(ErrorMessage = "Debe asignar un nivel de riesgo a este puesto")]
        public string NivelRiesgo { get; set; }
        [Display(Name = "Nivel de Salario Minimo")]
        [Required(ErrorMessage = "Debe establecer un salario minimo para este puesto")]
        public decimal? NivelMinimoSalario { get; set; }
        [Display(Name = "Nivel de Salario Maximo")]
        [Required(ErrorMessage = "Debe establecer un salario maximo para este puesto")]
        public decimal? NivelMaximoSalario { get; set; }

        public virtual ICollection<Empleados> Empleados { get; set; }
    }
}
