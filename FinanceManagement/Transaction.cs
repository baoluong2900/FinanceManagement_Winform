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
    
    public partial class Transaction
    {
        public int transactionID { get; set; }
        public string transactionName { get; set; }
        public Nullable<int> userID { get; set; }
        public string description { get; set; }
        public Nullable<int> categoryID { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
    
        public virtual User User { get; set; }
    }
}
