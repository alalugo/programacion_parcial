using Microsoft.AspNetCore.Mvc.Rendering;
using Nomina.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nomina.Models
{
    public class IndexEmpleadosModel
    {
        public IEnumerable<Empleados> Empleados { get; set; }
        public IList<SelectListItem> Filter { get; set; }
        public string SelectedFilter { get; set; }
        public string TextFilter { get; set; }
    }
}
