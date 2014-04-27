using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MvcApplication1.Controllers
{
    public class TestControllers : ApiController
    {

        // GET api/values/AnyPart
        [HttpGet]
        [ActionNameAttribute("GetPart")]
        public FilePart GetPart(long id)
        {
            var part = ValuesController.parts.FirstOrDefault(p => p.Id == id);
            return part;          
        }      
    }
}