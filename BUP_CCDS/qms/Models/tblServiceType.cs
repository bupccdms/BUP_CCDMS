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
    
    public partial class tblServiceType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblServiceType()
        {
            this.tblTokenQueues = new HashSet<tblTokenQueue>();
            this.tblServiceSubTypes = new HashSet<tblServiceSubType>();
        }
    
        public int service_type_id { get; set; }
        public string service_type_name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblTokenQueue> tblTokenQueues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblServiceSubType> tblServiceSubTypes { get; set; }
    }
}