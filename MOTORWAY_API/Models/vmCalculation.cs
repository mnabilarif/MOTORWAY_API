using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOTORWAY_API.Models
{
    public class vmCalculation
    {
        public decimal BaseRate { get; set; }
        public string DistanceCostBreakdown { get; set; }
        public decimal SubTotal { get; set; }
        public decimal OtherDiscout{ get; set; }
        public decimal Total { get; set; }


    }
}