using Microsoft.AspNet.Identity;
using MOTORWAY_API.Models;
using MOTORWAY_API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace MOTORWAY_API.Controllers
{

    [Authorize]
    [RoutePrefix("api/Motorway")]
    public class MotorwayController : ApiController
    {

        public JsonResult<bool> AddEntry(vmEntryExit Data)
        {
            var Id = RequestContext.Principal.Identity.GetUserId();

            MotorwayRepository M = new MotorwayRepository();

            return Json(M.AddEntry(Data, Id));
        }

        public JsonResult<vmCalculation> AddExit(vmEntryExit Data)
        {
            var Id = RequestContext.Principal.Identity.GetUserId();

            MotorwayRepository M = new MotorwayRepository();

            return Json(M.AddExit(Data, Id));
        }


        //Bismillah
        public JsonResult<bool> Test()
        {
            return Json(true);
        }

        //// GET: api/Motorway
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Motorway/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Motorway
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/Motorway/5
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/Motorway/5
        //public void Delete(int id)
        //{
        //}
    }
}
