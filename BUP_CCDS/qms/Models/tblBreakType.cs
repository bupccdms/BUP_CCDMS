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
    
    public partial class tblBreakType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblBreakType()
        {
            this.tblDailyBreaks = new HashSet<tblDailyBreak>();
        }
    
        public int break_type_id { get; set; }
        public string break_type_short_name { get; set; }
        public string break_type_name { get; set; }
        public Nullable<System.DateTime> start_time { get; set; }
        public Nullable<System.DateTime> end_time { get; set; }
        public int duration { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblDailyBreak> tblDailyBreaks { get; set; }
    }
}
