﻿using DataAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvcApplication1.Controllers
{
    public class ValuesController : ApiController
    {
        internal static HubClient c = new HubClient(Constants.Host, Clients.Server);
        internal static List<FilePart> parts = new List<FilePart>();
        internal static int globalID = 0;
        internal static FileInfo finfo;

        //GET api/values/GetFileInfo
        [HttpGet]
        [ActionNameAttribute("GetFileInfo")]
        public FileInfo GetFileInfo()
        {
            return finfo;
            //using (DataBaseContext context = new DataBaseContext())
            //{
            //    var f = context.Files.FirstOrDefault();
            //    if (f != null)
            //    {
            //        return f;
            //    }
            //    return null;
            //}
        }

        //POST api/values/PostFileInfo
        [HttpPost]
        [ActionNameAttribute("PostFileInfo")]        
        public void PostFileInfo(FileInfo info)
        {            
            //using (DataBaseContext context = new DataBaseContext())
            //{
            //    context.Files.Add(info);
            //    context.SaveChanges();
            //}
            finfo = info;
            c.SendMessage(new MsgData { From = Clients.Server, To = Clients.Downloader, Message = Messages.FileInfoAvailable });       
        }

        // DELTE api/values/RemoveFileInfo/{id}
        [HttpPut]
        [ActionNameAttribute("RemoveFileInfo")]        
        public void DeleteFileInfo(long id)
        {
            finfo = null;
            //using (DataBaseContext context = new DataBaseContext())
            //{
            //    var file = context.Files.FirstOrDefault(p => p.Id == id);
            //    if (file != null)
            //    {
            //        context.Files.Remove(file);
            //        context.SaveChanges();
            //    }
            //}
        }





        // GET api/values/GetPart
        [HttpGet]
        [ActionNameAttribute("GetPart")]
        public FilePart GetPart(long id)
        {
            var part = parts.FirstOrDefault(p => p.Id == id);
            return part;            

            //using (DataBaseContext context = new DataBaseContext())
            //{
            //    var part = context.Parts.FirstOrDefault(p => p.Id == id);
            //    return part;
            //}
        }      

        // POST api/values/AddPart
        [HttpPost]
        [ActionNameAttribute("AddPart")]
        public void Post(FilePart newPart)
        {
            newPart.Id = ++globalID;
            parts.Add(newPart);
            c.SendMessage(new MsgData { From = Clients.Server, To = Clients.Downloader, Message = string.Format("{0}{1}", newPart.Id, Messages.DownloadAvailable) });
            if (parts.Count() > 4) //>4
                c.SendMessage(new MsgData { From = Clients.Server, To = Clients.Uploader, Message = Messages.PauseUploading });

            //using (DataBaseContext context = new DataBaseContext())
            //{
            //    context.Configuration.AutoDetectChangesEnabled = false;
            //    context.Configuration.ValidateOnSaveEnabled = false;
            //    context.Parts.Add(newPart);
            //    context.SaveChanges();
            //    c.SendMessage(new MsgData { From = Clients.Server, To = Clients.Downloader, Message = string.Format("{0}{1}", newPart.Id, Messages.DownloadAvailable) });
            //    if (context.Parts.Count() > 2)
            //    {
            //        c.SendMessage(new MsgData { From = Clients.Server, To = Clients.Uploader, Message = Messages.PauseUploading });
            //    }
            //}
        }

        [HttpPut]
        [ActionNameAttribute("RemovePart")]
        public void RemovePart(long id)
        {
            var part = parts.FirstOrDefault(p => p.Id == id);
            parts.Remove(part);
            if (parts.Count <= 1) //<=1
            {
                c.SendMessage(new MsgData { From = Clients.Server, To = Clients.Uploader, Message = Messages.ContinueUploading });
            }

            //using (DataBaseContext context = new DataBaseContext())
            //{
            //    var part = context.Parts.FirstOrDefault(p => p.Id == id);
            //    if (part != null)
            //    {                    
            //        context.Parts.Remove(part);
            //        context.SaveChanges();
            //    }
            //    if (!context.Parts.Any())
            //    {
            //        c.SendMessage(new MsgData { From = Clients.Server, To = Clients.Uploader, Message = Messages.ContinueUploading });
            //    }
            //}
        }

        [HttpPut]
        [ActionNameAttribute("ClearDb")]
        public void ClearDb()
        {
            //using (DataBaseContext context = new DataBaseContext())
            //{
            //    var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext;
            //    objCtx.ExecuteStoreCommand("TRUNCATE TABLE [Parts]");
            //    objCtx.ExecuteStoreCommand("TRUNCATE TABLE [Files]");
            //}

            parts.Clear();
            globalID = 0;
        }

        [HttpGet]
        [ActionNameAttribute("GetDbState")]
        public string GetDbState()
        {
            return string.Format("Global id:{0};  Parts in memory: {1};", globalID, parts.Count);
        }

    }
}