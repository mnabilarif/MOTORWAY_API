using MOTORWAY_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOTORWAY_API.Repository
{
    public class MotorwayRepository
    {
        MOTORWAY_API_DBEntities db = new MOTORWAY_API_DBEntities();
        public bool AddEntry(vmEntryExit Data, string UserId)
        {
            try
            {
                var NewEntry = new tbl_VRNEntryExit()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedOn = DateTime.Now,
                    CreatedBy = UserId,
                    VRN = Data.VRN,
                    EntryTime = Data.Time,
                    EntryAt = UserId

                    //EntryAt = Data.StationId
                };

                db.tbl_VRNEntryExit.Add(NewEntry);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public decimal GetPositiveDistance(decimal Distance)
        {
            return Distance >= 0 ? ++Distance : --Distance;
        }

        public vmCalculation AddExit(vmEntryExit Data, string UserId)
        {

            var Settings = db.CT_Setting.FirstOrDefault();

            if (Settings != null)
            {
                var BaseRate = Settings.BaseRate;
                var PerKMRate = Settings.PerKMRate;
                var WeekendRate = Settings.WeekendsPercentage;
                var HolidayDiscount = Settings.HolidayDiscountPercentage;
                var VrnDiscount = Settings.VRNDiscount;

                decimal SubTotal = 0;

                decimal TotalDiscount = 0;
                decimal Total = 0;
                decimal DiscountPercent = 0;
                string CostBreakDown = "Base: 20Rs , ";


                var Entry = db.tbl_VRNEntryExit.Where(x => x.VRN == Data.VRN && x.ExitAt == null).OrderByDescending(x => x.CreatedOn).FirstOrDefault();

                if (Entry != null)
                {
                    var EntryPoint = Entry.EntryAt;
                    var EntryDistance = Entry.AspNetUser.Distance;
                    Entry.ExitTime = Data.Time;  // frocalculation at entry point requirement
                    //Entry.ExitAt = Data.StationId;
                    Entry.ExitAt = UserId;

                    db.SaveChanges(); //

                    var EntryTime = (DateTime)Entry.EntryTime;
                    var ExitDistance = db.AspNetUsers.Where(x => x.Id == UserId).FirstOrDefault().Distance;

                    var TotalDistance = ExitDistance - EntryDistance;
                    var VRNNumber = Convert.ToInt32(Data.VRN.Split('-')[1]);  //get last three digits

                    if ((EntryTime.Day == 23 && Data.Time.Month == 3) || (EntryTime.Day == 25 && EntryTime.Month == 12) || (Data.Time.Day == 14 && Data.Time.Month == 8))
                    {
                        //50%
                        //weekend also
                        if (EntryTime.DayOfWeek == DayOfWeek.Saturday || EntryTime.DayOfWeek == DayOfWeek.Sunday)
                        {
                            //weekend peak
                            SubTotal = BaseRate + ((PerKMRate * (decimal)WeekendRate) * GetPositiveDistance((decimal)TotalDistance));
                            Total = SubTotal * (decimal)HolidayDiscount;
                            TotalDiscount = SubTotal - Total;
                            DiscountPercent = (decimal)HolidayDiscount;
                            CostBreakDown += "Distance: "+GetPositiveDistance((decimal)TotalDistance).ToString() + "  Km , Weekend Peak Rate : "+ (WeekendRate).ToString()
                                +" Holiday Discount:" +HolidayDiscount.ToString()
                                ;
                        }
                        else  //no peak / no discount
                        {
                            SubTotal = BaseRate + (PerKMRate * GetPositiveDistance((decimal)TotalDistance));
                            Total = SubTotal * (decimal)HolidayDiscount;

                            CostBreakDown += "Distance: " + GetPositiveDistance((decimal)TotalDistance).ToString() + " Km ,  Normal Day Rate : " + PerKMRate.ToString()
                                + " Holiday Discount:" + HolidayDiscount.ToString()
                                ;
                        }

                    }
                    else if (EntryTime.DayOfWeek == DayOfWeek.Saturday || EntryTime.DayOfWeek == DayOfWeek.Sunday)
                    {
                        //fare 1.5 peak /km

                        SubTotal = BaseRate + ((PerKMRate * (decimal)WeekendRate) * GetPositiveDistance((decimal)TotalDistance));
                        Total = SubTotal ;

                        CostBreakDown += "Distance: " + GetPositiveDistance((decimal)TotalDistance).ToString() + " Km , Weekend Peak Rate : " + (WeekendRate).ToString()
                            + " Holiday Discount:" + HolidayDiscount.ToString()
                            ;
                    }
                    else if (EntryTime.DayOfWeek == DayOfWeek.Monday || EntryTime.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        if (VRNNumber % 2 == 0)
                        {
                            //Even discount
                            SubTotal = BaseRate + (PerKMRate * GetPositiveDistance((decimal)TotalDistance));
                            Total = SubTotal * (decimal)VrnDiscount;

                            TotalDiscount = SubTotal - Total;
                            DiscountPercent = (decimal)VrnDiscount;

                            CostBreakDown += "Distance: " + GetPositiveDistance((decimal)TotalDistance).ToString() + "Km ,"
                                + " VRN EVEN Discount:" + VrnDiscount.ToString()
                                ;
                        }
                        else
                        {
                            //Odd  no discoutn
                            SubTotal = BaseRate + (PerKMRate * GetPositiveDistance((decimal)TotalDistance));
                            Total = SubTotal;
                            CostBreakDown += " Distance: " + GetPositiveDistance((decimal)TotalDistance).ToString() + "Km ,"
                               ;
                        }
                    }
                    else if (EntryTime.DayOfWeek == DayOfWeek.Tuesday || EntryTime.DayOfWeek == DayOfWeek.Thursday)
                    {
                        if (VRNNumber % 2 == 0)
                        {
                            //Even
                            //Odd  no discoutn
                            SubTotal = BaseRate + (PerKMRate * GetPositiveDistance((decimal)TotalDistance));
                            Total = SubTotal;
                            CostBreakDown += " Distance: " + GetPositiveDistance((decimal)TotalDistance).ToString() + "Km ,"
                               ;
                        }
                        else
                        {
                            //Odd discount

                            SubTotal = BaseRate + (PerKMRate * GetPositiveDistance((decimal)TotalDistance));
                            Total = SubTotal * (decimal)VrnDiscount;

                            TotalDiscount = SubTotal - Total;
                            DiscountPercent = (decimal)VrnDiscount;

                            CostBreakDown += "Distance: " + GetPositiveDistance((decimal)TotalDistance).ToString() + "Km ,"
                                + " VRN ODD Discount:" + VrnDiscount.ToString()
                                ;
                        }
                    }

                    string From = db.AspNetUsers.Where(x => x.Id == Entry.EntryAt).FirstOrDefault().InterchangeName;
                    string To = db.AspNetUsers.Where(x => x.Id == Entry.ExitAt).FirstOrDefault().InterchangeName;

                    //response back to UI
                    var Calculation = new vmCalculation()
                    {
                        BaseRate = BaseRate,
                        DistanceCostBreakdown = CostBreakDown +" ... From:"+From+" > TO> "+To,
                        SubTotal = SubTotal,
                        OtherDiscout = TotalDiscount,
                        Total = Total
                    };

                    return Calculation;
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }
        }
    }
}