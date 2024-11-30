using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/[controller]")]

public class BuggyController : BaseApiController
{
    private readonly StoreContext _context;

    public BuggyController(StoreContext context)
    {
        _context = context;
    }

    [HttpGet("testauth")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult<string> TestAuth()
    {
        return "You are authorized";
    }

    [HttpGet("NotFound")]
    public ActionResult HandleNotFoundRequest()
    {
        return NotFound(new ApiResponse(404));
    }

    [HttpGet("ServerError")]
    public ActionResult HandleServerError()
    {
        throw new Exception("This is a server error");
    }

    [HttpGet("BadRequest")]
    public ActionResult HandleBadRequest()
    {
        return BadRequest(new ApiResponse(400));
    }

    [HttpGet("BadRequest/{id}")]
    public ActionResult HandleBadRequest(int id)
    {
        return Ok();
    }
}