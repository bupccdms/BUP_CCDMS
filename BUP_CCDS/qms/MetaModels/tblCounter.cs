using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.Models
{
    [MetadataType(typeof(DeviceMeta))]
    public partial class tblDevice
    {
    }

    public class DeviceMeta
    {
        [Display(Name = "Device No")]
        [Required]
        [StringLength(5)]
        public string device_no { get; set; }

        [Display(Name = "Location")]
        [StringLength(250)]
        public string location { get; set; }

    }
}