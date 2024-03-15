using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Graph;
using Microsoft.Kiota.Abstractions.Authentication;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph.Models;

namespace GraphCalendar.Host.Controllers;


[ApiController]
// [Produces("application/json")]
[Route("api/[controller]/[action]")]
public class CalendarController : ControllerBase
{
    private readonly GraphServiceClient _graphClient;

    public CalendarController(GraphServiceClient graphClient)
    {
        _graphClient = graphClient;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeamsMeeting()
    {
        var requestBody = new OnlineMeeting
        {
            StartDateTime = DateTimeOffset.UtcNow,
            EndDateTime = DateTimeOffset.UtcNow.AddHours(1),
            Subject = "Test Meeting",
            Participants = new MeetingParticipants()
            {
                Attendees = new List<MeetingParticipantInfo>()
                {
                    new()
                    {
                        //Upn = user email
                        Upn = "vsanyinclude@gmail.com",
                        Role = OnlineMeetingRole.Attendee
                    }
                }
            }
        };

        var newEvent = await _graphClient.Users["0de6011c-83d2-406c-92a6-10b3a69a04ba"].OnlineMeetings.PostAsync(requestBody);
        Console.WriteLine($"Event created with ID {newEvent.Id}");
        return Ok(newEvent);
    }
}
