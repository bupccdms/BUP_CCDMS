//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace qms.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblServiceStatu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblServiceStatu()
        {
            this.tblTokenQueues = new HashSet<tblTokenQueue>();
        }
    
        public short service_status_id { get; set; }
        public string service_status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblTokenQueue> tblTokenQueues { get; set; }
    }
}
