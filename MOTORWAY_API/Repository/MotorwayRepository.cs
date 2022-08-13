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
                    EntryAt = Data.StationId
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
                var WeekendPercentage = Settings.WeekendsPercentage;

                decimal SubTotal = 0;
                decimal TotalDiscount = 0;


                var Entry = db.tbl_VRNEntryExit.Where(x => x.VRN == Data.VRN && x.ExitAt == null).OrderByDescending(x => x.CreatedOn).FirstOrDefault();

                if (Entry != null)
                {
                    var EntryPoint = Entry.EntryAt;
                    var EntryDistance = Entry.AspNetUser.Distance;
                    Entry.ExitTime = Data.Time;
                    Entry.ExitAt = Data.StationId;

                    db.SaveChanges();

                    var EntryTime = (DateTime)Entry.EntryTime;
                    var ExitDistance = db.AspNetUsers.Where(x => x.Id == Data.StationId).FirstOrDefault().Distance;

                    var TotalDistance = ExitDistance - EntryDistance;

                    if ((EntryTime.Day == 23 && Data.Time.Month == 3) || (EntryTime.Day == 25 && EntryTime.Month == 12) || (Data.Time.Day == 14 && Data.Time.Month == 8))
                    {
                        //50%
                        //weekend also
                        if (EntryTime.DayOfWeek == DayOfWeek.Saturday || EntryTime.DayOfWeek == DayOfWeek.Sunday)
                        {
                            PerKMRate = PerKMRate * (decimal)1.5;
                            TotalDiscount = 50;
                            SubTotal = PerKMRate * BaseRate * 50 / 100;
                        }
                        else
                        {
                            TotalDiscount = 50;
                            SubTotal = PerKMRate * BaseRate * 50 / 100;
                        }

                    }
                    else if (EntryTime.DayOfWeek == DayOfWeek.Saturday || EntryTime.DayOfWeek == DayOfWeek.Sunday)
                    {
                        PerKMRate = PerKMRate * (decimal)1.5;
                        TotalDiscount = 50;
                        SubTotal = PerKMRate * BaseRate * 50 / 100;

                    }
                    else if (EntryTime.DayOfWeek == DayOfWeek.Monday || EntryTime.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        SubTotal = BaseRate + (PerKMRate * GetPositiveDistance((decimal)TotalDistance));

                    }
                    else if (EntryTime.DayOfWeek == DayOfWeek.Tuesday || EntryTime.DayOfWeek == DayOfWeek.Thursday)
                    {
                        var VRNNumber = Convert.ToInt32(Data.VRN.Split('-')[1]);

                        if (VRNNumber % 2 == 0)
                        {
                            //Even
                            SubTotal = BaseRate + (PerKMRate * GetPositiveDistance((decimal)TotalDistance) );
                        }
                        else
                        {
                            //Odd
                            SubTotal = BaseRate + (PerKMRate * GetPositiveDistance((decimal)TotalDistance) * 10 / 100);
                        }

                        var Calculation = new vmCalculation()
                        {
                            BaseRate = BaseRate,
                            OtherDiscout = TotalDiscount,
                            SubTotal = TotalDiscount,
                        };

                        return Calculation;


                    }



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

            return null;
        }
    }
}