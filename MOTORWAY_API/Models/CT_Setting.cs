//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MOTORWAY_API.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CT_Setting
    {
        public int Id { get; set; }
        public decimal BaseRate { get; set; }
        public decimal PerKMRate { get; set; }
        public decimal WeekendsPercentage { get; set; }
        public Nullable<decimal> HolidayDiscountPercentage { get; set; }
        public Nullable<decimal> VRNDiscount { get; set; }
    }
}
