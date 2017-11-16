using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AddressBook.Helpers
{
    /// <summary>
    /// JsonResult always returns HttpStatus code 200 even when there are errors.
    /// This class will be used to return code 400 on JsonResult
    /// </summary>
    public class JsonBadRequest : JsonResult
    {
        public JsonBadRequest()
        {
            
        }
        
        public JsonBadRequest(string message)
        {
            this.Data = message;
        }

        public JsonBadRequest(object data)
        {
            this.Data = data;
        }
        
        public JsonBadRequest(string message, JsonRequestBehavior jsonRequestBehaviour)
            : this(message)
        {
            this.JsonRequestBehavior = jsonRequestBehaviour;
        }

        public JsonBadRequest(object data, JsonRequestBehavior jsonRequestBehaviour)
           : this(data)
        {
            this.JsonRequestBehavior = jsonRequestBehaviour;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.RequestContext.HttpContext.Response.StatusCode = 400;
            base.ExecuteResult(context);
        }
    }
}