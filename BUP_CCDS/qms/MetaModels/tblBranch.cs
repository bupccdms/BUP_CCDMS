using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.Models
{
    [MetadataType(typeof(DepartmentMeta))]
    public partial class tblDepartment
    {
    }

    public class DepartmentMeta
    {
        [Display(Name ="Department Name")]
        [Required]
        [StringLength(150)]
        public string department_name { get; set; }

        [Display(Name = "Faculty Name")]
        [StringLength(250)]
        public string address { get; set; }

        [Display(Name = "Contact Person")]
        [StringLength(150)]
        public string contact_person { get; set; }

        [Display(Name = "Contact No")]
        [StringLength(50)]
        public string contact_no { get; set; }

        [Display(Name = "Next Displays")]
        [Required]
        public int display_next { get; set; }

        [Display(Name = "Static IP")]
        [StringLength(20)]
        public string static_ip { get; set; }
        public int is_active { get; set; }
    }
}