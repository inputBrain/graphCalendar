using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Graph;
using Microsoft.Kiota.Abstractions.Authentication;


namespace GraphCalendar.Host.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CalendarController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}