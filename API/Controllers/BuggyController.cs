using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseAPIController
    {
        private readonly StoreContext context;

        public BuggyController(StoreContext storeContext)
        {
            context = storeContext;
        }

        [HttpGet("notfound")]
        public IActionResult GetNotFoundRequest()
        {
            var thing = context.Products.Find(1142);

            if(thing == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return NotFound(new ApiResponse(404));
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var thing = context.Products.Find(42);

            var thingToReturn = thing.ToString();

            return Ok();
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }

        [HttpGet("testauth")]
        [Authorize]
        public ActionResult<string> GetSecretText()
        {
            return "secret stuff";
        }
    }
}