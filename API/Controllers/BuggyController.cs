using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController: BaseApiController 
{
    private readonly StoreContext _context;

    public BuggyController(StoreContext context)
    {
        _context = context;
    }

    [HttpGet("testauth")]
    [Authorize]
    public ActionResult<string> TestAuth()
    {
        return "Hello World!";
    }
     
    [HttpGet("NotFound")]
    public ActionResult GetNotFoundRequest()
    {
        return NotFound(new ApiResponse(404));
    }
    [HttpGet("ServerError")]
    public ActionResult GetServerErrorRequest()
    {
        return Ok();
    }
    [HttpGet("BadRequest")]
    public ActionResult GetBadRequestRequest()
    {
        return BadRequest(new ApiResponse(400));
    }
    [HttpGet("BadRequest/{id}")]
    public ActionResult GetNotFoundRequest(int id)
    {
        return Ok();
    }
    
}