using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nomina.Models
{
    public class EditIdentityModel
    {
        public IdentityUser User { get; set; }
        public IList<SelectListItem> Roles { get; set; }
    }
}
