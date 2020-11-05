using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nomina.Entities
{
    public partial class TiposDeducciones
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe asignar un nombre a este tipo de Ingreso")]
        public string Nombre { get; set; }
        [Display(Name = "Depende Salario")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string DependeSalario { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Estado { get; set; }
    }
}
