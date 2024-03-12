using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace GraphCalendar;

class Program
{
    // Graph API: https://developer.microsoft.com/en-us/graph/graph-explorer#
    
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        
        
        // The client credentials flow requires that you request the
        // /.default scope, and pre-configure your permissions on the
        // app registration in Azure. An administrator must grant consent
        // to those permissions beforehand.
        var scopes = new[] {"https://graph.microsoft.com/.default"};
        
        
        //userId is the object ID of a user in Microsoft Entra admin center > user management page
        //See more details in:
        //https://entra.microsoft.com/#view/Microsoft_AAD_UsersAndTenants/UserProfileMenuBlade/~/overview/userId/0de6011c-83d2-406c-92a6-10b3a69a04ba/hidePreviewBanner~/true
        var userId = config["Azure:UserId"];
        var tenantId = config["Azure:TenantId"];
        var clientId = config["Azure:ClientId"];
        var clientSecret = config["Azure:ClientSecret"];

        var options = new ClientSecretCredentialOptions {AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,};

        var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret, options);

        var graphClient = new GraphServiceClient(clientSecretCredential, scopes);
        
        try
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
        
            var result = await graphClient.Users[userId].OnlineMeetings.PostAsync(requestBody);
            Console.WriteLine($"Online meeting created. Join URL: {result.JoinWebUrl}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }
}