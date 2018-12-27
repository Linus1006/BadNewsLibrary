using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using BadNewsEngine.Service;
using BadNewsEngine.Models;
using System.Web;

namespace BadNewsEngine.Controllers
{
    public class ELandEtlController : ApiController
    {
        public int Get()
        {
            HttpContext.Current.Server.ScriptTimeout = 300000;

            return ELandDataHelper.FastEtl();
        }
    }
}
