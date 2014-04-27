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


        // POST api/values/AddPart
        [HttpPost]
        [ActionNameAttribute("AddPart")]
        public void Post(FilePart newPart)
        {
            newPart.Id = ++ValuesController.globalID;
            ValuesController.parts.Add(newPart);
            ValuesController.c.SendMessage(new MsgData { From = Clients.Server, To = Clients.Downloader, Message = string.Format("{0}{1}", newPart.Id, Messages.DownloadAvailable) });
            if (ValuesController.parts.Count() > 4)
                ValuesController.c.SendMessage(new MsgData { From = Clients.Server, To = Clients.Uploader, Message = Messages.PauseUploading });            
        }
    }
}