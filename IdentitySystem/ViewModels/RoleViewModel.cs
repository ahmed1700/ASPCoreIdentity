using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentitySystem.ViewModels
{
    public class RoleViewModel
    {

        public RoleViewModel()
        {
            Users = new List<string>();
        }
        public string Id { get; set; }
        [Required]
        [Display(Name ="Role")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
