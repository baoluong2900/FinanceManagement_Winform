//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FinanceManagement
{
    using System;
    using System.Collections.Generic;
    
    public partial class Analysis
    {
        public int analysisID { get; set; }
        public Nullable<int> userID { get; set; }
        public string analysisName { get; set; }
        public string analysisDescription { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }
    
        public virtual User User { get; set; }
    }
}
