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
    
    public partial class Debt
    {
        public int debtID { get; set; }
        public Nullable<int> userID { get; set; }
        public string debtName { get; set; }
        public Nullable<decimal> debtAmount { get; set; }
        public Nullable<System.DateTime> debtDueDate { get; set; }
        public string description { get; set; }
        public Nullable<bool> isDebt { get; set; }
        public Nullable<int> debtStatus { get; set; }
        public Nullable<int> repaymentPlan { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }
    
        public virtual User User { get; set; }
    }
}