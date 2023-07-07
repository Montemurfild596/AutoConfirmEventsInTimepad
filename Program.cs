using System.Text;
using Microsoft.AspNetCore.Mvc;
using AutoConfirmEventsInTimepad.Models;
using AutoConfirmEventsInTimepad.Loggers;
using TimepadClient;
using System.Text.Json;
using MimeKit;
using MailKit.Net.Smtp
;


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
        if (webHookTicket != null)
        {
            var currentEmail = webHookTicket.email;
            if (webHookTicket.status_raw == "pending")
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
            else if (webHookTicket.status_raw == "ok")
            {
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(webHookTicket.email, webHookTicket.event_id.ToString(), $"Добрый день!\n\nВаша регистрация на событие подтверждена!", $"{webHookTicket.surname} {webHookTicket.name}");
            }
            //await response.WriteAsync(email.ToString());
        }
    }
    catch
    {}
});

app.Run();


// ValueTask<WebHookTicket> DeserializeWebHookTicketBody<T>(Stream stream)
// {
//     try
//     {
//         return JsonSerializer.DeserializeAsync<WebHookTicket>(stream);
//     }
//     catch (JsonException e)
//     {
//         throw new Exception(e.Message);
//     }
// }