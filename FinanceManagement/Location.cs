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
    
    public partial class Location
    {
        public int locationID { get; set; }
        public string locationNo { get; set; }
        public string name { get; set; }
        public string parent { get; set; }
        public Nullable<int> levels { get; set; }
        public string slug { get; set; }
        public string nameWithType { get; set; }
        public Nullable<int> type { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }
    }
}