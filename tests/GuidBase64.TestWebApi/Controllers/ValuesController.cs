namespace GuidBase64.TestWebApi.Controllers {

    using System;
    using Microsoft.AspNetCore.Mvc;

    [Route( "api/values" )]
    [ApiController]
    public class ValuesController : ControllerBase {

        [HttpGet]
        public ActionResult<String> GetWithQueryParameter( Base64Guid id ) => id.Guid.ToString();

        [HttpGet( "{id}" )]
        public ActionResult<String> GetWithRouteParameter( Base64Guid id ) => id.Guid.ToString();

    }

}