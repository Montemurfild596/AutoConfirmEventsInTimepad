using System.Text;
using Microsoft.AspNetCore.Mvc;
using AutoConfirmEventsInTimepad.Models;
using AutoConfirmEventsInTimepad.Loggers;
using TimepadClient;
using System.Text.Json;

ValueTask<WebHookTicket> DeserializeWebHookTicketBody<T>(Stream stream)
{
    try
    {
        return JsonSerializer.DeserializeAsync<WebHookTicket>(stream);
    }
    catch (JsonException e)
    {
        throw new Exception(e.Message);
    }
}

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
var app = builder.Build();

List<string> emailDomens = new List<string>
{
    //"@timepath.ru", 
    //"@pminst.ru"
    "@gmail.com"
};


app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    TimepadClient.TimepadClient timepadClient = new TimepadClient.TimepadClient(new HttpClient());
    //var abc = timepadClient.GetHooksAsync(105926, "ticket_change");
    //await response.WriteAsJsonAsync(abc);
    try
    {
        var webHookTicket = await request.ReadFromJsonAsync<WebHookTicket>();
        //var requestBody = request.Body;
        //app.Logger.LogInformation($"Info: {webHookTicket!.ToString()} Time: {DateTime.Now.ToLongTimeString()}");
        if (webHookTicket != null)
        {
            var currentEmail = webHookTicket.email;
            //var payload = Newtonsoft.Json.JsonConvert.DeserializeObject<WebHookTicket>(request);
            //WebHookTicket ticket = new WebHookTicket("5184211:83845994", 215813, 29963, "4955686", "2015-07-24 19:04:37", 361138, "забронировано", "booked", "test-mail@ya.ru", "Смирнов", "Владимир", false, "83845994", "83845994", 1000, null);
            //await response.WriteAsJsonAsync(ticket);
            if (webHookTicket.status == "pending")
            {
                StringBuilder email = new StringBuilder(currentEmail);
                email.Remove(0, currentEmail!.IndexOf('@'));
                if (emailDomens.Contains(email.ToString()))
                {
                    await timepadClient.ApproveEventOrderAsync(webHookTicket.event_id, int.Parse(webHookTicket.order_id!));
                }
                else
                {
                    await timepadClient.RejectEventOrderAsync(webHookTicket.event_id, int.Parse(webHookTicket.order_id!));
                }
                //return StatusCodeResult
            }
            
            //await response.WriteAsync(email.ToString());
            
            
        }
    }
    catch
    {}
});

app.Run();