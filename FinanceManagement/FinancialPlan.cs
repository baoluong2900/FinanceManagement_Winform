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
    
    public partial class FinancialPlan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FinancialPlan()
        {
            this.FinancialPlanDetails = new HashSet<FinancialPlanDetail>();
        }
    
        public int financialPlanID { get; set; }
        public Nullable<int> userID { get; set; }
        public string financialPlanName { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public string description { get; set; }
        public Nullable<decimal> progress { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<int> cateogryID { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }
    
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FinancialPlanDetail> FinancialPlanDetails { get; set; }
    }
}
