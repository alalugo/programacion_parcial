using Nomina.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nomina.Models
{
    public class DeleteEmpleadoModel: Empleados
    {
        public List<Departamentos> Departamentos { get; set; }
        public List<Puestos> Puestos { get; set; }
    }
}
