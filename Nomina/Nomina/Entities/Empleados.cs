using Nomina.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nomina.Entities
{
    public partial class Empleados
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La cedula es obligatoria")]
        [ValidateIdentification]
        public string Cedula { get; set; }
        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "El nombre del empleado es obligatorio")]
        public string Nombre { get; set; }
        [Display(Name = "Departamento")]
        [Required(ErrorMessage = "Debe seleccionar un departamento para el empleado")]
        public int IdDepartamento { get; set; }
        [Display(Name = "Puesto")]
        [Required(ErrorMessage = "Debe seleccionar un puesto para el empleado")]
        public int IdPuesto { get; set; }
        [Display(Name = "Salario Mensual")]
        [Required(ErrorMessage = "Debe asignar un salario al empleado")]
        public decimal? SalarioMensual { get; set; }

        public virtual Departamentos IdDepartamentoNavigation { get; set; }
        public virtual Puestos IdPuestoNavigation { get; set; }
    }
}
